using System.Text.RegularExpressions;
using Vertical.Cli.Conversion;

namespace SqlDump.Cli;

/// <summary>
/// Converts arguments to KeyValuePair
/// </summary>
public partial class KeyValuePairConverter : ValueConverter<KeyValuePair<string, string>>
{
    /// <inheritdoc />
    public override KeyValuePair<string, string> Convert(string s)
    {
        var match = MyRegex().Match(s);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid key/value pair syntax.");
        }

        return new KeyValuePair<string, string>(
            match.Groups[1].Value,
            match.Groups[2].Value);
    }

    [GeneratedRegex(@"^([^=]+)=(.+)$")]
    private static partial Regex MyRegex();
}