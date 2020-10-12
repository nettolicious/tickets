using Nettolicious.Tickets.Contracts.Domain.Model;
using Nettolicious.Tickets.Contracts.Domain.Responses;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.E2ETests.Controllers {
  public class ImportControllerTests {

    public const string CsvContent = @"OrderId,FirstName,LastName,TicketNumber,EventDate
1,Angele,Gowdy,UR6XWTAYOC,11/20/2018 9:05
1,Debora,Bangs,J6THN9SP11,11/20/2018 9:40
1,Golda,Drakeford,SN5RN5DWYM,11/20/2018 10:05
2,Exie,Cournoyer,XWVDXF1648,12/4/2018 10:45
3,Loreta,Arcuri,DYN0SIH81X,12/21/2018 11:05
3,Dominic,Minich,RQM23P4B7F,12/21/2018 11:50
3,Jerold,Lumpkins,IQGGURFB7K,12/21/2018 11:55
3,Esther,Wehling,YLTGIIY3FA,12/21/2018 12:05
3,Guy,Fukushima,HDK6UI6RSE,12/21/2018 12:25
3,Rebeca,Waddell,CYUX67XCJ7,12/21/2018 12:35
3,Sidney,Shover,SK1WRNWY4Q,12/21/2018 12:50
4,Sherise,Pizana,FA9741Z4XI,12/26/2018 13:35
4,Chia,Clauss,QMWOIQ1G2C,12/26/2018 14:50
4,Annabell,Esser,TVOH49NQLP,12/26/2018 15:00
4,Soo,Czerwinski,EDMRGZ5Y1M,12/26/2018 15:10
5,Regan,Garrow,HVY2DPYCOV,12/4/2018 15:40
5,Arnette,Corprew,SOH3C8PIKJ,12/4/2018 15:55
6,Lee,Hatley,RGZ79CGKCY,11/20/2018 16:05
7,Rosio,Calvi,IUUSFFJV6D,11/20/2018 16:30
7,Kristofer,Redding,3O5SZI5RFG,11/20/2018 16:50
";

    [Fact]
    public async Task PostAsync_WithNoFile_ShouldReturnBadRequest() {
      // Arrange
      await TestUtils.DoHttpClientWorkAsync(async client => {
        var uri = new Uri("http://localhost/import");
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        // Act
        var actual = await client.SendAsync(request);
        // Assert
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
      });
    }

    [Fact]
    public async Task PostAsync_WithValidRequest_ShouldReturnOkResponse() {
      // Arrange
      await TestUtils.DoHttpClientWorkAsync(async client => {
        var uri = new Uri("http://localhost/import");
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        var uploadRequestContent = new MultipartFormDataContent();
        HttpContent fileStreamContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(CsvContent)));
        fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
          Name = "Data File",
          FileName = "data.csv"
        };
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        uploadRequestContent.Add(fileStreamContent);
        request.Content = uploadRequestContent;
        // Act
        var actual = await client.SendAsync(request);
        // Assert
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        var responseContent = await actual.Content.ReadAsAsync<DomainResponse<ImportCsvResponse>>();
        Assert.NotNull(responseContent);
        Assert.Equal(20, responseContent.Data.RowsImported);
      });
    }
  }
}
