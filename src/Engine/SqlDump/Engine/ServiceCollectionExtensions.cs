using Microsoft.Extensions.DependencyInjection;
using SqlDump.Core.SqlDump;
using SqlDump.Data;

namespace SqlDump.Engine;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExportEngine(this IServiceCollection services)
    {
        services
            .AddSingleton<ICommandHandler<ExportOptions>, ExportEngine>()
            .AddSingleton<KeyedServiceFactory<string, IDataProvider>>();
        
        return services;
    }
}