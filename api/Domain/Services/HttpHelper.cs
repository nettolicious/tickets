using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Nettolicious.Tickets.Contracts.Domain.Services;
using System;
using System.Text;

namespace Nettolicious.Tickets.Domain.Services {
  public class HttpHelper : IHttpHelper {
    public bool IsMultipartContentType(string contentType) {
      return !string.IsNullOrEmpty(contentType)
             && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit) {
      // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
      // The spec says 70 characters is a reasonable limit.
      var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;
      if (string.IsNullOrWhiteSpace(boundary)) {
        throw new System.IO.InvalidDataException("Missing content-type boundary.");
      }
      if (boundary.Length > lengthLimit) {
        throw new System.IO.InvalidDataException(
            $"Multipart boundary length limit {lengthLimit} exceeded.");
      }
      return boundary;
    }

    public bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition) {
      // Content-Disposition: form-data; name="key";
      return contentDisposition != null
              && contentDisposition.DispositionType.Equals("form-data")
              && string.IsNullOrEmpty(contentDisposition.FileName.Value)
              && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
    }

    public bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition) {
      // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
      return contentDisposition != null
              && contentDisposition.DispositionType.Equals("form-data")
              && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                  || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
    }

    public Encoding GetEncoding(MultipartSection section) {
      MediaTypeHeaderValue mediaType;
      var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
      // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
      // most cases.
      if (!hasMediaTypeHeader || System.Text.Encoding.UTF7.Equals(mediaType.Encoding)) {
        return System.Text.Encoding.UTF8;
      }
      return mediaType.Encoding;
    }
  }
}
