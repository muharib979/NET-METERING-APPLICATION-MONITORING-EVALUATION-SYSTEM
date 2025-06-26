using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.ZoneCircle;
using Core.Domain.UnionPorishad;
using Core.Domain.ZoneCircle;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.DatabaseConfig;
using Shared.DTOs.Religious;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ZoneCircle
{
    public class ZoneCircleRepository : IZoneCircleRepository
    {
        private readonly IMapper _mapper;
        public ZoneCircleRepository(DapperContext context, IMapper mapper)
        {
            _mapper= mapper;
        }
        public async Task<List<Circle>> GetAllCircle()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Circle>("DPG_MISCBILL_ZONE_PKG.GET_CIRCLE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList(); 
            return result;
        }

        public async Task<List<Circle>> GetAllCircleByZoneCode(string zoneCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Circle>("DPG_MISCBILL_ZONE_PKG.GET_CIRCLE_BY_ZONE_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
           
        }

        public async Task<List<Zone>> GetAllZone()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Zone>("DPG_MISCBILL_ZONE_PKG.GET_ALL_ZONE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<ZoneDto>> GetZoneByUserID(string userName)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<ZoneDto> dto = new List<ZoneDto>();
            string query = $@"
                            SELECT  DISTINCT  Z.* FROM  MISCBILLAPP_MS_ZONE Z
                                INNER JOIN MISCBILLAPP_MS_LOCATION L ON L.ZONE_CODE =  Z.CODE
                                INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM  ON L.ID =  LM.LOCATION_ID
                                INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON dm.id = lm.dm_id
                                WHERE dm.userid =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME) =:userName)";
            var result = await con.QueryAsync<Zone>(query, new { UserName = userName.ToUpper() });
            dto = _mapper.Map(result.ToList(), dto);
            return dto;

        }

        public async Task<List<CircleDTO>> GetCircleByUserNameZoneCode(string userName, string zoneCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<CircleDTO> dto = new List<CircleDTO>();
            string query = $@"
                            SELECT DISTINCT C.*  FROM MISCBILLAPP_MS_LOCATION L
                                INNER JOIN  MISCBILLAPP_MS_LOC_MAPPING LM ON L.ID =  LM.LOCATION_ID
                                INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON dm.id = lm.dm_id
                                INNER JOIN MISCBILLAPP_MS_CIRCLE C ON c.code = L.CIRCLE_CODE
                                WHERE dm.userid =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME) =:userName) AND  l.zone_code =:zoneCode";
            var result = await con.QueryAsync<Circle>(query, new { UserName = userName.ToUpper(), ZoneCode=zoneCode});
            dto = _mapper.Map(result.ToList(), dto);
            return dto;
        }

        public async Task<List<DatabaseConfigDto>> GetDBCenterByUserName(string userName)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<DatabaseConfigDto> dto = new List<DatabaseConfigDto>();
            string query = $@"SELECT DISTINCT D.ID,D.NAME,D.CODE FROM MISCBILLAPP_MS_DATABASE D
                                INNER JOIN MISCBILLAPP_MS_LOCATION L ON D.CODE=L.DB_CODE
                                INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM ON L.ID=LM.LOCATION_ID
                                INNER JOIN MISCBILLAPP_MS_DB_MAPPING DM ON DM.ID=LM.DM_ID
                                WHERE DM.USERID =(SELECT DISTINCT ID FROM MISCBILLAPP_USER_DETAILS WHERE UPPER(USER_NAME) =:userName)";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(query, new { UserName = userName.ToUpper() });
            dto = _mapper.Map(result.ToList(), dto);
            return dto;
        }
    }
}
