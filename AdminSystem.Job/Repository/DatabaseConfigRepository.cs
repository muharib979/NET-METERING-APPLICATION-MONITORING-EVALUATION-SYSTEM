

using AdminSystem.Job.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AdminSystem.Job.Repository
{
    public interface IDatabaseConfigRepository
    {
        Task<object> GetAllDatabaseConfigAsync();
        Task<DatabaseConfig> GetDatabaseConfigByIDAsync(string dbCode);
        Task<List<DatabaseConfig>> GetAllActiveDatabaseConfigAsync();
    }
    public class DatabaseConfigRepository : IDatabaseConfigRepository
    {
        private readonly IConfiguration _config;
        public DatabaseConfigRepository(IConfiguration config)
        {
            _config = config;
        }

        public OracleConnection Connection
        {
            get
            {
                return new OracleConnection(_config.GetConnectionString("ConnectionString"));
            }
        }
        #region Method
        public async Task<object> GetAllDatabaseConfigAsync()
        {
            using IDbConnection conn = Connection;
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE ORDER BY DB_NAME";

                var result = await conn.QueryAsync<object>(sQuery);
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<DatabaseConfig> GetDatabaseConfigByIDAsync(string dbcode)
        {
            using IDbConnection conn = Connection;
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE CODE= :dbCode";

                var result = await conn.QueryFirstOrDefaultAsync<DatabaseConfig>(sQuery, new { dbCode = dbcode });
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<List<DatabaseConfig>> GetAllActiveDatabaseConfigAsync()
        {
            using IDbConnection conn = Connection;
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE D WHERE D.IS_ACTIVE = :IsActive AND IS_JOB = 1 ORDER BY D.ORDER_NO";

                var result = await conn.QueryAsync<DatabaseConfig>(sQuery, new { IsActive = 1 });
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
