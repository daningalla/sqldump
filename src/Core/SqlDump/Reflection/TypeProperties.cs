namespace SqlDump.Reflection;

public sealed class TypeProperties(Type type)
{
    public Type Type => type;

    /// <summary>
    /// Gets whether a type implements <see cref="IParsable{TSelf}"/>
    /// </summary>
    public bool IsParsable { get; } = Set(() =>
    {
        var parsableType = typeof(IParsable<>).MakeGenericType(type);
        return type.GetInterfaces().Any(implements => implements == parsableType);
    });

    /// <summary>
    /// Gets the underlying type of <see cref="Nullable{T}"/>
    /// </summary>
    public Type? NullableValueType { get; } = Set(() =>
        type is { IsGenericType: true, GenericTypeArguments.Length: 1 } &&
        typeof(Nullable<>) == type.GetGenericTypeDefinition()
            ? type.GenericTypeArguments[0]
            : null);

    /// <summary>
    /// Gets whether the type is <see cref="Nullable{T}"/>
    /// </summary>
    public bool IsNullableValueType => NullableValueType != null;

    private static T Set<T>(Func<T> value) => value();
}