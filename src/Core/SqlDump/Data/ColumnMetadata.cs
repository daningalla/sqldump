namespace SqlDump.Data;

/// <summary>
/// Describes metadata about a column in a schema.
/// </summary>
public class ColumnMetadata
{
    /// <summary>
    /// Gets the ordinal position.
    /// </summary>
    public int Ordinal { get; init; }

    /// <summary>
    /// Gets the column name.
    /// </summary>
    public string Name { get; init; } = default!;
    
    /// <summary>
    /// Gets the alias of the column.
    /// </summary>
    public string? Alias { get; init; }

    /// <summary>
    /// Gets the mapped CLR type.
    /// </summary>
    public Type ClrType { get; init; } = default!;
    
    /// <summary>
    /// Gets whether the column is nullable.
    /// </summary>
    public bool IsNullable { get; init; }
    
    /// <summary>
    /// Gets the maximum character length.
    /// </summary>
    public int? MaxLength { get; init; }
    
    /// <summary>
    /// Gets the precision of a numeric type.
    /// </summary>
    public int? Precision { get; init; }
    
    /// <summary>
    /// Gets the scale of a numeric type.
    /// </summary>
    public int? Scale { get; init; }

    /// <summary>
    /// Gets the type converter.
    /// </summary>
    public ITypeManager TypeManager { get; init; } = default!;

    /// <inheritdoc />
    public override string ToString() => $"{Name} ({ClrType})";
}