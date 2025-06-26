

using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Bibliography;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class CalculateBillGenerateRepository : ICalculateBillGenerateRepository
    {
        private readonly IMapper _mapper;

        public CalculateBillGenerateRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<CalculateBills> GetCalculateDcRcBill(CalculateBills calculateBills)
        {
            decimal principleAmount = 0;
            CalculateBills calculate = new CalculateBills();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: calculateBills.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_TYPE_CODE", value: calculateBills.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var priAmount = con.QuerySingle<CalculateBills>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_DCRC_BILL_PRN_AMT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            principleAmount = (decimal)priAmount.PRN_AMOUNT;

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_PRN_AMT", value: principleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<CalculateBills>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_DCRC_BILL_VAT_AMT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            priAmount.VAT_AMOUNT = result.VAT_AMOUNT;
            return priAmount;
        }

        public async Task<CalculateBills> GetCalculateMiscChargeBill(CalculateBills calculateBills)
        {
            decimal principleAmount = 0;
            CalculateBills calculate = new CalculateBills();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: calculateBills.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_TYPE_CODE", value: calculateBills.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("p_xformer_kva", value: calculateBills.xformer_kva, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("p_xformer_day_rent", value: calculateBills.xformer_day_rent, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            var priAmount = con.QuerySingle<CalculateBills>("DPG_MISCBILL_CHARGE_CUST.DPD_DCRC_BILL_PRN_AMT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            principleAmount = (decimal)priAmount.PRN_AMOUNT;

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_PRN_AMT", value: principleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<CalculateBills>("DPG_MISCBILL_CHARGE_CUST.DPD_DCRC_BILL_VAT_AMT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            priAmount.VAT_AMOUNT = result.VAT_AMOUNT;
            return priAmount;
        }

        public async Task<CalculateBills> GetCalculateNonConsumerpenaltyBill(CalculateBills calculateBills)
        {
            decimal principleAmount = 0;
            CalculateBills calculate = new CalculateBills();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_SR_UNIT", value: calculateBills.PenaltyUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PEAK_UNIT", value: calculateBills.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: calculateBills.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_RATE_MULTIPLY", value: calculateBills.MultypliBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: calculateBills.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var priAmount = con.QuerySingle<CalculateBills>("DPG_MISCBILL_PNL_BILL_NONCUST.DPD_PENALTY_BILL_PRN_AMT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            principleAmount = (decimal)priAmount.PRN_AMOUNT;

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_PRN_AMT", value: principleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<CalculateBills>("DPG_MISCBILL_PNL_BILL_NONCUST.DPD_PENALTY_BILL_VAT_AMT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            priAmount.VAT_AMOUNT = result.VAT_AMOUNT;
            return priAmount;

        }

        public async Task<CalculateBills> GetCalculatePenaltyBill(CalculateBills calculateBills)
        {
            decimal principleAmount = 0;
            CalculateBills calculate = new CalculateBills();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_SR_UNIT", value: calculateBills.PenaltyUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PEAK_UNIT", value: calculateBills.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: calculateBills.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_RATE_MULTIPLY", value: calculateBills.MultypliBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: calculateBills.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var priAmount = con.QuerySingle<CalculateBills>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_PENALTY_BILL_PRN_AMT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            principleAmount = (decimal)priAmount.PRN_AMOUNT;

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_PRN_AMT", value: principleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<CalculateBills>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_PENALTY_BILL_VAT_AMT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            priAmount.VAT_AMOUNT = result.VAT_AMOUNT;
            return priAmount;
        }

        public async Task<CalculateBills> GetCalculateSupplementaryBill(CalculateBills calculateBills)
        {
            decimal principleAmount = 0;
            CalculateBills calculate = new CalculateBills();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TARIFF", value: calculateBills.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_SR_UNIT", value: calculateBills.PenaltyUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PEAK_UNIT", value: calculateBills.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: calculateBills.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_UNIT_FIXED", value: calculateBills.MultypliBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_FIXED_AMOUNT", value: calculateBills.SupplymentaryAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var priAmount = con.QuerySingle<CalculateBills>("DPG_MISCBILL_SUPPLMNTR_BILL. DPD_SUPP_BILL_PRN_AMT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            principleAmount = (decimal)priAmount.PRN_AMOUNT;

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: calculateBills.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: calculateBills.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_PRN_AMT", value: principleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: calculateBills.P_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<CalculateBills>("DPG_MISCBILL_SUPPLMNTR_BILL.DPD_SUPP_BILL_VAT_AMT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            priAmount.VAT_AMOUNT = result.VAT_AMOUNT;
            return priAmount;

        }
    }
}
