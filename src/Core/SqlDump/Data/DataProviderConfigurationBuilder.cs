using Microsoft.Extensions.DependencyInjection;

namespace SqlDump.Data;

/// <summary>
/// Builder for data providers.
/// </summary>
public sealed class DataProviderConfigurationBuilder(IServiceCollection services)
{
    /// <summary>
    /// Gets the service collection.
    /// </summary>
    public IServiceCollection Services => services;
}