using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nettolicious.Tickets.Contracts.Domain.Requests;
using Nettolicious.Tickets.Contracts.Domain.Services;

namespace Nettolicious.Tickets.API.Controllers {
  [ApiController]
  [Route("import")]
  public class ImportController : ControllerBase {
    private readonly ILogger<ImportController> mLogger;
    private readonly IImportService mImportService;

    public ImportController(ILogger<ImportController> logger, IImportService importService) {
      mLogger = logger ?? throw new ArgumentNullException(nameof(logger));
      mImportService = importService ?? throw new ArgumentNullException(nameof(importService));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CancellationToken cancellationToken) {
      var domainRequest = new ImportCsvRequest();
      var result = await mImportService.ImportCsvAsync(domainRequest, cancellationToken);
      return new StatusCodeResult((int)result.Status);
    }
  }
}
