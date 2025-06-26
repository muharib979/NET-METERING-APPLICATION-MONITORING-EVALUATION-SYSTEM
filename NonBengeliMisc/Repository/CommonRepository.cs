
using Dapper;
using NonBengeliMisc.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace NonBengeliMisc.Repository
{
    public interface ICommonRepository
    {
        string CreateConnectionString(string host, string port, string serviceName, string userId, string password);
        Task<string> GetServiceNameByDBId(int dbId);
        Task<string> GetDBNameByDBId(int dbId);
        Task<string> GetConnectionStringByDBId(int dbId);
        Task<string> SetSysConnectionStringByDBId(int dbId);
    }
    public class CommonRepository : ICommonRepository
    {
        private readonly IConfiguration _config;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly IDapperService _dapperService;
        private IConfiguration _configuration;

        public CommonRepository(IConfiguration config, IDatabaseConfigRepository dbConfigRepo, IDapperService dapperService, IConfiguration configuration)
        {
            _config = config;
            _dbConfigRepo = dbConfigRepo;
            _dapperService = dapperService;
            _configuration = configuration;
        }

        public OracleConnection Connection
        {
            get
            {
                return new OracleConnection(_config.GetConnectionString("ConnectionString"));
            }
        }


        /// <summary>
        /// Creates the connection string for database.
        /// </summary>
        /// <param name="host">string host is host of database</param>
        /// <param name="port">string port is port of database</param>
        /// <param name="serviceName">string serviceName is service name of database</param>
        /// <param name="userId">string userId is user id of database</param>
        /// <param name="password">string password is password of database</param>
        /// <returns>String connection string  for database</returns>
        public string CreateConnectionString(string host, string port, string serviceName, string userId, string password)
        {
            // string timeout = _config.GetSection("ConnectionStrings").GetSection("Timeout").Value; // Getting timeout value from appsettings.json.
            var timeout = _configuration["ConnectionStrings:TimeOut"];
            string conn = "";
            try
            {
                conn = @"DATA SOURCE= (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + port + "))" +
                    "(CONNECT_DATA=(SERVER=dedicated)(SERVICE_NAME=" + serviceName + ")));PERSIST SECURITY INFO=True;PASSWORD=" + password + ";USER ID=" + userId + ";Connect Timeout=" + timeout + ";";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return conn;
        }




        /// <summary>
        /// Service Name
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public async Task<string> GetServiceNameByDBId(int dbId)
        {
            string serviceName = "";
            using var con = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            try
            {
                string iQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID=:DbId";

                var result = await con.QueryFirstAsync(iQuery, new
                {
                    DbId = dbId
                });
                if (result != null)
                    serviceName = result.SERVICE_NAME;
            }
            catch (Exception ex)
            {
                throw;
            }


            return serviceName;
        }



        /// <summary>
        /// Service Name
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public async Task<string> GetDBNameByDBId(int dbId)
        {
            string dbName = "";
            using var con = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            try
            {
                string iQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID=:DbId";

                var result = await con.QueryFirstAsync(iQuery, new
                {
                    DbId = dbId
                });
                if (result != null)
                    dbName = result.DB_NAME;
            }
            catch (Exception ex)
            {
                throw;
            }


            return dbName;
        }


        public async Task<string> SetSysConnectionStringByDBId(int dbId)
        {
            string _connstr = "";
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            try
            {
                string sQuery = @"SELECT BC_DB_LOCATION.LOCATION_CODE,BC_DATABASE_CONFIG.*
                                FROM BC_DB_LOCATION INNER JOIN BC_DATABASE_CONFIG ON BC_DATABASE_CONFIG.ID = BC_DB_LOCATION.DBCONFIG_ID
                                WHERE BC_DB_LOCATION.DBCONFIG_ID=:DBId";

                var result = await conn.QueryFirstAsync<Models.DatabaseConfig>(sQuery, new
                {
                    DBId = dbId
                });
                _connstr = CreateConnectionString(result.HOST, result.PORT, result.SERVICE_NAME, "system", "sys123");
            }
            catch (System.Exception)
            {

            }

            return _connstr;

        }

        public async Task<string> GetConnectionStringByDBId(int dbId)
        {
            string _connstr = "";
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            string sQuery = @"SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID =:DBId";

            var result = await conn.QueryFirstAsync<Models.DatabaseConfig>(sQuery, new
            {
                DBId = dbId
            });

            if (result == null)
                return null;

            return _connstr = CreateConnectionString(result.HOST, result.PORT, result.SERVICE_NAME, result.USER_ID, result.PASSWORD);
        }

        /// <summary>
        /// Gets cust id of consumer by their consumer no.
        /// </summary>
        /// <param name="consumerNo">int customerNo is the customer number of user</param>
        /// <param name="connectionString">string connectionString is the connection string for a particular database</param>
        /// <returns>string custId of consumer</returns>
        public async Task<string> GetCustIdByConsumerNo(int consumerNo, string connectionString)
        {
            string custId = null;
            try
            {
                using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
                string sQuery = @"SELECT cust_id
	                                    FROM bc_customers
	                                    WHERE customer_num =:ConsumerNo AND check_digit =:ConsumerCheckDigit";

                dynamic result = await conn.QueryFirstOrDefaultAsync(sQuery, new
                {
                    ConsumerNo = consumerNo.ToString().Substring(0, 7),
                    ConsumerCheckDigit = consumerNo.ToString().Substring(7, 1)
                });

                if (result != null)
                {
                    custId = result.CUST_ID.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return custId;
        }






    }
}
