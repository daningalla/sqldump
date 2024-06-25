namespace SqlDump.Reflection;

/// <summary>
/// Exposes additional properties of Type.
/// </summary>
public sealed class TypeProperties
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="type">Type</param>
    public TypeProperties(Type type)
    {
        Type = type;

        IsParsable = type
            .GetInterfaces()
            .Any(implements => implements is
            {
                IsInterface: true,
                IsGenericType: true,
                GenericTypeArguments.Length: 1
            } && implements.GetGenericTypeDefinition() == typeof(IParsable<>));
        
        UnderlyingType = type is { IsGenericType: true, GenericTypeArguments.Length: 1 } 
                            && type.GetGenericTypeDefinition() == typeof(Nullable<>)
            ? type.GenericTypeArguments[0]
            : null;

        UnderlyingTypeProperties = UnderlyingType != null
            ? new TypeProperties(UnderlyingType)
            : null;
    }
    
    /// <summary>
    /// Gets the type.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets whether the type is a string.
    /// </summary>
    public bool IsString => Type == typeof(string);

    /// <summary>
    /// Gets whether a type implements <see cref="IParsable{TSelf}"/>
    /// </summary>
    public bool IsParsable { get; }

    /// <summary>
    /// Gets the underlying type of <see cref="Nullable{T}"/>
    /// </summary>
    public Type? UnderlyingType { get; }
    
    /// <summary>
    /// Gets the underlying type properties.
    /// </summary>
    public TypeProperties? UnderlyingTypeProperties { get; }

    /// <summary>
    /// Gets whether the type is <see cref="Nullable{T}"/>
    /// </summary>
    public bool IsNullableType => UnderlyingType != null;
}