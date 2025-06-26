using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
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
    public class CustomerInstallmentRepository : ICustomerInstallmentRepository
    {
        private readonly IMapper _mapper;

        public CustomerInstallmentRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<PenaltyBillViewDTO>> GetPenaltyBillInstallmentPlan(string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_CUST_DETAILS_INSTALL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PenaltyBillViewDTO>> GetSupplementaryInstallmentPlan(string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_CUST_DETAILS_INSTALL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }
}
