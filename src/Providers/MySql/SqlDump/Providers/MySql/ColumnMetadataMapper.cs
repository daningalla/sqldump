using SqlDump.Data;

namespace SqlDump.Providers.MySql;

internal static class ColumnMetadataMapper
{
    internal static KeyValuePair<string, ColumnMetadata> Map(MySqlColumnMetadata metadata)
    {
        return new KeyValuePair<string, ColumnMetadata>(
            metadata.Name,
            CreateMySqlMetadata(metadata));
    }

    private static ColumnMetadata CreateMySqlMetadata(MySqlColumnMetadata metadata)
    {
        var clrType = GetClrType(metadata);
        var typeConverter = TypeConverter.Create(clrType);
        
        return new ColumnMetadata
        {
            Name = metadata.Name,
            Ordinal = metadata.OrdinalPosition,
            Precision = metadata.Precision,
            Scale = metadata.Scale,
            ClrType = clrType,
            IsNullable = metadata.Nullable == "YES",
            MaxLength = metadata.MaxLength,
            TypeConverter = typeConverter
        };
    }

    private static Type GetClrType(MySqlColumnMetadata metadata)
    {
        return typeof(string);
    }
}