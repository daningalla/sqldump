using Microsoft.Extensions.DependencyInjection;

namespace SqlDump.Engine;

/// <summary>
/// Manages keyed services of a specific type.
/// </summary>
/// <param name="serviceProvider">Service provider</param>
public class KeyedServiceFactory<TKey, TService>(IServiceProvider serviceProvider) where TService : notnull
{
    /// <summary>
    /// Resolves the service instance.
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns><c>TService</c></returns>
    public TService? Resolve(TKey key) => serviceProvider.GetKeyedService<TService>(key);
}