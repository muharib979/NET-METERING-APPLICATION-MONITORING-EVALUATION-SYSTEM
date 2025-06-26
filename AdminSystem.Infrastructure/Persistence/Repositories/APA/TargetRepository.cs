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
    public class TargetRepository : ITargetRepository
    {
        private readonly IMapper _mapper;

        public TargetRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> DeleteTargetBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.DELETE_TARGET", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

     
        public async Task<List<FinancialYearDTO>> GetFinancialYearList()
        {
            List<FinancialYearDTO> FinancialYearDTO = new List<FinancialYearDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<FinancialYear>("DPG_MISCBILL_APA_PKG.ALL_FINANCIAL_YEAR_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            FinancialYearDTO = _mapper.Map(result, FinancialYearDTO);
            return FinancialYearDTO;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_mapper);
        }

        public async Task<List<TargetDTO>> GetTargetList()
        {
            //List<TargetDTO> TargetDTO = new List<TargetDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<TargetDTO>("DPG_MISCBILL_APA_PKG.ALL_TARGET_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
          //  TargetDTO = _mapper.Map(result, TargetDTO);
            return result;
        }

        public async Task<bool> SaveTargetBill(TargetDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);     
            oracleDynamicParameters.Add("P_FISCALYEAR_CODE", value: model.FiscalYearCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PERFORMANCE_INDEX_CODE", value: model.PerformanceIndexCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_VALUE", value: model.Value, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
       
            if (model.Id != null)
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_TARGET", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            else
            {
                var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_TARGET", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }

            return (isSuccess);
        }
    }
}
