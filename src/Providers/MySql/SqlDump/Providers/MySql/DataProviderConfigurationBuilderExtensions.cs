using Microsoft.Extensions.DependencyInjection;
using SqlDump.Data;

namespace SqlDump.Providers.MySql;

public static class DataProviderConfigurationBuilderExtensions
{
    public static DataProviderConfigurationBuilder AddMySql(this DataProviderConfigurationBuilder builder)
    {
        builder
            .Services
            .AddKeyedSingleton<IDataProvider, MySqlDataProvider>("mysql")
            .AddSingleton<MySqlConnectionFactory>();
        
        return builder;
    }
}