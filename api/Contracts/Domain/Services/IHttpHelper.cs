using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Nettolicious.Tickets.Contracts.Domain.Services {
  public interface IHttpHelper {
    bool IsMultipartContentType(string contentType);
    string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit);
    bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition);
    bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition);
    Encoding GetEncoding(MultipartSection section);
  }
}
