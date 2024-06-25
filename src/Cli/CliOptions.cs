using Microsoft.Extensions.Logging;

namespace SqlDump.Cli;

/// <summary>
/// Options specific to the CLI application.
/// </summary>
public sealed class CliOptions : ExportOptions
{
    /// <summary>
    /// Gets the logging verbosity.
    /// </summary>
    public LogLevel Verbosity { get; init; }
}