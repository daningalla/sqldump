namespace SqlDump.Data;

public static class TypeConverter
{
    private static readonly Dictionary<Type, ITypeConverter> Cache = new();
    
    /// <summary>
    /// Creates a type converter.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see cref="ITypeConverter"/></returns>
    public static ITypeConverter Create(Type type)
    {
        return null!;
    }

    private abstract class Base<T>(Type type) : ITypeConverter
    {
        public Type Target => type;

        public virtual object ConvertString(string str) => Convert.ChangeType(str, Target);
    }

    private sealed class Parsable<T> : Base<T> where T : IParsable<T>
    {
        internal Parsable() : base(typeof(T))
        {
        }

        public override object ConvertString(string str) => T.Parse(str, null);
    }
}