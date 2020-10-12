using Nettolicious.Tickets.Contracts.Domain.Model;
using Nettolicious.Tickets.Contracts.Domain.Requests;
using Nettolicious.Tickets.Contracts.Domain.Responses;
using Nettolicious.Tickets.Contracts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nettolicious.Tickets.Domain.Services {
  public class ImportService : IImportService {

    public async Task<DomainResponse<ImportCsvResponse>> ImportCsvAsync(ImportCsvRequest request, 
      CancellationToken cancellationToken = default) {
      return new DomainResponse<ImportCsvResponse>(HttpStatusCode.OK);
    }
  }
}
