using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nettolicious.Tickets.Contracts.Domain.Requests {
  public class ImportCsvRequest {
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream InputStream { get; set; }
  }
}
