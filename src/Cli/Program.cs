using SqlDump.Cli;
using Vertical.Cli;

var rootCommand = CliConfiguration.Create();

try
{
    await rootCommand.InvokeAsync(args);
}
catch (ApplicationException exception)
{
    Console.WriteLine(exception.Message);
}
catch (CommandLineException exception)
{
    Console.WriteLine(exception.Message);
}
catch (Exception exception)
{
    Console.WriteLine(exception.ToString());
}