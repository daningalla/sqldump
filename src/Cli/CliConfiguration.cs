using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDump.Core.SqlDump;
using SqlDump.Engine;
using Vertical.Cli;

namespace SqlDump.Cli;

public static class CliConfiguration
{
    public static RootCommand<CliOptions> Create()
    {
        var command = new RootCommand<CliOptions>(
            "sqldump",
            "Dumps data from a relational database to files.");

        command
            .AddOption(x => x.Provider,
                names: ["-p", "--provider"],
                arity: Arity.One,
                description: "Name of the data provider to use (e.g. mysql)")
            .AddOption(x => x.ConnectionProperties,
                names: ["--connection-prop"],
                arity: Arity.ZeroOrMany,
                description: "A property used when composing the data provider connection.",
                operandSyntax: "KEY=VALUE")
            .AddOption(x => x.SchemaName,
                names: ["--schema"],
                arity: Arity.ZeroOrOne,
                description: "The schema owner of the table (will make query statements fully qualified in most providers).",
                operandSyntax: "IDENTIFIER")
            .AddOption(x => x.TableName,
                names: ["--table"],
                arity: Arity.One,
                description: "The name of the table being exported.",
                operandSyntax: "IDENTIFIER")
            .AddOption(x => x.ExcludedColumns,
                names: ["-x", "--exclude"],
                arity: Arity.ZeroOrMany,
                description: "Identifier of a column excluded from the export.",
                operandSyntax: "IDENTIFIER")
            .AddOption(x => x.Verbosity,
                names: ["-v", "--verbosity"],
                defaultProvider: () => LogLevel.Information,
                description: "The verbosity of output logging (Debug, Information, Warning, Error)",
                operandSyntax: "LEVEL");

        command.HandleAsync(async (options, cancellationToken) =>
        {
            var services = ServiceConfiguration.GetServiceProvider(options);

            try
            {
                var handler = services.GetRequiredService<ICommandHandler<ExportOptions>>();
                return await handler.HandleAsync(options, cancellationToken);
            }
            finally
            {
                if (services is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync();
            }
        });

        command.AddHelpSwitch();
        command.AddAction(["--version"], 
            () => Console.WriteLine($"{EngineVersion.Value}"),
            description: "Displays the version of the application.");
        
        command.ConfigureOptions(options =>
        {
            options.EnableResponseFiles = true;
            options.ValueConverters.Add(new KeyValuePairConverter());
        });
        
        return command;
    }
}