using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Location;
using Core.Domain.Dbo;
using Core.Domain.MISReport;
using Core.Domain.Report;
using Core.Domain.Temporary;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using System.Drawing;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Common
{
    public class CommonRepository : ICommonRepository
    {

        private readonly IConfiguration _config;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private IConfiguration _configuration;
        private readonly ILocationRepository _locationRepo;
        private readonly IDbConnection _db;

        public CommonRepository(IConfiguration config, IDatabaseConfigRepository dbConfigRepo, IConfiguration configuration, ILocationRepository locationRepo, DapperContext context)
        {
            _config = config;
            _dbConfigRepo = dbConfigRepo;
            _configuration = configuration;
            _locationRepo = locationRepo;
            _db = context.GetDbConnection();
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
            var timeout = _configuration["DatabaseSettings:TimeOut"];
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
            using IDbConnection conn = Connection;
            try
            {
                string iQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID=:DbId";

                var result = await conn.QueryFirstAsync(iQuery, new
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
            using IDbConnection conn = Connection;
            try
            {
                string iQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID=:DbId";

                var result = await conn.QueryFirstAsync(iQuery, new
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
            using IDbConnection conn = Connection;
            try
            {
                string sQuery = @"SELECT BC_DB_LOCATION.LOCATION_CODE,BC_DATABASE_CONFIG.*
                                FROM BC_DB_LOCATION INNER JOIN BC_DATABASE_CONFIG ON BC_DATABASE_CONFIG.ID = BC_DB_LOCATION.DBCONFIG_ID
                                WHERE BC_DB_LOCATION.DBCONFIG_ID=:DBId";

                var result = await conn.QueryFirstAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new
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
            using IDbConnection conn = Connection;
            string sQuery = @"SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE ID =:DBId";

            var result = await conn.QueryFirstAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new
            {
                DBId = dbId
            });

            if (result == null)
                return null;

            return _connstr = CreateConnectionString(result.HOST, result.PORT, result.SERVICE_NAME, result.USER_ID, result.PASSWORD);
        }

        public async Task<string> GetMaxBillCycleCode(string connectionString)
        {
            try
            {
                using IDbConnection conn = new OracleConnection(connectionString);

                var sQuery = @" select max(bill_cycle_code) from bc_invoice_hdr ";
                var result = await conn.QueryAsync<string>(sQuery);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #region Get Bill Group 

        #region Multiple DB

        public List<Core.Domain.Report.BillGroup> MultipleDatabaseWiseBillGroupData { get; set; } = new List<Core.Domain.Report.BillGroup>();
        public async Task<List<Core.Domain.Report.BillGroup>> getDatabaseWiseBillGroupData(string[] dbCode)
        {
            try
            {

                List<Task> listOfTasks = new List<Task>();
                foreach (var code in dbCode)
                {
                    try
                    {
                        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                        string connectionString = CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                        listOfTasks.Add(Task.Run(() => GetBillGroupbyMultipleDbAsync(db.NAME, connectionString)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        continue;
                    }
                }

                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return MultipleDatabaseWiseBillGroupData.ToList();
        }

        public async Task GetBillGroupbyMultipleDbAsync(string dbName, string connectionString)
        {

            using (IDbConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    string sQuery = @"SELECT DISTINCT substr(AREA_CODE,4,2)  BILL_GROUP, :db  DB_NAME FROM EBC.BC_CUSTOMERS  ORDER BY substr(AREA_CODE,4,2)";
                    var result = await conn.QueryAsync<Core.Domain.Report.BillGroup>(sQuery, new { db = dbName });
                    MultipleDatabaseWiseBillGroupData.AddRange(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        #endregion Multiple DB

        #region Multiple Location
        public List<Core.Domain.Report.BillGroup> MultipleDatabaseLocationWiseBillGroupData { get; set; } = new List<Core.Domain.Report.BillGroup>();
        public async Task<List<Core.Domain.Report.BillGroup>> getLocationWiseBillGroupData(string[] dbCodes,string[] locationCode)
        {
            try
            {
                string locCode = string.Join(", ", locationCode);
                List<Task> listOfTasks = new List<Task>();
                foreach(var code in dbCodes)
                {
                    try
                    {
                        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                        string connectionString = CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                        listOfTasks.Add(Task.Run(() => GetBillGroupByMultipleLocationAsync(locationCode, db.NAME, connectionString)));
                    }
                    catch(Exception ex)
                    {
                        Log.Error(ex.Message);
                        continue;
                    }
                }
                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return MultipleDatabaseLocationWiseBillGroupData.ToList();
        }
        public async Task GetBillGroupByMultipleLocationAsync(string[] locationCodes, string dbName, string connectionString)
        {


            using (IDbConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    string locCode = "'" + string.Join("', '", locationCodes) + "'";
                    //string sQuery = "SELECT DISTINCT substr(AREA_CODE, 4, 2) AS BILL_GROUP FROM EBC.BC_CUSTOMERS where LOCATION_CODE = 'J6' ORDER BY substr(AREA_CODE, 4, 2)";
                    string sQuery = @$"SELECT DISTINCT substr(AREA_CODE,4,2) AS BILL_GROUP, :db as DB_NAME  FROM EBC.BC_CUSTOMERS  Where LOCATION_CODE in ({locCode})  ORDER BY substr(AREA_CODE,4,2)";
                    var result = await conn.QueryAsync<Core.Domain.Report.BillGroup>(sQuery, new { db = dbName });
                    MultipleDatabaseLocationWiseBillGroupData.AddRange(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion Multiple Location

        #endregion Get Bill Group 


        #region Get Book Multiple db and location

        #region Get Book by multiple Db

        public List<Book> MultipleDatabaseBookData { get; set; } = new List<Book>();
        public async Task<List<Book>> getMultipleDatabaseWiseBookData(string[] dBCodes, string billGroupId)
        {
            try
            {

                List<Task> listOfTasks = new List<Task>();
                foreach (var code in dBCodes)
                {
                    try
                    {
                        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                        string connectionString = CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                        listOfTasks.Add(Task.Run(() => GetBookByMultipeDbAndBillGroupAsync(billGroupId, db.NAME, connectionString)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        continue;
                    }
                }

                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return MultipleDatabaseBookData.ToList();
        }


        public async Task GetBookByMultipeDbAndBillGroupAsync(string billGroupId, string dbName, string connectionString)
        {


            using IDbConnection conn = new OracleConnection(connectionString);

            try
            {
                //bool locationCodeExist = (dto.Location_Id > 0) ? true : false; // Setting existence of location code depending on there being a value or not.
                //bool billGroupExistWhere = false;
                //bool bilGroupExistAnd = false;

                  bool  bilGroupExistAnd = (billGroupId != "0") ? true : false;

                string sQuery = @"  SELECT DISTINCT substr(AREA_CODE,1,3) || ' (' || count(*) || ')' AS BOOK_NUM, substr(AREA_CODE,1,3) as BOOK_ID, :db as DB_NAME FROM EBC.BC_CUSTOMERS "
                     + (bilGroupExistAnd ? @" Where substr(AREA_CODE,4,2)= :BillGroupId " : "") + @"
                    group by substr(AREA_CODE,1,3) order by count(*)  desc ";

                var result = await conn.QueryAsync<Book>(sQuery, new
                {
                    BillGroupId = billGroupId.ToString(),
                    db = dbName
                });
                MultipleDatabaseBookData.AddRange(result);

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion


        #region Get book by multiplte DB & Location

        public List<Book> MultipleLocationWiseBookData { get; set; } = new List<Book>();
        public async Task<List<Book>> getMultipleLocationWiseBookData(string[] dbCodes, string[] locationCodes, string billGroupId)
        {
            try
            {

                List<Task> listOfTasks = new List<Task>();

                foreach(var code in dbCodes)
                {
                    try
                    {
                        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                        string connectionString = CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                        listOfTasks.Add(Task.Run(() => GetBookByMultipleLocationAndBillGroupAsync(locationCodes, billGroupId, db.NAME, connectionString)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        continue;
                    }
                }
                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return MultipleLocationWiseBookData.ToList();
        }

        private async Task GetBookByMultipleLocationAndBillGroupAsync(string[] locationCodes, string billGroupId, string dbName, string connectionString)
        {

            using IDbConnection conn = new OracleConnection(connectionString);

            try
            {
                bool locationCodeExist = (locationCodes.Length > 0) ? true : false; // Setting existence of location code depending on there being a value or not.
                bool billGroupExistWhere = false;
                bool bilGroupExistAnd = false;

                if (!locationCodeExist && billGroupId != "0")
                {
                    billGroupExistWhere = (billGroupId != "0") ? true : false;

                }
                else
                {
                    bilGroupExistAnd = (billGroupId != "0") ? true : false;
                }
                string locCode = "'" + string.Join("', '", locationCodes) + "'";

                string sQuery = @"  SELECT DISTINCT substr(AREA_CODE,1,3) || ' (' || count(*) || ')' AS BOOK_NUM, substr(AREA_CODE,1,3) as BOOK_ID, :db as DB_NAME FROM EBC.BC_CUSTOMERS " + (locationCodeExist ? @$" WHERE LOCATION_CODE IN ({locCode}) " : "") +
                    (billGroupExistWhere ? @"WHERE substr(AREA_CODE,4,2) = :BillGroupId " : "") + (bilGroupExistAnd ? @" AND substr(AREA_CODE,4,2)= :BillGroupId " : "") + @"
                            group by substr(AREA_CODE,1,3) order by count(*)  desc ";

                var result = await conn.QueryAsync<Book>(sQuery, new
                {
                    BillGroupId = billGroupId.ToString(),
                    db = dbName
                });
                MultipleLocationWiseBookData.AddRange(result);
                //return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }


        #endregion Get book by multiplte DB & Location

        #endregion Get Book


        #region get db by user id

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDBByUserIdAsync(int userId, int RoleId)
        {


            string sQuery = @"SELECT DC.* FROM MISCBILLAPP_MS_DB_MAPPING DM INNER JOIN MISCBILLAPP_MS_DATABASE DC ON DC.CODE=DM.DB_CODE WHERE DC.IS_JOB =1 AND  USERID=:UserId";
            if (RoleId == 2)
                sQuery = @"SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE IS_JOB=1";
            try
            {
                var result = await _db.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { UserId = userId });
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion get db by user id


        #region get location Code by log in user id

        public async Task<object> GetLocationByDBMIdAsync(string dbCode, int userId, int RoleId = 0)
        {
            IEnumerable<DbMappingDto> data;
            List<object> result = new List<object>();
            string sQuery = @"SELECT DM.* FROM MISCBILLAPP_MS_DB_MAPPING DM  WHERE DB_CODE =:dbCode AND DM.FULLACCESS = 1 AND DM.USERID=:UserId";

            try
            {
                data = await _db.QueryAsync<DbMappingDto>(sQuery, new { dbCode = dbCode, UserId = userId });

            }
            catch (Exception)
            {
                throw;
            }


            string Query = "";
            if (RoleId == 2) //2 is admin role id
            {
                Query = @"   SELECT DBL.* FROM MISCBILLAPP_MS_LOCATION DBL 
                     INNER JOIN MISCBILLAPP_MS_DATABASE DBCON ON DBL.DB_CODE= DBCON.CODE
                     WHERE DBCON.CODE=:dbCode";

                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            else if (!data.Any())
            {
                Query = @" SELECT * FROM MISCBILLAPP_MS_LOCATION DBL 
                     INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM ON DBL.ID= LM.LOCATION_ID
                     INNER JOIN  MISCBILLAPP_MS_DB_MAPPING DM ON DM.ID = LM.DM_ID
                     WHERE DM.DB_CODE = '" + dbCode + "' AND DM.USERID=:UserId";

                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            else
            {
                Query = @"SELECT DBL.*FROM MISCBILLAPP_MS_LOCATION DBL
                     INNER JOIN MISCBILLAPP_MS_DATABASE DBCON ON DBL.DB_CODE = DBCON.CODE
                     INNER JOIN  MISCBILLAPP_MS_DB_MAPPING DM ON DM.DB_CODE = DBCON.CODE
                     WHERE DM.DB_CODE = " + dbCode + " AND DM.USERID =:UserId ";
                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            return result;

        }

        private async Task<IEnumerable<object>> ExuecuteLocationByDbmId(string sQuery, string dbCode, int UserId)
        {

            try
            {
                var result = await _db.QueryAsync<object>(sQuery, new { dbCode = dbCode, UserId = UserId });
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion get location code by login user id


    }
}
