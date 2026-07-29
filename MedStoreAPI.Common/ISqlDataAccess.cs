using Dapper;

namespace MedStoreAPI.Common
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Generic helper that wraps Dapper calls to stored procedures.
    /// Every module's repository (Medicine, Batch, Invoice, etc.) will depend on
    /// this interface instead of talking to Dapper/ADO.NET directly. This keeps
    /// all repositories thin and makes it easy to swap the data layer later
    /// (e.g. add caching, logging, retry policies) in one place only.
    /// </summary>
    public interface ISqlDataAccess
    {
        /// <summary>Returns a list of rows mapped to T from the given stored procedure.</summary>
        Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null);

        /// <summary>Returns a single row (or default) mapped to T from the given stored procedure.</summary>
        Task<T?> QuerySingleAsync<T>(string storedProcedure, object? parameters = null);

        /// <summary>Executes a stored procedure that does not return rows (INSERT/UPDATE/DELETE without SELECT).</summary>
        Task<int> ExecuteAsync(string storedProcedure, object? parameters = null);

        /// <summary>
        /// Executes a stored procedure that returns multiple result sets
        /// (e.g. SP_InvoiceGetByID returns invoice header + invoice items).
        /// Caller reads each result set in order using ReadAsync/ReadSingleAsync.
        /// </summary>
        Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, object? parameters = null);
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlDataAccess(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(
                storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<T?> QuerySingleAsync<T>(string storedProcedure, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(
                storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteAsync(string storedProcedure, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, object? parameters = null)
        {
            var connection = _connectionFactory.CreateConnection();
            return await connection.QueryMultipleAsync(
                storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
