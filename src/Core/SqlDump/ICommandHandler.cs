namespace SqlDump.Core.SqlDump;

/// <summary>
/// Represents an object that implements a command given a model type.
/// </summary>
/// <typeparam name="T">Model</typeparam>
public interface ICommandHandler<in T>
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="options">Command options</param>
    /// <param name="cancellationToken">Token observed for cancellation requests</param>
    /// <returns><c>int</c></returns>
    Task<int> HandleAsync(T options, CancellationToken cancellationToken);
}