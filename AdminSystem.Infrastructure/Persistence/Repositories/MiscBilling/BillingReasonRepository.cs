using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.CustomeEntity;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class BillingReasonRepository : IBillingReasonRepository
    {
        private readonly IMapper _mapper;


        public BillingReasonRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<BillReasonDTO>> GetBillingReason()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var result = con.Query<BillReason>(" DPG_MISCBILL_PENALTY_BILL_GEN. DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<List<BillReasonDTO>> GetDCRCBillingReason()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BillReason>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<List<BillReasonDTO>> GetBillReasonForPenalty()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BillReason>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<List<BillReasonDTO>> GetBillReasonForPenaltyNonCust()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BillReason>("DPG_MISCBILL_PNL_BILL_NONCUST.DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<List<BillReasonDTO>> GetBillReasonForSupplementaryBill()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BillReason>("DPG_MISCBILL_SUPPLMNTR_BILL.DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<List<BillReasonDTO>> GetBillReasonForMiscChargeBill()
        {
            try
            {
                List<BillReasonDTO> billReasons = new List<BillReasonDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BillReason>("DPG_MISCBILL_CHARGE_CUST.DPD_BILL_REASON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                billReasons = _mapper.Map(result, billReasons);
                return billReasons;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }
    }
}
