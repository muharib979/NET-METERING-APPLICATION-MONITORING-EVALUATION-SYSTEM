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
    public class ModBillRepository : IModBillRepository
    {
        public async Task<List<ModBillDTO>> getModBillByLocCodeAndBillMonth(string locationCode, string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("P_BILL_CYCLE_CODE", value: billMonth.ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ModBillDTO>("DPG_MISCBILL_MOD.MOD_CALCULATE_AMOUNT", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }
}
