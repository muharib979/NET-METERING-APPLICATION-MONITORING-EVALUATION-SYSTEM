using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Bibliography;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class BillPrintRepository : IBillPrintRepository<PenaltyBillPrintDTO>
    {
        private readonly IMapper _mapper;

        public BillPrintRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<PenaltyBillPrintDTO>> PenaltyBillNonCustPrint(string customerNumber, string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("CUST_NUMBER", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillPrintDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_NUNCUST_BILL", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

      

        public async Task<List<PenaltyBillPrintDTO>> PenaltyBillSrPrint(string customerNumber, string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters parameters=new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("CUST_NUMBER", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillPrintDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_SR_PRINT", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;

        }

        public async Task<List<PenaltyBillPrintDTO>> PenaltyBillPrepaidSrPrint(string customerNumber, string billNumber)
      {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("CUST_NUMBER", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillPrintDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_PREPAID_SR_PRINT", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }


    }
}
