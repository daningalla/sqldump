using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace SqlDump.Providers.MySql;

public sealed class MySqlConnectionFactory
{
    private readonly ILogger<MySqlConnectionFactory> _logger;
    private readonly string _connectionString;
    private readonly MySqlConnectionStringBuilder _connectionBuilder;
    private bool _firstConnect = true;

    public MySqlConnectionFactory(
        ILogger<MySqlConnectionFactory> logger,
        IOptions<ExportOptions> options)
    {
        _logger = logger;
        _connectionString = BuildConnectionString(options.Value.ConnectionProperties);
        _connectionBuilder = new MySqlConnectionStringBuilder(_connectionString);
    }

    /// <summary>
    /// Creates and opens the connection.
    /// </summary>
    /// <param name="cancellationToken">Token observed for cancellation requests.</param>
    /// <returns><see cref="MySqlConnection"/></returns>
    public async Task<MySqlConnection> CreateAndOpenConnectionAsync(CancellationToken cancellationToken)
    {
        if (_firstConnect)
        {
            _firstConnect = false;
            _logger.LogInformation("Creating connection factory, server={server}, database={database}.",
                _connectionBuilder.Server,
                _connectionBuilder.Database);
        }
        
        var connection = new MySqlConnection(_connectionString);
        
        _logger.LogDebug("Opening connection (async).");
        await connection.OpenAsync(cancellationToken);
        
        _logger.LogDebug("Connected to {server}/{database}",
            _connectionBuilder.Server,
            _connectionBuilder.Database);

        return connection;
    }

    private static string BuildConnectionString(IReadOnlyCollection<KeyValuePair<string, string>> properties)
    {
        return string.Join(
            ';',
            properties.Select(kv => $"{kv.Key}={kv.Value}"));
    }
}