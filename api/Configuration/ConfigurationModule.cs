using Autofac;
using Nettolicious.Tickets.Contracts.Domain.Services;
using Nettolicious.Tickets.Domain.Services;

namespace Nettolicious.Tickets.Configuration {
  public class ConfigurationModule : Module {

    protected override void Load(ContainerBuilder builder) {
      builder.RegisterType<ImportService>().As<IImportService>();
    }
  }
}
