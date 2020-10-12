using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nettolicious.Tickets.Configuration;
using Newtonsoft.Json;

namespace Nettolicious.Tickets.API {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services
        .AddControllers()
        .AddControllersAsServices()
        .AddNewtonsoftJson(opt => {
          opt.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
          // These should be the defaults, but we can be explicit:
          opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
          opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
          opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
          opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    }

    public void ConfigureContainer(ContainerBuilder builder) {
      builder.RegisterModule(new ConfigurationModule());
    }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
