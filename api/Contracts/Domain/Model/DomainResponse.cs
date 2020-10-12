using System.Net;

namespace Nettolicious.Tickets.Contracts.Domain.Model {
  public class DomainResponse {

    public HttpStatusCode Status { get; set; }
    public object Error { get; set; }

    public DomainResponse() { }

    public DomainResponse(HttpStatusCode status, object error = null) {
      Status = status;
      Error = error;
    }
  }

  public class DomainResponse<T> : DomainResponse {
    public T Data { get; set; }

    public DomainResponse() : base() { }

    public DomainResponse(HttpStatusCode status, object error = null, T data = default(T)) {
      Status = status;
      Error = error;
      Data = data;
    }
  }
}
