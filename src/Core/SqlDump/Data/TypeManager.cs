using SqlDump.Reflection;

namespace SqlDump.Data;

public static class TypeManager
{
    /// <summary>
    /// Creates a type manager.
    /// </summary>
    /// <param name="type">Type to manage.</param>
    /// <returns><see cref="ITypeManager"/></returns>
    public static ITypeManager Create(Type type)
    {
        var properties = new TypeProperties(type);

        return properties switch
        {
            { IsString: true } => new TypeManager<string>(s => s),
            { IsNullableType: true, UnderlyingTypeProperties.IsParsable: true } => 
                Create(typeof(NullParsableTypeManager<>), properties.UnderlyingType!),
            { IsParsable: true } => Create(typeof(ParsableTypeManager<>), type),
            _ => throw new InvalidOperationException($"Could not create ITypeManager for {type}")
        };
    }

    private static ITypeManager Create(Type genericType, Type valueType)
    {
        return (ITypeManager)Activator.CreateInstance(genericType.MakeGenericType(valueType))!;
    }
}

file class TypeManager<T> : ITypeManager
{
    private readonly Func<string, object> _converter;
    
    internal TypeManager(Func<string, object> converter) => _converter = converter;

    /// <inheritdoc />
    public Type Target => typeof(T);

    /// <inheritdoc />
    public object ConvertString(string str) => _converter(str);
}

file sealed class ParsableTypeManager<T> : TypeManager<T> where T : IParsable<T>
{
    public ParsableTypeManager() : base(str => T.Parse(str, null))
    {
    }
}

file sealed class NullParsableTypeManager<T> : TypeManager<T?> where T : struct, IParsable<T>
{
    public NullParsableTypeManager() : base(str => T.Parse(str, null))
    {
    }
}