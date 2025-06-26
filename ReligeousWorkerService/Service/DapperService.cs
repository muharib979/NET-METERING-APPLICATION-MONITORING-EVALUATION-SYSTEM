

using Dapper;
using ReligeousWorkerService.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ReligeousWorkerService.Service
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _config;

        public DapperService(IConfiguration config)
        {
            _config = config;
        }

        public OracleConnection Connection => new OracleConnection(_config.GetConnectionString("ConnectionString"));

        public async Task<T> QueryFirstOrDefaultAsyncWithDapper<T>(string sql, object parameters, string connectionString = null)
        {
            using IDbConnection conn = (connectionString == null) ? Connection : new OracleConnection(connectionString);
            return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);

        }

        public async Task<List<T>> QueryAsyncWithDapper<T>(string sql, object parameters, string connectionString = null)
        {

            using IDbConnection conn = (connectionString == null) ? Connection : new OracleConnection(connectionString);
            var result = await conn.QueryAsync<T>(sql, parameters);
            return result.ToList();
        }

        public async Task<int> ExecuteAsyncWithDapperAndTransaction(string sql, object parameters, IDbConnection conn, IDbTransaction transaction = null)
        {
            try
            {
                return await conn.ExecuteAsync(sql, parameters, transaction);
            }
            catch (Exception)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw;
            }
        }

        public async Task<int> ExecuteAsyncWithDapper(string sql, object parameters, string connectionString = null)
        {
            using IDbConnection conn = (connectionString == null) ? Connection : new OracleConnection(connectionString);
            return await conn.ExecuteAsync(sql, parameters);
        }

        public async Task<int> ExecuteScalarAsyncWithDapper(string sql, object parameters, IDbConnection conn, IDbTransaction transaction = null)
        {
            try
            {
                return await conn.ExecuteScalarAsync<int>(sql, parameters, transaction);
            }
            catch (Exception)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw;
            }
        }
    }
}
