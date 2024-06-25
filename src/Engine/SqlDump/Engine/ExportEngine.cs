using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SqlDump.Core.SqlDump;
using SqlDump.Data;

namespace SqlDump.Engine;

internal sealed class ExportEngine : ICommandHandler<ExportOptions>
{
    private readonly KeyedServiceFactory<string, IDataProvider> _dataProviderFactory;
    private readonly ILogger<ExportEngine> _logger;

    public ExportEngine(ILogger<ExportEngine> logger,
        KeyedServiceFactory<string, IDataProvider> dataProviderFactory)
    {
        _logger = logger;
        _dataProviderFactory = dataProviderFactory;
    }
    
    public async Task<int> HandleAsync(
        ExportOptions options, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("sqldump, version {version}", EngineVersion.Value);
        
        var stopWatch = Stopwatch.StartNew();

        try
        {
            await ExecuteWrappedAsync(options, cancellationToken);
        }
        catch (ApplicationException exception)
        {
            _logger.LogError("{message}", exception.Message);
            return -1;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled error occurred");
            return -2;
        }

        _logger.LogInformation("Task complete, run time={time}", stopWatch.ToString());
        
        await Task.CompletedTask;
        return 0;
    }

    private async Task ExecuteWrappedAsync(
        ExportOptions options,
        CancellationToken cancellationToken)
    {
        var dataProvider = GetDataProvider(options.Provider);

        _logger.LogInformation("Resolved data provider '{provider}'={providerType}",
            options.Provider,
            dataProvider.GetType());

        await dataProvider.ValidateConnectionAsync(cancellationToken);
        var columnMetadata = await dataProvider.GetColumnMetadataAsync(cancellationToken);
    }

    private IDataProvider GetDataProvider(string provider)
    {
        return _dataProviderFactory.Resolve(provider)
               ?? throw  new ApplicationException($"Invalid data provider '{provider}'.");
    }
}