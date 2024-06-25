using Microsoft.Extensions.DependencyInjection;

namespace SqlDump.Data;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures data providers.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="builder">Builder object used to configure providers.</param>
    /// <returns>Reference to services.</returns>
    public static IServiceCollection ConfigureProviders(
        this IServiceCollection services,
        Action<DataProviderConfigurationBuilder> builder)
    {
        builder(new DataProviderConfigurationBuilder(services));
        return services;
    }
}