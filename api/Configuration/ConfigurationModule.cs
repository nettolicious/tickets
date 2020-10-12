using Autofac;
using Microsoft.EntityFrameworkCore;
using Nettolicious.Common.Providers;
using Nettolicious.Tickets.Contracts;
using Nettolicious.Tickets.Contracts.Domain.Services;
using Nettolicious.Tickets.Domain.Services;
using Nettolicious.Tickets.Persistence;
using System;
using System.Collections.Generic;

namespace Nettolicious.Tickets.Configuration {
  public class ConfigurationModule : Module {

    private readonly IDictionary<string, string> mSettings;

    public ConfigurationModule(IDictionary<string, string> settings) {
      mSettings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    protected override void Load(ContainerBuilder builder) {
      builder.Register(c => new SettingsProvider(mSettings)).As<ISettingsProvider>();
      builder.Register(c => NewTicketsDbContext(mSettings[Constants.Persistence.TICKETS_DB_CONNECTION]))
        .As<TicketsDbContext>()
        .InstancePerLifetimeScope();
      builder.RegisterType<ImportService>().As<IImportService>();
      builder.RegisterType<HttpHelper>().As<IHttpHelper>();
    }

    private TicketsDbContext NewTicketsDbContext(string connection) {
      var options = new DbContextOptionsBuilder<TicketsDbContext>();
      options.UseSqlServer(connection);
      return new TicketsDbContext(options.Options);
    }
  }
}
