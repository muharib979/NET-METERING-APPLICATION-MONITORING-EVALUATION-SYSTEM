using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.ChannelAPI;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using System.Xml.Serialization;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ChannelAPI
{
    public class ChannelApiRepository : IChannelApiRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepositry;

        public ChannelApiRepository(IConfiguration configuration, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepositry)
        {
            _configuration = configuration;
            _dbConfigRepo = dbConfigRepo;
            _commRepositry = commRepositry;
        }

        public async Task<Result> GetCustomerTranferList(PrePaidToPostPaidMOD model)
        {
            string url = _configuration["prepaidtopostpaid:CustomerListGetByDeptCode"];
            model.UserName = _configuration["prepaidtopostpaid:userName"];
            model.UserPass = _configuration["prepaidtopostpaid:password"];
            var reqXmll = model.ToReqXml();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(7);
            var reqXmlll = new FormUrlEncodedContent(new[]
                  {
                     new KeyValuePair<string, string>("reqXml", reqXmll)
                  });
            HttpResponseMessage responses = await client.PostAsync(url, reqXmlll);
            var customerJsonString = await responses.Content.ReadAsStringAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(Result));
            Result result;

            using (TextReader reader = new StringReader(customerJsonString))
            {
                result = (Result)serializer.Deserialize(reader);
            }
            return result;
        }

        public async Task<CustomerInformation> GetPrepaidCustomerByTrans(PrePaidToPostPaidDTO model)
        {
            string url = _configuration["prepaidtopostpaid:PrepaidCustomerByTran"];
            model.UserName = _configuration["prepaidtopostpaid:userName"];
            model.UserPass = _configuration["prepaidtopostpaid:password"];
            var reqXmll = model.ToRequestXml();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(7);
            var reqXmlll = new FormUrlEncodedContent(new[]
                  {
                     new KeyValuePair<string, string>("reqXml", reqXmll)
                  });

            HttpResponseMessage responses = await client.PostAsync(url, reqXmlll);
            var customerJsonString = await responses.Content.ReadAsStringAsync();

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerInformation));
            CustomerInformation result;

            using (TextReader reader = new StringReader(customerJsonString))
            {
                result = (CustomerInformation)serializer.Deserialize(reader);
            }
            return result;
        }

        public async Task<Result> GetPrepaidMODData(PrePaidToPostPaidMOD model, string locationCode)
        {

            DateTime date = DateTime.Parse(model.Fdate);
            string billCycleCode = date.ToString("yyyyMM");
            string monthBiginDate = date.ToString("dd-MMM-yy").ToUpper();
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yy").ToUpper();
            using var con = new OracleConnection(Connection.ConnectionString());
            string url = _configuration["prepaidtopostpaidmod:CreateCustomer"];
            model.UserName = _configuration["prepaidtopostpaidmod:userName"];
            model.UserPass = _configuration["prepaidtopostpaidmod:password"];
            var reqXmll = model.ToReqXml();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(30);
            var reqXmlll = new FormUrlEncodedContent(new[]
                  {
                     new KeyValuePair<string, string>("reqXml", reqXmll)
                  });
            HttpResponseMessage responses = await client.PostAsync(url, reqXmlll);
            var customerJsonString = await responses.Content.ReadAsStringAsync();

            //string xmlResponse = "XML response here";

            // Deserialize the XML response into a .NET object
            XmlSerializer serializer = new XmlSerializer(typeof(Result));
            Result result;

            using (TextReader reader = new StringReader(customerJsonString))
            {
                result = (Result)serializer.Deserialize(reader);
            }

            // Serialize the tariffs element into JSON format
            //string jsonTariffs = JsonSerializer.Serialize(result.tariffs);
            string countQuery = $"SELECT NVL(COUNT(*),0) FROM MISCBILL_PRE_MOD_DATA WHERE DEPT_CODE={model.DeptCode} AND BILL_CYCLE_CODE={billCycleCode}";
            int count = con.QueryFirstOrDefault<int>(countQuery);
            if (count > 0)
            {
                throw new DuplicateNameException("data  already exist! Please, try new one.");
            }
            else
            {

                string query = $"SELECT NVL(MAX(CODE),0) FROM MISCBILL_PRE_MOD_DATA";
                int code = con.QueryFirstOrDefault<int>(query);
                code = code + 1;
                OracleDynamicParameters param = new OracleDynamicParameters();
                foreach (var item in result.Tariffs.TariffList)
                {
                    string tariff = item.TariffName.Trim().Replace("Tariff : ", "");
                    string tariffs = tariff.Trim().Replace("(", "");
                    param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                    param.Add("P_CODE", value: code, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_TARIFF_NAME", value: tariffs, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_NO_OF_CUSTOMER", value: item.NoOfCustomer, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_VENDING_CUSTOMER", value: item.VendingCustomer, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_SOLD_UNIT", value: item.SoldUnit, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_SOLD_AMOUNT_VAT", value: item.SoldAmountVat, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_SOLD_AMOUNT", value: item.SoldAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_COLLECTION_AMOUNT_VAT", value: item.CollectionAmountVat, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_COLLECTION_AMOUNT", value: item.CollectionAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_CREATED_BY", value: model.CreatedBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_IS_TRANSFER", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_DEPT_CODE", value: model.DeptCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_FORM_DATE", value: model.Fdate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_TO_DATE", value: model.Tdate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_BILL_CYCLE_CODE", value: billCycleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_METER_RENT_1P", value: item.MeterRent1p, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_METER_RENT_3P", value: item.MeterRent3p, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_SOLD_AMRRENT", value: item.SoldAmountWithVatMeterRent, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    var saveTarrif = con.ExecuteAsync("DPG_MISCBILL_MOD.SAVE_PRE_MOD_DATA", param: param, commandType: CommandType.StoredProcedure);
                    var db = await _dbConfigRepo.GetDatabaseInsertByDbCodeAsync(model.DbCode); // Getting database info by id.
                    string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
                    //string connectionStrings = _commRepositry.CreateConnectionString("192.168.0.5", "1521", "DBJAM", "ebc", "ebc");
                    using var connection = new OracleConnection(connectionString);
                    string federNo = locationCode + "XX00";
                    //string feederQuery = $"SELECT '{locationCode}'||'XX00' FROM BC_FEEDER_SETUP WHERE LOCATION_CODE='{locationCode}' AND SUBSTN_CODE='XX'";
                    //string federNo = connection.QueryFirstOrDefault<string>(feederQuery);
                    //using var connection = new OracleConnection(connectionStrings);
                    //EBC.BC_PRE_PAID_MOD_INPUT
                    var sQuery = $@" INSERT INTO EBC.BC_PRE_PAID_MOD_INPUT (MONTH_BEGIN_DATE, BILL_CYCLE_CODE, LOCATION_CODE, FEEDER_NO, USAGE_CATEGORY_CODE, CUSTOMER_TYPE_CODE, LIVE_CONSUMER_MONTH_END_MNL, BILLED_CONSUMER_MNL, TOTAL_ENERGY_SOLD_MNL, TOTAL_PRINCIPAL_BILLED_MNL, TOTAL_VAT_BILLED_MNL, TOTAL_COLLECTION_MNL, TOTAL_VAT_COLLECTION_MNL, CREATE_BY,CREATE_DATE,REC_STATUS)
                                                                  VALUES ('{monthBiginDate}','{billCycleCode}','{locationCode}','{federNo}','{tariffs}',            '05',          {item.NoOfCustomer},          {item.NoOfCustomer},    {item.SoldUnit},       {item.SoldAmount},      {item.SoldAmountVat}, {item.CollectionAmount}, {item.CollectionAmountVat},      'MISCBILL','{formattedDate}','C' )";
                    connection.Execute(sQuery);
                }
                //var datas = JsonConvert.DeserializeObject<Result>(customerJsonString);
                return result;
            }


        }

        public async Task<(bool IsSaved, string messages)> TransferPrePaidServer(PostpaidCustDetailsDTO model)
        {
            //DateTime date = DateTime.ParseExact(model.LastReadingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //string billCycleCode = date.ToString("yyyyMM");
            using var con = new OracleConnection(Connection.ConnectionString());
            model.UserName = _configuration["prepaidtopostpaid:userName"];
            model.UserPass = _configuration["prepaidtopostpaid:password"];
            string url = _configuration["prepaidtopostpaid:CreateCustomer"];
            model.ServiceProviders = "01";
            model.CustomerType = "03";
            model.Telephone = model.MobileNumber;
            var reqXmll = model.ToReqXml();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(7);
            var reqXmlll = new FormUrlEncodedContent(new[]
                  {
                     new KeyValuePair<string, string>("reqXml", reqXmll)
                  });
            HttpResponseMessage responses = await client.PostAsync(url, reqXmlll);
            var customerJsonString = await responses.Content.ReadAsStringAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(XML));
            XML result;

            using (TextReader reader = new StringReader(customerJsonString))
            {
                result = (XML)serializer.Deserialize(reader);
            }

            //DateTime currentDate = DateTime.Now;
            //// string updatedDate = currentDate.ToString("dd/MM/yyyy");
            //string updatedDate = currentDate.ToString("dd-MMM-yy").ToUpper();

            //string dbQuery = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION WHERE MISCBILLAPP_MS_LOCATION.CODE='{model.LocationCode}'";
            //string dbCode = con.QueryFirstOrDefault<string>(dbQuery);
            ////string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            //string connectionString = _commRepositry.CreateConnectionString("192.168.0.5", "1521", "DBJAM", "ebc", "ebc");
            //using var connection = new OracleConnection(connectionString);
            //string readingQuery = $"SELECT READING_ID FROM BC_METER_READING_CARD_DTL WHERE CUST_ID={model.CustId} AND BILL_CYCLE_CODE='{billCycleCode}'  AND METER_TYPE_CODE='09'    AND TOD_CODE='01'  AND TIME_CYCLE_CODE='1'  AND PURPOSE_OF_RDNG='B'";
            //int redingId = connection.QueryFirstOrDefault<int>(readingQuery);
            //string rQuery = $"SELECT READING_DATE FROM BC_METER_READING_CARD_HDR WHERE READING_ID={redingId}";
            //string resultR = connection.QueryFirstOrDefault<string>(rQuery);
            //DateTime dateTime = DateTime.ParseExact(resultR, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //string readingDate = dateTime.ToString("dd-MMM-yy").ToUpper();
            //if (model.MeterTypeCode == "01")
            //{
            //    string query = $"UPDATE BC_METER_READING_CARD_DTL SET READING_DATE='{readingDate}', DEFECTIVE_CODE='{model.MeterConditionCode}'  ,  BATCH_PROCESS_FLAG='E',  MRS_STATUS='M',  CLS_READING= {model.LastReading} ,  ADVANCE=  {model.LastReading}-OPN_READING,  UPDATE_BY= 'MISCBILL',  UPDATE_DATE= '{updatedDate}', CUM_NUM=NULL WHERE CUST_ID={model.CustId} AND BILL_CYCLE_CODE='{billCycleCode}'  AND METER_TYPE_CODE='09'    AND TOD_CODE='01'  AND TIME_CYCLE_CODE='1'  AND PURPOSE_OF_RDNG='B'";
            //    connection.Execute(query);
            //    string eventQuery = $"UPDATE BC_CUSTOMER_EVENT_LOG SET MRS_ENTRY_DATE='{updatedDate}'  WHERE CUST_ID={model.CustId}  AND BILL_CYCLE_CODE='{billCycleCode}' AND CARD_GEN_DATE IS NOT NULL";
            //    connection.Execute(eventQuery);
            //}
            //else if (model.MeterTypeCode == "02")
            //{
            //    string oquery = $"UPDATE BC_METER_READING_CARD_DTL SET READING_DATE='{readingDate}', DEFECTIVE_CODE= '{model.MeterConditionCode}' ,  BATCH_PROCESS_FLAG='E',  MRS_STATUS='M',  CLS_READING= {model.LastReading} ,  ADVANCE=  {model.LastReadingOffPeak}-OPN_READING,  UPDATE_BY= 'MISCBILL',  UPDATE_DATE= '{updatedDate}', CUM_NUM=NULL WHERE CUST_ID={model.CustId} AND BILL_CYCLE_CODE='{billCycleCode}'  AND METER_TYPE_CODE='09'    AND TOD_CODE='02'  AND TIME_CYCLE_CODE='2'  AND PURPOSE_OF_RDNG='B'";
            //    connection.Execute(oquery);
            //    string pquery = $"UPDATE BC_METER_READING_CARD_DTL SET READING_DATE='{readingDate}', DEFECTIVE_CODE= '{model.MeterConditionCode}' ,  BATCH_PROCESS_FLAG='E',  MRS_STATUS='M',  CLS_READING= {model.LastReading} ,  ADVANCE=  {model.LastReadingPeak}-OPN_READING,  UPDATE_BY= 'MISCBILL',  UPDATE_DATE= '{updatedDate}', CUM_NUM=NULL WHERE CUST_ID={model.CustId} AND BILL_CYCLE_CODE='{billCycleCode}'  AND METER_TYPE_CODE='09'    AND TOD_CODE='02'  AND TIME_CYCLE_CODE='3'  AND PURPOSE_OF_RDNG='B'";
            //    connection.Execute(pquery);
            //    string eventQuery = $"UPDATE BC_CUSTOMER_EVENT_LOG SET MRS_ENTRY_DATE='{updatedDate}'  WHERE CUST_ID={model.CustId}  AND BILL_CYCLE_CODE='{billCycleCode}' AND CARD_GEN_DATE IS NOT NULL";
            //    connection.Execute(eventQuery);
            //}
            //else
            //{

            //}
            //var responseContent = await responses.Content.ReadAsStringAsync();
            int state = Int32.Parse(result.State);
            if (state == 12)
            {
                OracleDynamicParameters param = new OracleDynamicParameters();
                param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                param.Add("P_TRANSACTION_ID", value: result.TransId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_CUSTOMER_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_PREPAID_CUSTOMER_NUMBER", value: result.PrepaidCustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var updateQuery = con.Query<int>("DPG_MISCBILL_POST_TO_PREPAID.UPDATE_POST_PAID_PRE", param: param, commandType: CommandType.StoredProcedure);
                bool success = param.Get<int>("O_Status") > 0;
                return (success, result.Message);
            }
            else
            {
                string query = $"DELETE FROM MISCBILL_POSTPAID_TO_PRE WHERE CUSTOMER_NUM='{model.CustomerNumber}'";
                int queryResult = con.Execute(query);
                return (false, result.Message);
            }
        }
    }
}
