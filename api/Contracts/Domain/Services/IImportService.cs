using Nettolicious.Tickets.Contracts.Domain.Model;
using Nettolicious.Tickets.Contracts.Domain.Requests;
using Nettolicious.Tickets.Contracts.Domain.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Nettolicious.Tickets.Contracts.Domain.Services {
  public interface IImportService {
    Task<DomainResponse<ImportCsvResponse>> ImportCsvAsync(ImportCsvRequest request, CancellationToken cancellationToken = default);
  }
}