using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.E2ETests.Controllers {
  public class PingControllerTests {

    [Fact]
    public async Task Get_WithValidRequest_ShouldReturnOkResponse() {
      // Arrange
      await TestUtils.DoHttpClientWorkAsync(async client => {
        var uri = new Uri("http://localhost/ping");
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        // Act
        var actual = await client.SendAsync(request);
        // Assert
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
      });
    }
  }
}
