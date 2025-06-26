using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.APA;
using Core.Domain.APA;
using Dapper;
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
    public class ProgramRepository : IProgramRepository
    {
        private readonly IMapper _mapper;
        public ProgramRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> DeleteProgramBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.DELETE_PROGRAM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

        public async Task<List<ProgramDTO>> GetAllProgramDataByStrategicCode(string strategicCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT P.NAMEBN, P.CODE, P.OBJECTIVE_CODE OBJECTIVECODE FROM MISCBILLAPP_APA_PROGRAM P WHERE P.OBJECTIVE_CODE = : strategicCode";
            var result = await con.QueryAsync<ProgramDTO>(sQuery, new { strategicCode = strategicCode });
            return result.ToList();
        }

        public async Task<List<ProgramDTO>> GetAllProgramList()
        {  
            //List< ProgramDTO>ProgramDTO=new List<ProgramDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<ProgramDTO>("DPG_MISCBILL_APA_PKG.ALL_PROGRAM_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
           // ProgramDTO = _mapper.Map(result, ProgramDTO);
            return result;
        }

        public async Task<bool> SaveProgramBill(ProgramDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ORDERBY", value: model.OrderBy, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_OBJECTIVE_CODE", value: model.ObjectiveCode, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            if (model.Id != null)
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_PROGRAM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            else
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_PROGRAM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }

            return (isSuccess);
        }
    }
}
