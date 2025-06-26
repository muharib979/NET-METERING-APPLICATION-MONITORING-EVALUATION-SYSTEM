using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.DatabaseConfig;
using Oracle.ManagedDataAccess.Client;

namespace AdminSystem.Infrastructure.Persistence.Repositories.DatabaseConfig
{
    public class DatabaseConfigRepository : IDatabaseConfigRepository
    {
        private readonly IDbConnection _db;
        public DatabaseConfigRepository(DapperContext context) => _db = context.GetDbConnection();
        public Task<int> AddAsync(Core.Domain.DatabaseConfig.DatabaseConfig entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<Core.Domain.DatabaseConfig.DatabaseConfig> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllAsync(PaginationParams pParams)
        {
            
            string sql = @"Select * from MISCBILLAPP_MS_DATABASE where IS_ACTIVE = 1";

            

            var allDatabase = await _db.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sql, new { startRow = (pParams.PageNumber * pParams.pageSize) + 1, endRow = (pParams.pageSize * pParams.PageNumber), searchBy = pParams.SearchBy });

            return allDatabase.ToList();
        }

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigDDList()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var result = @"select * from MISCBILLAPP_MS_DATABASE where IS_ACTIVE=1 AND IS_JOB=1"; // 1 IS TRUE
            return (List<Core.Domain.DatabaseConfig.DatabaseConfig>)await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(result);
        }
        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigListWithIsJob(int isJob =1)
        {
            var boolJob = isJob == 1 ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var result = @"select * from MISCBILLAPP_MS_DATABASE where IS_ACTIVE=1 " + (boolJob ? " AND IS_JOB=:is_job": " "); // 1 IS TRUE
            return (List<Core.Domain.DatabaseConfig.DatabaseConfig>)await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(result, new {is_job = isJob});
        }
        public Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseByZoneCode(string zonecode)
        {
            
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @"SELECT DISTINCT DB_CODE CODE FROM MISCBILLAPP_GC_CUSTOMER WHERE ZONE_CODE =:zoneCode";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { zoneCode = zonecode });
            return result.ToList();
        }

        public async Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseDataByDbCodeAsync(string dbcode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = "SELECT * FROM MISCBILLAPP_MS_DATABASE WHERE CODE= :dbCode";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { dbCode = dbcode });
            return (Core.Domain.DatabaseConfig.DatabaseConfig)result.FirstOrDefault(); 
        }

        public async Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseInsertByDbCodeAsync(string dbcode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = "SELECT * FROM MISC_MS_DATABASE WHERE CODE= :dbCode";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { dbCode = dbcode });
            return (Core.Domain.DatabaseConfig.DatabaseConfig)result.FirstOrDefault();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Core.Domain.DatabaseConfig.DatabaseConfig entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseCodeByZoneCode(string zonecode)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @"Select distinct  db_code from MISCBILLAPP_MS_LOCATION where ZONE_CODE =:zoneCode";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { zoneCode = zonecode });
            return result.ToList();
        }

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseByUserIdRoleId(int userId, int roleId)
        {
            string sQuery="";

            using var con = new OracleConnection(Connection.ConnectionString());
            if (roleId == 2) 
            {
                sQuery = @"SELECT * FROM   MISCBILLAPP_MS_DATABASE D WHERE D.IS_JOB = 1";
            }
            else
            {
                sQuery = @"SELECT D.*FROM MISCBILLAPP_MS_DB_MAPPING  DM INNER JOIN MISCBILLAPP_MS_DATABASE D ON D.CODE = DM.DB_CODE WHERE D.IS_JOB = 1 AND DM.USERID =:UserId";
            }
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { userId=userId,roleId=roleId });
            return result.ToList();
        }

        public async Task<List<DropdownResultForStringKey>> GetDbByCircleCode(string circleCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @$"SELECT DISTINCT MD.CODE Key, MD.NAME Value  FROM MISCBILLAPP_MS_DATABASE MD INNER JOIN MISCBILLAPP_MS_LOCATION ML ON ML.DB_CODE = MD.CODE WHERE ML.CIRCLE_CODE = {circleCode}";
            var result = await con.QueryAsync<DropdownResultForStringKey>(sQuery);
            return result.ToList();
        }

        public async Task<DropdownResultForStringKey> GetDbByLocationCode(string locCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @$"SELECT MD.CODE Key, MD.NAME Value  FROM MISCBILLAPP_MS_DATABASE MD INNER JOIN MISCBILLAPP_MS_LOCATION ML ON ML.DB_CODE = MD.CODE WHERE ML.CODE = '{locCode}'";
            var result = await con.QueryFirstOrDefaultAsync<DropdownResultForStringKey>(sQuery);
            return result;
        }

        public async Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseDataBylocCodeAsync(string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = "select *  from MISCBILLAPP_MS_DATABASE  db inner join MISCBILLAPP_MS_LOCATION ml on db.code = ml.db_code where ml.code =:LOCATION_CODE";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { LOCATION_CODE = locationCode });
            return (Core.Domain.DatabaseConfig.DatabaseConfig)result.FirstOrDefault();
        }

        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigListForFDMPrepaid()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            con.Open();
            var query = @"select * from MISC_MS_DATABASE where CODE IN ('1','2','4','8')";
            var result= await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(query);
            con.Close();
            return (result.ToList());


        }
    }
}
