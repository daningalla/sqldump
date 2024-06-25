using SqlDump.Core.SqlDump.Collections;

namespace SqlDump.Data;

public interface IDataProvider
{
    /// <summary>
    /// Validates the connection.
    /// </summary>
    /// <param name="cancellationToken">Token observed for cancellation.</param>
    /// <returns>Task</returns>
    Task ValidateConnectionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves column metadata.
    /// </summary>
    /// <param name="cancellationToken">Token observed for cancellation.</param>
    /// <returns>Column index</returns>
    Task<ReadOnlyIndex<string, ColumnMetadata>> GetColumnMetadataAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Creates a query given starting parameters.
    /// </summary>
    /// <param name="parameters">Parameters</param>
    /// <param name="cancellationToken">Token observed for cancellation.</param>
    /// <returns><see cref="IQuery"/></returns>
    Task<IQuery> CreateQueryAsync(IEnumerable<KeyValuePair<string, string>> parameters,
        CancellationToken cancellationToken);
}