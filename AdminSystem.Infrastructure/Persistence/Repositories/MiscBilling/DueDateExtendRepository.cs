using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.MiscBilling;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class DueDateExtendRepository : IDueDateExtendRepository
    {
        public async Task<int> ExtendDueDate(ExtendDueDateDTO model)
        {
            int result = 0;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NUMBER", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DUE_DATE", value: model.ExtendDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var results = con.Query<int>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_UPDATE_DUE_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            result = oracleDynamicParameters.Get<int>("O_Status");
            return result;
        }

        public async Task<int> ExtendDueDateForInst(ExtendDueDateDTO model)
        {
            int result = 0;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_INSTALLMENT_ID", value: model.BillId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NUMBER", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DUE_DATE", value: model.ExtendDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var results = con.Query<int>("DPG_MISCBILL_INSTALLMENT.DPD_UPDATE_INST_DUE_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            result = oracleDynamicParameters.Get<int>("O_Status");
            return result;
        }
    }
}
