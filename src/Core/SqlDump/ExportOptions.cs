namespace SqlDump;

/// <summary>
/// Defines export options.
/// </summary>
public class ExportOptions
{
    /// <summary>
    /// Gets the data provider to use.
    /// </summary>
    public string Provider { get; init; } = default!;

    /// <summary>
    /// Gets connection properties used by the provider.
    /// </summary>
    public KeyValuePair<string, string>[] ConnectionProperties { get; init; } = [];

    /// <summary>
    /// Gets the schema name.
    /// </summary>
    public string? SchemaName { get; init; } = default!;

    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string TableName { get; init; } = default!;

    /// <summary>
    /// Gets the excluded columns.
    /// </summary>
    public string[] ExcludedColumns { get; init; } = [];

    /// <summary>
    /// Gets one or more columns used in watermark sorting.
    /// </summary>
    public string[] SortColumns { get; init; } = [];
}