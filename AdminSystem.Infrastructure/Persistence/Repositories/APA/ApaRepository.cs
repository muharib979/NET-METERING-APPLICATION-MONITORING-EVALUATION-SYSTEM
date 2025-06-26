using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.APA;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.APA
{
    public class ApaRepository : IApaRepository
    {
        public async Task<List<ApaDTO>> GetAllApaData()
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var result = con.Query<ApaDTO>("DPG_MISCBILL_APA_PKG.DPD_GET_ALL_APA_DATA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ApaDTO>> GetApaByMonth(string billMonth)
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_BILL_MONTH", value: billMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<ApaDTO>("DPG_MISCBILL_APA_PKG.DPD_GET_APA_BY_BILLMONTH", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApaUitDTO> GetApaUnitIndexTarget(string performanceCode)
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_PERFORMANCE_CODE", value: performanceCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<ApaUitDTO>("DPG_MISCBILL_APA_PKG.DPD_GET_UNIT_INDEX_TARGET", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveApaData(List<ApaDTO> model)
        {
            bool success=false;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            foreach (var item in model)
            {
                param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                param.Add("P_STRATIGIC_CODE", value: item.StratigicCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PROGRAM_CODE", value: item.ProgramCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PERFORMANCE_CODE", value: item.PerformanceCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PERFORMANCE_NAME", value: item.PerformanceName, dbType: (OracleMappingType?)OracleDbType.NVarchar2, direction: ParameterDirection.Input);
                param.Add("P_UNIT_CODE", value: item.UnitCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_UNIT_NAME", value: item.UnitName, dbType: (OracleMappingType?)OracleDbType.NVarchar2, direction: ParameterDirection.Input);
                param.Add("P_INDEX_CODE", value: item.IndexName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_TARGET", value: item.TargetName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_BILL_MONTH", value: item.BillMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_FINANCIAL_AMOUNT", value: item.FinancialAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                param.Add("P_COMMENTS", value: item.Comments, dbType: (OracleMappingType?)OracleDbType.NVarchar2, direction: ParameterDirection.Input);
                var result = con.Execute("DPG_MISCBILL_APA_PKG.DPD_SAVE_APA", param: param, commandType: CommandType.StoredProcedure);
                success = param.Get<int>("O_Status") > 0;
            }
            return success;

        }


        public async Task<List<object>> GetAPAReportByBillMonth(string fromBillMonth, string toBillMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("START_MONTH", value: fromBillMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("END_MONTH", value: toBillMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<object>("DPG_MISCBILL_APA_PKG .DPD_APA_REPORT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            
            return result.ToList();
        }
    }
}
