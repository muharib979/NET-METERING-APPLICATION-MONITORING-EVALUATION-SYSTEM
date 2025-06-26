using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.ConsumerBill;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.ConsumerBill;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ConsumerBill
{
    public class ConsumerBillRepository : IConsumerBillRepository
    {
        private readonly IMapper _mapper;

        public ConsumerBillRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<ConsumerBillDTO>> BillDetailsByBillNumber(string customerNumber, string billNumber, string user)
        {
            List<ConsumerBillDTO> conMap = new List<ConsumerBillDTO>();
            using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("P_CUST_NO", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("P_BILL_NO", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("P_USER", value: user, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.MISCBILL.ConsumerBill>("DPG_PAYBILL_PAY_GTWAY_TRANSCT.DPD_REQUEST_DATA", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            conMap = _mapper.Map(result, conMap);
            return conMap;
        }

        public async Task<List<Core.Domain.MISCBILL.ConsumerBill>> GetConsumerData(string customerNumber, string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            param.Add("P_CUST_NO", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_BILL_NO", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_USER", value: "admin", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.MISCBILL.ConsumerBill>("DPG_PAYBILL_PAY_GTWAY_TRANSCT.DPD_REQUEST_DATA", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<bool> SaveConsumerPayment(PaymentReceivedDataDTO model)
        {
            try
            {
                bool success = false;
                string createdDate = DateTime.Now.ToString("dd/MM/yyyy");
                using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
                OracleDynamicParameters param = new OracleDynamicParameters();
                param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                param.Add("P_MISCBILL_TRAN_ID", value: model.tran_id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CUST_NO", value: model.cust_no, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_BILL_NO", value: model.bill_no, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_GATEWAY_CODE", value: "SSL", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_GTWAY_RET_ERROR", value: model.error, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_SUCCESS_STATUS", value: model.success, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_BANK_TRAN_ID", value: model.bank_tran_id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PAY_DATE", value: model.tran_date, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PAY_TOTAL_AMOUNT", value: model.amount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                param.Add("P_CARD_TYPE", value: model.card_type, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CARD_NO", value: model.card_no, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CARD_ISSUER", value: model.card_issuer, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CARD_BRAND", value: model.card_brand, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CARD_SUB_BRAND", value: model.card_sub_brand, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CARD_ISSUER_COUNTRY", value: model.card_issuer_country, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_VERIFY_SIGN", value: model.verify_sign, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_VERIFY_SHA2", value: model.verify_sign_sha2, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_VERIFY_KEY", value: model.verify_key, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_GTWAY_STATUS", value: model.gateway_status, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_VAL_ID", value: model.val_id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_STORE_AMOUNT", value: model.store_amount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                param.Add("P_RISK_LEVEL", value: model.risk_level, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_RISK_TITLE", value: model.risk_title, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_REMARKS", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_USER", value: model.user, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var result = con.Query<int>("DPG_PAYBILL_PAY_GTWAY_TRANSCT.DPD_RECEIVED_DATA_SAVE", param: param, commandType: CommandType.StoredProcedure);
                success = param.Get<int>("O_Status") > 0;
                return success;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveConsumerRequestData(Core.Domain.MISCBILL.ConsumerBill model)
        {
            bool isSuccess = false;
            using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_MISCBILL_TRAN_ID", value: model.TransactionId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NO", value: model.CUSTOMER_NUM, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NO", value: model.BILL_NO, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_GATEWAY_CODE", value: "SSL", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQUEST_CHANNEL_TYPE", value: "WEB", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LOCATION_CODE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_TOTAL_AMOUNT", value: model.TOTAL_BILL_AMOUNT, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NAME", value: model.CUST_NAME, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_ADDRESS", value: model.CUST_ADDRESS, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PHONE", value: model.PHONE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_PRINCIPAL_AMOUNT", value: model.TOTAL_PRINCIPAL_AMOUNT, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_VAT_AMOUNT", value: model.TOT_VAT_AMOUNT, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "admin", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var result = con.Query<int>("DPG_PAYBILL_PAY_GTWAY_TRANSCT.DPD_REQUEST_DATA_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return isSuccess;
        }
    }
}
