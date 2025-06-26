using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Location;
using Core.Domain.DatabaseConfig;
using Core.Domain.Location;
using Core.Domain.ZoneCircle;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;
using Shared.DTOs.Location;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Location
{
    public class LocationRepository : ILocationRepository
    {
        
        private readonly IMapper _mapper;

        public LocationRepository(DapperContext context,IMapper mapper)
        {
            
            _mapper = mapper;
        }
        public Task<int> AddAsync(LocationsDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<LocationsDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LocationsDto>> GetAllAsync(PaginationParams pParams)
        {
            List<LocationsDto> locationsDtos= new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
           var result = con.Query<Locations>("DPG_MISCBILL_LOCATION_PKG.GET_LOCATION_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            locationsDtos = _mapper.Map(result.ToList(), locationsDtos);
            return locationsDtos;
        }

        public async Task<LocationsDto> GetByIdAsync(int id)
        {
            LocationsDto locationsDto = new LocationsDto();
            using var con = new OracleConnection(Connection.ConnectionString());
            var sql = @"select * from BC_DB_LOCATION l where l.ID = :id";
            var location = await con.QueryAsync<Locations>(sql, new { id });
            locationsDto=_mapper.Map(location.FirstOrDefault(),locationsDto);
            return locationsDto;
        }

        public async Task<List<LocationsDto>> GetLocationByDbArray(string[] dbCode)
        {
            List<LocationsDto> locations = new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            string dbCodes = string.Join(", ", dbCode);
            var sql = @$"select * FROM MISCBILLAPP_MS_LOCATION dl WHERE dl.db_code in ({dbCodes}) ORDER BY dl.CODE ";
             var result = await con.QueryAsync<Locations>(sql, new { dbCode });
            locations = _mapper.Map(result, locations);
            return locations;
        }

        public async Task<List<Locations>> GetLocationBydbcode(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_DB_CODE", value: dbCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Locations>("DPG_MISCBILL_LOCATION_PKG.GET_LOCATION_BY_DB_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        public async Task<Locations> GetLocationByLocation(string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString()); 
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Locations>("DPG_MISCBILL_LOCATION_PKG.GET_LOCATION_BY_LOCATION_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
          
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(LocationsDto entity)
        {
            throw new NotImplementedException();
           
        }

        public async Task<List<LocationDTO>> GetNonCustLocationDesc(string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_LOCATION_BY_CODE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        /// <summary>
        /// This method get location code by zone code
        /// </summary>
        /// <param name="zoneCode">Zone code</param>
        /// <returns>return list of location code.</returns>
        public async Task<List<LocationsDto>> GetLocationByZoneCode(string zoneCode)
        {
            List<LocationsDto> locations = new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Locations>("DPG_MISCBILL_LOCATION_PKG.GET_LOC_BY_ZONE_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
         
            locations = _mapper.Map(result, locations);
            return locations;
        }

        public async Task<List<Locations>> GetLocationByDBMIdAsync(string dbCode, int userId, int RoleId = 0)
        {
            IEnumerable<DbMappingDto> data;
            List<Locations> result = new List<Locations>();
            string sQuery = @"SELECT DM.* FROM MISCBILLAPP_MS_DB_MAPPING DM  WHERE DB_CODE=:DB_CODE AND DM.FULLACCESS = 1 AND DM.USERID=:UserId";
            using var con = new OracleConnection(Connection.ConnectionString());
            
                try
                {
                    data = await con.QueryAsync<DbMappingDto>(sQuery, new { DB_CODE = dbCode, UserId = userId });

                }
                catch (Exception)
                {
                    throw;
                }
            

            string Query = "";
            if (RoleId == 2) // role id 2 is admin
            {
                Query = @"SELECT L.* FROM MISCBILLAPP_MS_LOCATION L 
                        INNER JOIN MISCBILLAPP_MS_DATABASE D ON L.DB_CODE = D.CODE 
                        WHERE D.CODE =:dbCode";

                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            else if (!data.Any())
            {
                Query = @"    select * from MISCBILLAPP_MS_LOCATION L
                     INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM  ON L.ID= LM.LOCATION_ID
                     INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.ID = LM.DM_ID
                     WHERE DM.DB_CODE =:dbCode AND DM.USERID =:userId";

                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            else
            {
                Query = @"SELECT L.* FROM MISCBILLAPP_MS_LOCATION L
                     INNER JOIN MISCBILLAPP_MS_DATABASE D ON L.DB_CODE = D.CODE
                     INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.DB_CODE = D.CODE
                     WHERE DM.DB_CODE =:dbCode AND DM.USERID =:userId";
                var tempResult = await ExuecuteLocationByDbmId(Query, dbCode, userId);
                result.AddRange(tempResult);
            }
            return result;

        }

        private async Task<IEnumerable<Locations>> ExuecuteLocationByDbmId(string sQuery, string dbCode, int UserId)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            {
                try
                {
                    var result = await con.QueryAsync<Locations>(sQuery, new { dbCode = dbCode, userId = UserId });
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<List<Locations>> GetLocationByMultipleDBUserRoleId(string[] dbCodes, int userId, int RoleId = 0)
        {
            string dbCodeList = (dbCodes != null) ? "'" + string.Join("', '", dbCodes) + "'" : " ";
            IEnumerable<DbMappingDto> data;
            List<Locations> result = new List<Locations>();
            string sQuery = @$"SELECT DM.* FROM MISCBILLAPP_MS_DB_MAPPING DM WHERE DB_CODE IN ({dbCodeList}) AND DM.FULLACCESS = 1 AND DM.USERID=:UserId";
            using var con = new OracleConnection(Connection.ConnectionString());

            try
            {
                data = await con.QueryAsync<DbMappingDto>(sQuery, new { UserId = userId });
            }
            catch (Exception)
            {
                throw;
            }

            string Query = "";
            if (RoleId == 2) // role id 2 is admin
            {
                Query = @$"SELECT L.* FROM MISCBILLAPP_MS_LOCATION L 
                        INNER JOIN MISCBILLAPP_MS_DATABASE D ON L.DB_CODE = D.CODE 
                        WHERE D.CODE IN ({dbCodeList})";

                var tempResult = await ExuecuteLocationByDbMulDBId(Query, userId);
                result.AddRange(tempResult);
            }
            else if (!data.Any())
            {
                Query = @$"SELECT * from MISCBILLAPP_MS_LOCATION L
                     INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM  ON L.ID= LM.LOCATION_ID
                     INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.ID = LM.DM_ID
                     WHERE DM.DB_CODE IN ({dbCodeList}) AND DM.USERID =:userId";

                var tempResult = await ExuecuteLocationByDbMulDBId(Query, userId);
                result.AddRange(tempResult);
            }
            else
            {
                Query = @$"SELECT L.* FROM MISCBILLAPP_MS_LOCATION L
                     INNER JOIN MISCBILLAPP_MS_DATABASE D ON L.DB_CODE = D.CODE
                     INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.DB_CODE = D.CODE
                     WHERE DM.DB_CODE ({dbCodeList}) AND DM.USERID =:userId";
                var tempResult = await ExuecuteLocationByDbMulDBId(Query, userId);
                result.AddRange(tempResult);
            }
            return result;
        }
        private async Task<IEnumerable<Locations>> ExuecuteLocationByDbMulDBId(string sQuery, int UserId)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            {
                try
                {
                    var result = await con.QueryAsync<Locations>(sQuery, new {  userId = UserId });
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<List<LocationsDto>> GetLocationAndDeptCode()
        {
            List<LocationsDto> deptCode = new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Locations>("DPG_MISCBILL_LOCATION_PKG.GET_LOC_BY_DEPT_CODE", param: param, commandType: CommandType.StoredProcedure).ToList();
            // var sql = @$"SELECT NAME,CODE,DEPTCODE FROM MIBILL.MISCBILLAPP_MS_LOCATION";

            //var result = await con.QueryAsync<Locations>(sql);
            deptCode = _mapper.Map(result, deptCode);
            return deptCode;
        }

        public async Task<List<ZoneCircleLocationDTO>> GetZoneCircleByLocation(string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ZoneCircleLocationDTO>("MISC_REPORT_LOCATION_PKG.LOCATION_DATA_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllPrepaidDivision()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<DropdownResultForStringKey>("DPG_MISCBILL_POST_TO_PREPAID.DPD_GET_PREPAID_DIVISION", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            result = result.ToList();
            return result;
        }

        public async Task<List<DropdownResultForStringKey>> GetPrepaidDistrictByDivision(string divCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_DIVISION_CODE", value: divCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<DropdownResultForStringKey>("DPG_MISCBILL_POST_TO_PREPAID.DPD_GET_PREPAID_DIST_BY_DIV", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<DropdownResultForStringKey>> GetPrepaidThanaByDistrict(string distCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_DISTRICT_CODE", value: distCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<DropdownResultForStringKey>("DPG_MISCBILL_POST_TO_PREPAID.DPD_GET_PREPAID_THANA_BY_DIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<LocationsDto>> GetLocationByUserNameCircleCode(string userName, string circleCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<LocationsDto> dto = new List<LocationsDto>();
            string query = $@"
                           SELECT DISTINCT L.*  FROM MISCBILLAPP_MS_LOCATION L
                            INNER JOIN  MISCBILLAPP_MS_LOC_MAPPING LM ON L.ID =  LM.LOCATION_ID
                            INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON dm.id = lm.dm_id
                            WHERE dm.userid =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME) =:userName) AND  l.circle_code =:circleCode";
            var result = await con.QueryAsync<Locations>(query, new { UserName = userName.ToUpper(), CircleCode = circleCode });
            dto = _mapper.Map(result.ToList(), dto);
            return dto;
        }

        public async Task<List<LocationsDto>> GetLocationByUserNameZoneCode(string userName, string zoneCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<LocationsDto> dto = new List<LocationsDto>();
            string query = $@"
                           SELECT DISTINCT L.*  FROM MISCBILLAPP_MS_LOCATION L
                            INNER JOIN  MISCBILLAPP_MS_LOC_MAPPING LM ON L.ID = LM.LOCATION_ID
                            INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.id = LM.dm_id
                            WHERE DM.userid =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME) =:userName) AND  L.ZONE_CODE =:zoneCode";
            var result = await con.QueryAsync<Locations>(query, new { UserName = userName.ToUpper(), ZoneCode = zoneCode });
            dto = _mapper.Map(result.ToList(), dto);
            return dto;
        }
        
        public async Task<List<LocationsDto>> GetLocationByLocCode(string locCode)
        {
            List<LocationsDto> locations = new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @$"SELECT * FROM MISCBILLAPP_MS_LOCATION WHERE CODE IN ({locCode}) ORDER BY CODE";
            var result = await con.QueryAsync<Locations>(sQuery);
            locations = _mapper.Map(result, locations).ToList();
            return locations;
        }

        public async Task<LocationsDto> GetDivisionAndDistrictByLocation(string locCode)
        {
            LocationsDto locations = new LocationsDto();
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = $"SELECT LOCATION_DESC Name,DEPTCODE DeptCode ,DISTRICTCODE DistrictCode,DIVISIONCODE DivisionCode FROM MISCBILLAPP_MS_LOCATION WHERE CODE='{locCode}' ORDER BY CODE";
            locations = con.QueryFirstOrDefault<LocationsDto>(sQuery);
            return locations;
        }

        public async Task<List<LocationsDto>> GetLocationByUserNameCenterCode(string userName, string[] db)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<LocationsDto> dto = new List<LocationsDto>();
            string centerCodes = string.Join(", ", db);
            string query = @$"
                           SELECT DISTINCT L.*  FROM MISCBILLAPP_MS_LOCATION L
                            INNER JOIN  MISCBILLAPP_MS_LOC_MAPPING LM ON L.ID =  LM.LOCATION_ID
                            INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON dm.id = lm.dm_id
                            WHERE dm.userid =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME)=:userName) AND  L.DB_CODE IN ({centerCodes})";
            var result = await con.QueryAsync<Locations>(query, new { UserName = userName.ToUpper(), Location = db });
            dto = _mapper.Map(result, dto).ToList();
            return dto;
        }

        public async Task<List<LocationsDto>> GetLocationAndDeptCodeByCenterCode(string centerCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<LocationsDto> dto = new List<LocationsDto>();
            string query = $@"SELECT NAME Name,NAMEBN NameBn,CODE Code,DEPTCODE DeptCode 
                              FROM MISCBILLAPP_MS_LOCATION
                              WHERE DB_CODE={centerCode} ORDER BY CODE ASC";
            var result = await con.QueryAsync<LocationsDto>(query, new { CenterCode= centerCode });
            return result.ToList();
        }
    }
}
