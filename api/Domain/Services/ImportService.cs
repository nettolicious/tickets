using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nettolicious.Tickets.Contracts.Domain.Model;
using Nettolicious.Tickets.Contracts.Domain.Requests;
using Nettolicious.Tickets.Contracts.Domain.Responses;
using Nettolicious.Tickets.Contracts.Domain.Services;
using Nettolicious.Tickets.Contracts.Persistence.Entities;
using Nettolicious.Tickets.Persistence;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nettolicious.Tickets.Domain.Services {
  public class ImportService : IImportService {
    private readonly ILogger<ImportService> mLogger;
    private readonly TicketsDbContext mDbContext;

    public ImportService(ILogger<ImportService> logger, TicketsDbContext dbContext) {
      mLogger = logger ?? throw new ArgumentNullException(nameof(logger));
      mDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<DomainResponse<ImportCsvResponse>> ImportCsvAsync(ImportCsvRequest request, 
      CancellationToken cancellationToken = default) {
      if (request is null) {
        throw new ArgumentNullException(nameof(request));
      }
      using var streamReader = new StreamReader(request.InputStream, Encoding.UTF8);
      var line = await streamReader.ReadLineAsync();
      var response = new ImportCsvResponse();
      do {
        mLogger.LogDebug("Processing line: {0}", line);
        if (!string.IsNullOrWhiteSpace(line)) {
          if (await ProcessLineAsync(line, cancellationToken)) {
            response.RowsImported++;
          }
        }
        line = await streamReader.ReadLineAsync();
      }
      while (line != null);
      return new DomainResponse<ImportCsvResponse>(HttpStatusCode.OK, data: response);
    }

    private async Task<bool> ProcessLineAsync(string line, CancellationToken cancellationToken) {
      if (string.IsNullOrWhiteSpace(line)) {
        throw new ArgumentException($"'{nameof(line)}' cannot be null or whitespace", nameof(line));
      }
      if (line.Contains("OrderId")) {
        return false;
      }
      if (!TryParseLine(line, out var parseResult)) {
        return false;
      }
      var order = await mDbContext.Orders.SingleOrDefaultAsync(x => x.ImportOrderId == parseResult.order.ImportOrderId, 
        cancellationToken);
      if (order == null) {
        var orderRef = mDbContext.Orders.Add(parseResult.order);
        await mDbContext.SaveChangesAsync(cancellationToken);
        order = orderRef.Entity;
      }
      parseResult.ticket.OrderId = order.OrderId;
      var ticket = await mDbContext.Tickets.SingleOrDefaultAsync(
        x => x.TicketNumber == parseResult.ticket.TicketNumber, cancellationToken);
      if (ticket == null) {
        mDbContext.Tickets.Add(parseResult.ticket);
        await mDbContext.SaveChangesAsync(cancellationToken);
      }
      return true;
    }

    private bool TryParseLine(string line, out (Order order, Ticket ticket) parseResult) {
      string[] parts = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
      var order = new Order {
        ImportOrderId = int.Parse(parts[0])
      };
      var ticket = new Ticket {
        FirstName = parts[1],
        LastName = parts[2],
        TicketNumber = parts[3]
      };
      if (DateTimeOffset.TryParse(parts[4], out DateTimeOffset eventDate)) {
        ticket.EventDate = eventDate;
      }
      parseResult = (order, ticket);
      return true;
    }
  }
}
