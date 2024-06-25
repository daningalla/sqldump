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
        var typeConverter = TypeManager.Create(clrType);
        
        return new ColumnMetadata
        {
            Name = metadata.Name,
            Alias = GetAlias(metadata),
            Ordinal = metadata.OrdinalPosition,
            Precision = metadata.Precision,
            Scale = metadata.Scale,
            ClrType = clrType,
            IsNullable = metadata.Nullable == "YES",
            MaxLength = metadata.MaxLength,
            TypeManager = typeConverter
        };
    }

    private static string GetAlias(MySqlColumnMetadata metadata)
    {
        return metadata.DataType switch
        {
            "point" => $"ST_AsText(`{metadata.Name}`) as `{metadata.Name}`",
            "polygon" => $"ST_AsText(`{metadata.Name}`) as `{metadata.Name}`",
            _ => $"`{metadata.Name}`"
        };
    }

    private static Type GetClrType(MySqlColumnMetadata metadata)
    {
        return metadata switch
        {
            { Nullable: "YES", ColumnSpec: "char(36)" } => typeof(Guid?),
            { Nullable: "YES", DataType: "integer" } => typeof(int?),
            { Nullable: "YES", DataType: "int" } => typeof(int?),
            { Nullable: "YES", DataType: "smallint" } => typeof(short?),
            { Nullable: "YES", DataType: "mediumint" } => typeof(int?),
            { Nullable: "YES", DataType: "bigint" } => typeof(long?),
            { Nullable: "YES", ColumnSpec: "tinyint(1)" } => typeof(bool?),
            { Nullable: "YES", DataType: "tinyint" } => typeof(byte?),
            { Nullable: "YES", DataType: "decimal" } => typeof(decimal?),
            { Nullable: "YES", DataType: "numeric" } => typeof(decimal?),
            { Nullable: "YES", DataType: "float" } => typeof(float?),
            { Nullable: "YES", DataType: "double" } => typeof(double?),
            { Nullable: "YES", DataType: "datetime" } => typeof(DateTime?),
            
            { ColumnSpec: "char(36)" } => typeof(Guid),
            { DataType: "integer" } => typeof(int),
            { DataType: "int" } => typeof(int),
            { DataType: "smallint" } => typeof(short),
            { DataType: "mediumint" } => typeof(int),
            { DataType: "bigint" } => typeof(long),
            { ColumnSpec: "tinyint(1)" } => typeof(bool),
            { DataType: "tinyint" } => typeof(byte),
            { DataType: "decimal" } => typeof(decimal),
            { DataType: "numeric" } => typeof(decimal),
            { DataType: "float" } => typeof(float),
            { DataType: "double" } => typeof(double),
            { DataType: "datetime" } => typeof(DateTime),
            
            { DataType: "char" } => typeof(string),
            { DataType: "varchar" } => typeof(string),
            { DataType: "json" } => typeof(string),
            { DataType: "text" } => typeof(string),
            { DataType: "tinytext" } => typeof(string),
            { DataType: "mediumtext" } => typeof(string),
            { DataType: "enum" } => typeof(string),
            { DataType: "set" } => typeof(string),
            { DataType: "point" } => typeof(string),
            { DataType: "polygon" } => typeof(string),
            
            _ => throw new InvalidOperationException($"Could not resolve CLR type for {metadata}.")
        };
    }
}