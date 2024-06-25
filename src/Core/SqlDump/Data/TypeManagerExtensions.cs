namespace SqlDump.Data;

public static class TypeManagerExtensions
{
    public static object ConvertParameter(this ITypeManager typeManager, string value)
    {
        try
        {
            return typeManager.ConvertString(value);
        }
        catch(Exception exception)
        {
            throw new ApplicationException(
                $"Could not convert value '{value}' to {typeManager.Target}: {exception.Message}",
                exception);
        }
    }
}