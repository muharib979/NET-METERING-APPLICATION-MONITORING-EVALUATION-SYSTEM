using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.PaymentGateway;
using Dapper;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.PaymentGateway;

public class BkashGatewayRepository : IBkashAppRepository
{
    public async Task<BkashCheckBillResponseDTO> CheckBillBkash(string userName, string password, string customerNo, string? billMonth)
    {
		try
		{
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            
            using var connection = new OracleConnection(Connection.ConnectionString());
			OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            param.Add("P_USER_NAME", value: userName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_PASSWORD", value: password, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_CUSTOMER_NO", value: customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_BILL_MONTH", value: billMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = connection.QueryFirstOrDefault<BkashCheckBillResponseDTO>("DPG_PAYMENT_GATEWAY.DPG_BKASH_BILL_CHECK", param: param, commandType: CommandType.StoredProcedure);
            result.AmountBreakDown= connection.QueryFirstOrDefault<AmountBreakDownDTO>("DPG_PAYMENT_GATEWAY.DPG_BKASH_BILL_CHECK", param: param, commandType: CommandType.StoredProcedure);
            stopwatch.Stop();
            result.QueryTime=stopwatch.ElapsedMilliseconds.ToString();
            return result;
        }
        catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
    }

    public async Task<BkashCallBackResponseDTO> SaveBkashPaymentFromApp(string userName, string password, string customerNo, string? billMonth, string amount, string mobileNumber, string transactionId, string payTime)
    {
        try
        {
            DateTime middlewarePayTime = DateTime.Now; // Replace this with your actual DateTime object

            // Format the DateTime object as "yyyyMMddHHmmss"
            string formattedDateTime = middlewarePayTime.ToString("yyyyMMddHHmmss");

            BkashCallBackResponseDTO result = new BkashCallBackResponseDTO();
            bool isSuccess = false;
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $@"SELECT BILL_NUM,CUSTOMER_NAME,LOCATION_CODE,ADDRESS,TOTAL_BILL_AMOUNT
                           ,PRINCPAL_AMOUNT EnergyCost,VAT_AMOUNT VatAmount FROM BC_BILL_IMAGE WHERE CUSTOMER_NUM='{customerNo}' ";

            var response = con.QueryFirstOrDefault<BkashBillImageDTO>(query);
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_MISCBILL_TRAN_ID", value: transactionId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NO", value: customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NO", value: response.BILL_NUM, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_GATEWAY_CODE", value: "BKASH", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQUEST_CHANNEL_TYPE", value: "BKASHAPP", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: response.LOCATION_CODE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_TOTAL_AMOUNT", value: amount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NAME", value: response.CUSTOMER_NAME, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_ADDRESS", value: response.ADDRESS, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PHONE", value: mobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_PRINCIPAL_AMOUNT", value: response.EnergyCost, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REQ_VAT_AMOUNT", value: response.VatAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: userName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_MIDDLEWAREPAYTIME", value: formattedDateTime, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BKASH_PAY_TIME", value: payTime, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CARD_ISSUER", value: "BKash Mobile Banking", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CARD_BRAND", value: "MOBILEBANKING", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CARD_SUB_BRAND", value: "Classic", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CARD_ISSUER_COUNTRY", value: "Bangladesh", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CARD_TYPE", value: "BKASH-BKash", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BANK_TRAN_ID", value: transactionId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var saveResult = con.Query<int>("DPG_PAYMENT_GATEWAY.DPD_REQUEST_DATA_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            //return (isSuccess);
            if (isSuccess)
            {
                result.MiddlewarePayTime = formattedDateTime;
                result.TrxId = transactionId;
                result.TotalAmount = amount;
                result.ConsumerName = response.CUSTOMER_NAME;
                result.AmountBreakDown= con.QueryFirstOrDefault<AmountBreakDownDTO>(query);
                return result;

            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
