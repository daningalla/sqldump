using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlDump.Data;
using SqlDump.Engine;
using SqlDump.Providers.MySql;
using Vertical.SpectreLogger;

namespace SqlDump.Cli;

internal static class ServiceConfiguration
{
    internal static IServiceProvider GetServiceProvider(CliOptions options)
    {
        var services = new ServiceCollection()
            .AddLogging(logging =>
            {
                logging.SetMinimumLevel(options.Verbosity);
                logging.AddSpectreConsole(configure => configure.SetMinimumLevel(options.Verbosity));
            })
            .AddExportEngine()
            .AddSingleton<IOptions<ExportOptions>>(new OptionsWrapper<ExportOptions>(options))
            .ConfigureProviders(builder => builder.AddMySql())
            .BuildServiceProvider();

        return services;
    }
}