using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Nettolicious.Tickets.API;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.E2ETests {
  public static class TestUtils {

    public static async Task DoHttpClientWorkAsync(Func<HttpClient, Task> asyncWork) {
      var webHostBuilder = new WebHostBuilder()
        .ConfigureServices(services => services.AddAutofac())
        .UseEnvironment("E2E")
        .ConfigureAppConfiguration((hostingCtx, config) => {
          var env = hostingCtx.HostingEnvironment;
          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
          config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
        })
        .UseStartup<Startup>();
      using var server = new TestServer(webHostBuilder);
      using var client = server.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      await asyncWork.Invoke(client);
    }

    public static async Task<T> DoHttpClientWorkAsync<T>(Func<HttpClient, Task<T>> asyncWork) {
      var webHostBuilder = new WebHostBuilder()
        .ConfigureServices(services => services.AddAutofac())
        .UseEnvironment("E2E")
        .ConfigureAppConfiguration((hostingCtx, config) => {
          var env = hostingCtx.HostingEnvironment;
          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
          config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
        })
        .UseStartup<Startup>();
      using var server = new TestServer(webHostBuilder);
      using var client = server.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      return await asyncWork.Invoke(client);
    }
  }
}
