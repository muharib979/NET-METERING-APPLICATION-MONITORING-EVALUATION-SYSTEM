
using Dapper;
using ReligeousWorkerService.DbContext;
using ReligeousWorkerService.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ReligeousWorkerService.Repository
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
            using var con = new OracleConnection(ReligeousWorkerService.DbContext.Connection.ConnectionString());
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE ORDER BY DB_NAME";

                var result = await con.QueryAsync<object>(sQuery);
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<DatabaseConfig> GetDatabaseConfigByIDAsync(string dbcode)
        {
            using var con = new OracleConnection(ReligeousWorkerService.DbContext.Connection.ConnectionString());
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE CODE= :dbCode";

                var result = await con.QueryFirstOrDefaultAsync<DatabaseConfig>(sQuery, new { dbCode = dbcode });
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<List<DatabaseConfig>> GetAllActiveDatabaseConfigAsync()
        {
            //using IDbConnection conn = Connection;
            using var con = new OracleConnection(ReligeousWorkerService.DbContext.Connection.ConnectionString());
            try
            {
                string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE D WHERE D.IS_ACTIVE = :IsActive AND IS_JOB = 1 ORDER BY D.ORDER_NO";

                var result = await con.QueryAsync<DatabaseConfig>(sQuery, new { IsActive = 1 });
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
