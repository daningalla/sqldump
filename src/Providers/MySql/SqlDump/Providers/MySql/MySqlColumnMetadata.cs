namespace SqlDump.Providers.MySql;

/// <summary>
/// Structure used to receive column metadata from INFORMATION_SCHEMA.
/// </summary>
public sealed class MySqlColumnMetadata
{
    public string Name { get; init; } = default!;
    public int OrdinalPosition { get; init; }
    public string DataType { get; init; } = default!;
    public string ColumnSpec { get; init; } = default!;
    public string Nullable { get; init; } = default!;
    public int? MaxLength { get; init; }
    public int? Precision { get; init; }
    public int? Scale { get; init; }

    public override string ToString() => $"{ColumnSpec} `{Name}`";
}