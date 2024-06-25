namespace SqlDump.Data;

/// <summary>
/// Represents a type converter.
/// </summary>
public interface ITypeManager
{
    /// <summary>
    /// Gets the target type.
    /// </summary>
    Type Target { get; }

    /// <summary>
    /// Converts the given string to an instance of the type.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns><see cref="object"/></returns>
    object ConvertString(string str);
}