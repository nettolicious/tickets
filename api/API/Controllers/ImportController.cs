using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Nettolicious.Tickets.API.Attributes;
using Nettolicious.Tickets.Contracts.Domain.Requests;
using Nettolicious.Tickets.Contracts.Domain.Services;

namespace Nettolicious.Tickets.API.Controllers {
  [ApiController]
  [Route("import")]
  public class ImportController : ControllerBase {
    private readonly ILogger<ImportController> mLogger;
    private readonly IImportService mImportService;
    private readonly IHttpHelper mHttpHelper;

    public ImportController(ILogger<ImportController> logger, IImportService importService, IHttpHelper httpHelper) {
      mLogger = logger ?? throw new ArgumentNullException(nameof(logger));
      mImportService = importService ?? throw new ArgumentNullException(nameof(importService));
      mHttpHelper = httpHelper ?? throw new ArgumentNullException(nameof(httpHelper));
    }

    [HttpPost]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> PostAsync(CancellationToken cancellationToken) {
      if (!mHttpHelper.IsMultipartContentType(Request.ContentType)) {
        string error = $"Expected a multipart request, but got {Request.ContentType}";
        mLogger.LogWarning(error);
        return BadRequest(error);
      }
      var boundary = mHttpHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 128);
      var reader = new MultipartReader(boundary, Request.Body);
      MultipartSection section = null;
      try {
        section = await reader.ReadNextSectionAsync(cancellationToken);
      }
      catch (IOException ex) {
        mLogger.LogError(ex, "Caught exception reading multipart content");
        return BadRequest("An error occurred while reading multipart content");
      }
      if (section is null || !ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition)) {
        var error = "multipart section was null or unable to parse content disposition";
        mLogger.LogError(error);
        return BadRequest(error);
      }
      if (!mHttpHelper.HasFileContentDisposition(contentDisposition)) {
        var error = "multipart section does not have file content disposition";
        mLogger.LogError(error);
        return BadRequest(error);
      }
      var domainRequest = new ImportCsvRequest {
        ContentType = section.Headers["Content-Type"],
        FileName = contentDisposition.FileName.Value
      };
      domainRequest.InputStream = section.Body;
      var result = await mImportService.ImportCsvAsync(domainRequest, cancellationToken);
      return StatusCode((int)result.Status, result);
    }
  }
}
