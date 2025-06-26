using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.APA;
using Core.Domain.APA;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.APA
{
    public class PerformanceIndexRepository : IPerformanceIndexRepository
    {
        private readonly IMapper _mapper;
        public PerformanceIndexRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> DeletePerformanceBill(int id)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.DELETE_PERFORMANCE_INDEX", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

       
        public async Task<List<PerfomanceIndexDto>> GetAllPerformanceIndexDataByProgramCode(string programCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT P.NAMEBN, P.CODE, P.PROGRAM_CODE PROGRAMCODE FROM APA_PERFORMANCE_INDEX P WHERE P.PROGRAM_CODE = : programCode";
            var result = await con.QueryAsync<PerfomanceIndexDto>(sQuery, new { programCode = programCode});
            return result.ToList();
        }

        public async Task<List<PerfomanceIndexDto>> GetAllPerformanceIndexList()
        {
            //List< PerfomanceIndexDto>perfomanceIndexDTO=new List< PerfomanceIndexDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<PerfomanceIndexDto>("DPG_MISCBILL_APA_PKG.ALL_PERFORMANCE_INDEX_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            // = _mapper.Map(result, perfomanceIndexDTO);
            return result;
        }

        public async Task<bool> SavePerformanceBill(PerfomanceIndexDto model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PROGRAM_CODE", value: model.ProgramCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_INDEX_UNIT_CODE", value: model.IndexUnitCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_VALUE", value: model.Value, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ORDERBY", value: model.OrderBy, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            if (model.Id != null)
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_PERFORMANCE_INDEX", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            else
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_PERFORMANCE_INDEX", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }

            return (isSuccess);
        }
    }
}
