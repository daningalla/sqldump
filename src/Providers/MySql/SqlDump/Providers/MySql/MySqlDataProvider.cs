using System.Text;
using System.Text.Json.Nodes;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlDump.Core.SqlDump.Collections;
using SqlDump.Data;

namespace SqlDump.Providers.MySql;

public sealed class MySqlDataProvider : IDataProvider
{
    private readonly ILogger<MySqlDataProvider> _logger;
    private readonly MySqlConnectionFactory _connectionFactory;
    private readonly ExportOptions _options;
    private ReadOnlyIndex<string, ColumnMetadata>? _cachedMetadata;

    public MySqlDataProvider(
        ILogger<MySqlDataProvider> logger,
        IOptions<ExportOptions> options,
        MySqlConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task ValidateConnectionAsync(CancellationToken cancellationToken)
    {
        await using var _ = await _connectionFactory.CreateAndOpenConnectionAsync(cancellationToken);
        _logger.LogInformation("Validated connection to MySql database server.");
    }

    /// <inheritdoc />
    public async Task<ReadOnlyIndex<string, ColumnMetadata>> GetColumnMetadataAsync(
        CancellationToken cancellationToken)
    {
        if (_cachedMetadata != null)
            return _cachedMetadata;
        
        var sql =
            """
            select
                `COLUMN_NAME` as `Name`,
                `ORDINAL_POSITION` as `OrdinalPosition`,
                `DATA_TYPE` as `DataType`,
                `COLUMN_TYPE` as `ColumnSpec`,
                `IS_NULLABLE` as `Nullable`,
                `CHARACTER_MAXIMUM_LENGTH` as `MaxLength`,
                `NUMERIC_PRECISION` as `Precision`,
                `NUMERIC_SCALE` as `Scale`
            from `INFORMATION_SCHEMA`.`COLUMNS`
            WHERE TABLE_SCHEMA=@schema AND TABLE_NAME=@table
            ORDER BY `ORDINAL_POSITION`
            """;

        await using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(cancellationToken);
        var results = await connection.QueryAsync<MySqlColumnMetadata>(sql,
            new
            {
                schema = _options.SchemaName,
                table = _options.TableName
            });

        var mappedEntries = results
            .Select(ColumnMetadataMapper.Map)
            .Where(kv => !_options.ExcludedColumns.Contains(kv.Key))
            .ToArray();
        
        _logger.LogInformation("Mapped {count} column definitions for source table,", mappedEntries.Length);

        var excluded = new HashSet<string>(_options.ExcludedColumns);
        excluded.ExceptWith(mappedEntries.Select(kv => kv.Key));
        if (excluded.Count > 0)
        {
            throw new ApplicationException($"Invalid excluded column(s): {string.Join(',', excluded)}");
        }

        LogExcludedColumns(_options.ExcludedColumns);
        LogSchemaColumns(mappedEntries);
        
        return _cachedMetadata = new ReadOnlyIndex<string, ColumnMetadata>(mappedEntries);
    }

    public async Task<IQuery> CreateQueryAsync(IEnumerable<KeyValuePair<string, string>> parameters, 
        CancellationToken cancellationToken)
    {
        var columnSchema = await GetColumnMetadataAsync(cancellationToken);
        var invalidSortColumn = _options
            .SortColumns
            .FirstOrDefault(id => !columnSchema.ContainsKey(id));

        if (invalidSortColumn != null)
        {
            throw new ApplicationException($"Invalid sort column '{invalidSortColumn}'.");
        }

        var typedParameters = parameters
            .Select(parameter =>
            {
                if (!columnSchema.TryGetValue(parameter.Key, out var columnMetadata))
                {
                    throw new ApplicationException($"Invalid query parameter '{parameter.Key}'.");
                }

                return new KeyValuePair<string, object>(
                    parameter.Key,
                    columnMetadata.TypeManager.ConvertParameter(parameter.Value));
            })
            .ToArray();
        
        
    }

    private void LogSchemaColumns(KeyValuePair<string, ColumnMetadata>[] mappedEntries)
    {
        foreach (var (key, value) in mappedEntries)
        {
            _logger.LogDebug("Column metadata '{key}': {type}, provider null: {nullable}",
                value.Name,
                value.ClrType,
                value.IsNullable);
        }
    }

    private void LogExcludedColumns(string[] excludedColumns)
    {
        foreach (var name in excludedColumns)
        {
            _logger.LogDebug("Excluded column '{name}' from export definition.", name);
        }
    }
}