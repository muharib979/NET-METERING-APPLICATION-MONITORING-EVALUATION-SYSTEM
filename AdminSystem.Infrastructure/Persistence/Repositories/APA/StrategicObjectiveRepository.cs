using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.APA;
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
    public class StrategicObjectiveRepository : IStrategicObjectiveRepository
    {
        private readonly IMapper _mapper;
        public StrategicObjectiveRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<StrategicObjectiveDto>> GetAllStrategicObjectiveList()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<StrategicObjectiveDto>("DPG_MISCBILL_APA_PKG.ALL_STRATEGIC_OBJECTIVE_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<bool> SaveStrategicObjectiveBill(StrategicObjectiveDto model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ORDERBY", value: model.OrderBy, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            if(model.Id != null)
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_STRATEGIC_OBJECTIVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            else
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_STRATEGIC_OBJECTIVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            
             return (isSuccess);
            
        }

        public async Task<bool> UpdateStrategicObjectiveBill(StrategicObjectiveDto model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ORDERBY", value: model.OrderBy, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CODE", value: model.Code, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.UPDATE_STRATEGIC_OBJECTIVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);
        }

        public async Task<int> DeleteStrategicObjectiveBill(int id)
        {
          
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value:id , dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.DELETE_STRATEGIC_OBJECTIVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

      
    }
}
