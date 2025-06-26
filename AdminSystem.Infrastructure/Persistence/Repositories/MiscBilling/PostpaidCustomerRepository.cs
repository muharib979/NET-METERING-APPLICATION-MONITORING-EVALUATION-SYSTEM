using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.ChannelAPI;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.Ministry;
using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using Shared.DTOs.Temporary;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection;
using static Core.Application.ClientApiSetup.PostPaidToPrePaidEndPoint;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class PostpaidCustomerRepository : IPostpaidCustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IChannelApiRepository _apiRepository;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepositry;
        public PostpaidCustomerRepository(IMapper mapper, IConfiguration configuration, IChannelApiRepository apiRepository, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepositry)
        {
            _mapper = mapper;
            _configuration = configuration;
            _apiRepository = apiRepository;
            _dbConfigRepo = dbConfigRepo;
            _commRepositry = commRepositry;
        }
        public async Task<PostpaidCustDetailsDTO> GetPostpaidCustomerByNumber(string custNumber, string locationCode)
        {
            try
            {
                PostpaidCustDetailsDTO customerDetails = new PostpaidCustDetailsDTO();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUST_NUMBER", value: custNumber.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<PostpaidCustDetailsDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_CUST_BILL_DETAILS_POSTPAID", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                var customerObject = result.FirstOrDefault();
                customerDetails = _mapper.Map(customerObject, customerDetails);
                return customerDetails;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PostToPrepaidViewDTO>> GetPostToPrepaidByDate(string startDate, string endDate, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_FROM_START_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TO_END_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PostToPrepaidViewDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_POSTTOPREPAID_BY_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<Result> GetPostToPrepaidForUpdate(string startDate, string endDate, string locationCode)
        {
            Result result = new Result();
            PrePaidToPostPaidMOD prePaid = new PrePaidToPostPaidMOD();
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $"SELECT CODE  || ' - ' || NAME LocationName, DEPTCODE FROM MISCBILLAPP_MS_LOCATION WHERE CODE='{locationCode}'";
            var deptCode = con.QueryFirstOrDefault<LocationsDTO>(query);
            prePaid.Fdate = startDate;
            prePaid.Tdate = endDate;
            prePaid.DeptCode = deptCode.DeptCode;
            result = await _apiRepository.GetCustomerTranferList(prePaid);
            foreach (var item in result.CustomerList.CustomersList)
            {
                item.LocationName = deptCode.LocationName;
            }
            return result;

            //using var con = new OracleConnection(Connection.ConnectionString());
            //OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            //oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            //oracleDynamicParameters.Add("P_FROM_START_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //oracleDynamicParameters.Add("P_TO_END_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //var result = con.Query<PostpaidCustDetailsDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_POSTTOPREPAIDSRC_FOR_UPDATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            //return result;
        }

        public async Task<CustomerInformation> GetPrepaidCustomerByTransId(string transID)
        {
            CustomerInformation result = new CustomerInformation();
            PrePaidToPostPaidDTO prePaid = new PrePaidToPostPaidDTO();
            prePaid.TransId = transID;
            result = await _apiRepository.GetPrepaidCustomerByTrans(prePaid);
            return result;
        }
        public async Task<List<PostpaidCustDetailsDTO>> GetPostToPrepaidSearchByDate(string startDate, string endDate, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_FROM_START_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TO_END_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PostpaidCustDetailsDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_POSTTOPREPAIDSRC_BY_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PostpaidCustDetailsDTO>> GetPrepaidList(string locationCode)
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<PostpaidCustDetailsDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_PREPAID_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> GetPrepaidMODDATA(PrePaidToPostPaidMOD model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $"SELECT NAME as LocationName,CODE as LocationCode FROM MISCBILLAPP_MS_LOCATION where DEPTCODE='{model.DeptCode}'";
            DeptCodeModel deptCode = con.QueryFirstOrDefault<DeptCodeModel>(query);
            Result message = await _apiRepository.GetPrepaidMODData(model, deptCode.LocationCode);
            message.LocationCode = deptCode.LocationCode;
            message.LocationName = deptCode.LocationName;
            return message;
        }

        public async Task<List<PostpaidCustDetailsDTO>> PostPaidToPrepaidPrint(string custNumber, string locationCode)
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value: custNumber.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<PostpaidCustDetailsDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_PREPAID_PRINT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool IsSaved, string messages)> SavePostpaidToPrepaid(PostpaidCustDetailsDTO model)
        {
            bool success = false;
            try
            {
                EndPointSetting endPoint = new EndPointSetting();
                Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1960, 1, 1)).TotalSeconds;
                endPoint.tran_id = unixTimestamp.ToString();
                using var con = new OracleConnection(Connection.ConnectionString());

                string query = $"SELECT COUNT(*) FROM MISCBILL_POSTPAID_TO_PRE WHERE CUSTOMER_NUM={model.CustomerNumber}";
                int count = con.QuerySingle<int>(query);
                if (count > 0)
                {
                    return (false, "Customer has existed");
                }
                else
                {
                    DateTime date = DateTime.ParseExact(model.LastReadingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string formattedDate = date.ToString("dd-MMM-yy").ToUpper();
                    OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                    oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                    oracleDynamicParameters.Add("P_TRANSACTION_ID", value: endPoint.tran_id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_USERNAME", value: endPoint.Username, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_PASSWORD", value: endPoint.Password, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_CUSTOMER_NAME", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_FATHER_NAME", value: model.FatherName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_CUSTOMER_TYPE", value: "03", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    //oracleDynamicParameters.Add("P_DIVISION", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_NID_NUMBER", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_MOBILE_NUMBER", value: model.MobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_POWER_UTILITY", value: model.PowerUtility, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_MAX_POWER", value: model.MaxPower, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_METER_OWNER", value: model.MeterOwnerCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_STATUS", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_USER", value: model.UserCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_AREA_CODE", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_PRVAC_NO", value: model.PrvAcNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_METER_TYPE_CODE", value: model.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_METER_NUM", value: model.MeterNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LAST_READING", value: model.LastReading, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LAST_READING_PK", value: model.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LAST_READING_OPK", value: model.LastBillReadingOfPk, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LASTBILLREADINGSR", value: model.LastBillReadingSr, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LASTBILLREADINGPK", value: model.LastBillReadingPk, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LASTBILLREADINGOFPK", value: model.LastBillReadingOfPk, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_CUT_OUT_DATE", value: formattedDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_TIN_NUMBER", value: model.Tin, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_SANCTION_LOAD", value: model.SanctionedLoad, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_ADDRESS", value: model.CustomerAddr, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_BUSINESS_TYPE", value: model.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_TARIFF_NAME", value: model.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_ARREAR_AMOUNT", value: model.ArrearAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_CONTACT_NAME", value: model.ContactName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LATITUDE", value: model.Latitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LONGITUDE", value: model.Longitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_DEVISION", value: model.DivisionCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_DISTRICT", value: model.DistrictCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_THANA", value: model.ThanaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_METER_SEAL_LEFT", value: model.LeftSerialNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_METER_SEAL_RIGHT", value: model.RightSerialNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_TERMINAL_LOSS", value: model.TerminalLoss, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    var result = con.Query<int>("DPG_MISCBILL_POST_TO_PREPAID.DPD_POST_TOPRE_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                    success = oracleDynamicParameters.Get<int>("O_Status") > 0;
                    if (success)
                    {
                        model.TransID = endPoint.tran_id;
                        return (await _apiRepository.TransferPrePaidServer(model));
                    }
                    else
                    {
                        return (false, "Failed To Save, Please try Again");
                    }

                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<int> UpdatePostToPrepaidCustomer(List<PostpaidCustDetailsDTO> model)
        {
            throw new NotImplementedException();
        }


        public async Task<List<CustomerDetailsDTOByBookNumber>> GetPostPaidCustomerByBookNumber(string bookNumber, string locationCode)
        {
            List<CustomerDetailsDTOByBookNumber> customerDetails = new List<CustomerDetailsDTOByBookNumber>();
            using var con = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locationCode}'";
            var dbCode = con.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
           
            using var connection = new OracleConnection(connectionString);
            string query = $@"select a.cust_id,a.bill_cycle_code,
                                CLS_KWH_SR_RDNG LastBillReadingSr,
                                CLS_KWH_PK_RDNG LastBillReadingPk,
                                CLS_KWH_OFPK_RDNG LastBillReadingOfPk,
                                a.location_code LocationCode,
                                a.CUSTOMER_NUM||a.CONS_CHK_DIGIT CustomerNumber,
                                b.CUSTOMER_NAME CustomerName,
                                d.ADDR_DESCR1||'-'||d.ADDR_DESCR2 CustomerAddr,
                                b.F_H_NAME FatherName,a.area_code AreaCode, 
                                substr(b.area_code,4) BillGroup,
                                substr(b.area_code,1,3) BookNumber,
                                a.METER_NUM_KWH MeterNum,
                                a.METER_COND_KWH,
                                e.DEFECTIVE_DESC MeterConditionDesc,
                                a.SANC_LOAD SanctionedLoad, b.WALKING_SEQUENCE WalkOrder,
                                a.TARIFF TariffDesc, 
                                f.total_arrear ArrearAmount
                                from bc_bill_image a,bc_customers b,(SELECT t1.cust_id,MAX(BILL_CYCLE_CODE) bill_cycle_code from  bc_bill_image  t,bc_customers t1
                                                       WHERE t.cust_id = t1.cust_id
                                                        AND t1.LOCATION_CODE =:locationCode 
                                                        and SUBSTR(t1.AREA_CODE, 1,3) = '{bookNumber}'
                                                        group by t1.cust_id) c,BC_CUSTOMER_ADDR d,BC_DEFECTIVE_CODE e,(select ih.cust_id,sum((NVL(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)-NVL(IH.PRINCIPAL_APPL,0)) + ( NVL(IH.LPS_AMT,0)+NVL(IH.LPS_ADJ,0)-NVL(IH.LPS_APPL,0))+
                                                    (NVL(IH.VAT_AMT,0)+NVL(IH.VAT_ADJ,0)-NVL(VAT_APPL,0)))  total_arrear from bc_invoice_hdr ih,bc_customers t1
                                                       WHERE ih.cust_id = t1.cust_id
                                                        AND t1.LOCATION_CODE =:locationCode 
                                                        and SUBSTR(t1.AREA_CODE, 1,3) = '{bookNumber}'
                                                        group by ih.cust_id) f
                                        where A.bill_cycle_code=c.bill_cycle_code
                                        and A.CUST_ID=b.cust_id
                                        and a.cust_id=c.cust_id
                                        AND B.LOCATION_CODE =:locationCode 
                                        AND A.LOCATION_CODE =:locationCode
                                        and SUBSTR(B.AREA_CODE, 1,3) = '{bookNumber}'
                                        AND d.ADDR_TYPE='B'
                                        AND d.ADDR_EXP_DATE IS NULL
                                        and a.cust_id=d.cust_id
                                        and A.METER_COND_KWH=e.DEFECTIVE_CODE(+)
                                        and a.cust_id=f.cust_id(+)
                                        and A.INVOICE_NUM is not null";
            var result = connection.Query<CustomerDetailsDTOByBookNumber>(query, new { locationCode }).ToList();
            return result;
        }

        public async Task<List<BillGroupDTO>> GetAllBillGroup(string locationCode)
        {
            List<BillGroupDTO> billGroup = new List<BillGroupDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locationCode}'";
            var dbCode = con.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);

            using var connection = new OracleConnection(connectionString);

            string query = $"SELECT BILL_GROUP BILL_GRP,BILL_GRP_DESCR FROM BC_BILL_GROUP ORDER BY BILL_GROUP";
            var result = connection.Query<BillGroup>(query).ToList();
            billGroup = _mapper.Map(result, billGroup);
            return billGroup;
        }

        public async Task<List<BlockNumDTO>> GetAllBookNumber(string locationCode, string billgroup)
        {
            List<BlockNumDTO> bookNumber = new List<BlockNumDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locationCode}'";
            var dbCode = con.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);

            using var connection = new OracleConnection(connectionString);

            string query = $"SELECT DISTINCT  BLOCK_NUM BlockNum FROM BC_AREA_CODE WHERE LOCATION_CODE='{locationCode}'  AND BILL_GRP='{billgroup}' ORDER BY BLOCK_NUM";
            bookNumber = connection.Query<BlockNumDTO>(query).ToList();
            return bookNumber;
        }

        public async Task<List<DivisionDTO>> GetDivisionForPrepaidCustomer()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $@"SELECT DIV_ID DivisionId,DIV_NAME DivisionName FROM MISCBILL_DIVISION";
            var result = con.Query<DivisionDTO>(query).ToList();
            return result;
        }

        public async Task<List<DistrictDTO>> GetDistrictForPrepaidCustomer()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $@"SELECT DIST_ID DistrictId,DIST_NAME DistrictName FROM MISCBILL_DISTRICT";
            var result = con.Query<DistrictDTO>(query).ToList();
            return result;
        }

        public async Task<List<ThanaDTO>> GetThanaForPrepaidCustomer()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string query = $@"SELECT THANA_ID ThanaId,THANA_NAME ThanaName FROM MISCBILL_THANA";
            var result = con.Query<ThanaDTO>(query).ToList();
            return result;
        }

        public async Task<string> PrepaidToPostPaid(PrepaidToPostPaidTransferDTO model)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yy").ToUpper();
            using var con = new OracleConnection(Connection.ConnectionString());
            string customerQuery = $@"select LOCATION_CODE,CUSTOMER_NUM,PREPAID_CUSTOMER_NUMBER,CUSTOMER_NAME,DB_CODE,
                                        METER_NUM,'C' REC_STATUS,TO_CHAR(TO_DATE(CUT_OUT_DATE, 'DD-MON-RR'), 'YYYYMM')  BILL_CYCLE_CODE,
                                        CUT_OUT_DATE,NVL(LAST_READING,0)LAST_READING,NVL(LAST_READING_PK,0)LAST_READING_PK,NVL(LAST_READING_OPK,0)LAST_READING_OPK from MISCBILL_POSTPAID_TO_PRE
                                        inner join MISCBILLAPP_MS_LOCATION on MISCBILLAPP_MS_LOCATION.CODE=miscbill_postpaid_to_pre.location_code
                                        where customer_num='{model.PostPaidCustomerNum}' and prepaid_customer_number='{model.PrePaidCustomerNum}'";
            PrepaidToPostPaidCustomerDTO response = con.QueryFirstOrDefault<PrepaidToPostPaidCustomerDTO>(customerQuery);
            var db = await _dbConfigRepo.GetDatabaseInsertByDbCodeAsync(response.DB_CODE); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var connection = new OracleConnection(connectionString);
            var sQuery = $@" INSERT INTO EBC.BC_PREPAID_CUST_INT (POST_PAID_CUST_NUM, PRE_PAID_CUST_NUM, CUSTOMER_NAME, LAST_RDG_DATE, LOCATION_CODE,CREATE_DATE,CREATE_BY,REC_STATUS,PRE_PAID_START_BILL_CYCLE,POST_METER_NUM,LAST_SR_RDG,LAST_OPK_RDG,LAST_PK_RDG)
                                                      VALUES ('{response.CUSTOMER_NUM}','{response.PREPAID_CUSTOMER_NUMBER}','{response.CUSTOMER_NAME}','{response.CUT_OUT_DATE}','{response.LOCATION_CODE}','{formattedDate}','MISCBILL','{response.REC_STATUS}','{response.BILL_CYCLE_CODE}','{response.METER_NUM}',{response.LAST_READING},{response.LAST_READING_OPK},{response.LAST_READING_PK})";

            var result = connection.Execute(sQuery);
            //var result = 1;
            if (result > 0)
            {
                return "Postpaid to Prepaid transfer successful";
            }
            else
            {
                return "Transfer failed. Invalid credentials provided";
            }
        }

        public async Task<string> FDMToPostpaidSave(PostpaidCustFDMDTO model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            con.OpenAsync();
            using (var tran = con.BeginTransaction())
            {
                try
                {

                    string query = $"SELECT COUNT(*) FROM MISCBILL_POSTPAID_TO_PRE WHERE CUSTOMER_NUM='{model.CustomerNumber}'";
                    int count = con.QuerySingle<int>(query);
                    if (count > 0)
                    {
                        return ("Customer has existed");
                    }
                    else
                    {
                        OracleDynamicParameters param = new OracleDynamicParameters();
                        param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                        param.Add("P_TRANSACTION_ID", value: model.TransId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_UserName", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Password", value: model.Password, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerNumber", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerName", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerType", value: model.CustomerType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LocationCode", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_FatherName", value: model.FatherName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_NID", value: model.NID, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PowerUtility", value: model.PowerUtility, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MaxPower", value: model.MaxPower, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Mobile", value: model.Mobile, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_AreaCode", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterTypeCode", value: model.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterNumber", value: model.MeterNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterRemoveDate", value: model.MeterRemoveDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TinNumber", value: model.TinNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_SanctionLoad", value: model.SanctionLoad, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                        param.Add("P_Address", value: model.Address, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_BusinessType", value: model.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TariffName", value: model.TariffName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_ArrearAmount", value: model.ArrearAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_ContactName", value: model.ContactName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Latitude", value: model.Latitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_Longitude", value: model.Longitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_Division", value: model.Division, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_District", value: model.District, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Thana", value: model.Thana, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterSealLeft", value: model.MeterSealLeft, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterSealRight", value: model.MeterSealRight, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TerminalLoss", value: model.TerminalLoss, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PrepaidCustomerNumber", value: model.PrepaidCustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PrepaidMeterNumber", value: model.PrepaidMeterNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingPeak", value: model.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingOffPeak", value: model.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingSr", value: model.LastReadingSr, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_InstalationDate", value: model.InstalationDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingSr", value: model.LastBillReadingSr, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingPeak", value: model.LastBillReadingPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingOffPeak", value: model.LastBillReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_WALK_ORDER", value: model.WalkOrder, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                        param.Add("P_TRANSFERBY", value: model.TransferBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        var result = con.Query<int>("DPG_MISCBILL_POST_TO_PREPAID.DPD_FDM_TO_POSTPAID_SAVE", param: param, commandType: CommandType.StoredProcedure);
                        bool success = param.Get<int>("O_Status") > 0;
                        if (success)
                        {
                            DateTime currentDate = DateTime.Now;
                            string formattedDate = currentDate.ToString("dd-MMM-yy").ToUpper();
                            string inputDate = model.MeterRemoveDate;
                            string desiredFormat = "yyMMM";
                            DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);

                            // Format the date as YYYYMM
                            string billcycle = parsedDate.ToString("yyyyMM");

                            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{model.LocationCode}'";
                            var dbCode = con.QueryFirstOrDefault<string>(querydb);
                            var db = await _dbConfigRepo.GetDatabaseInsertByDbCodeAsync(dbCode); // Getting database info by id.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
                            using var connection = new OracleConnection(connectionString);
                            string billCycleQuery = $@"SELECT TO_CHAR(TO_DATE(MAX(BILL_CYCLE_CODE), 'YYYYMM') + INTERVAL '1' MONTH, 'YYYYMM') AS NEXT_BILL_CYCLE_CODE FROM BC_BILL_CYCLE_CODE WHERE AREA_CODE = '{model.AreaCode}' AND location_code = '{model.LocationCode}'";
                            billcycle = connection.QueryFirstOrDefault<string>(billCycleQuery);
                            string sQuery;
                            if (model.LastReadingSr > 0)
                            {
                                sQuery = $@" INSERT INTO EBC.BC_PREPAID_CUST_INT (POST_PAID_CUST_NUM, PRE_PAID_CUST_NUM, CUSTOMER_NAME, LAST_RDG_DATE, LOCATION_CODE,CREATE_DATE,CREATE_BY,REC_STATUS,PRE_PAID_START_BILL_CYCLE,POST_METER_NUM,LAST_SR_RDG)
                                                      VALUES ('{model.CustomerNumber}','{model.PrepaidCustomerNumber}','{model.CustomerName}','{model.MeterRemoveDate}','{model.LocationCode}','{formattedDate}','MISCBILL','C','{billcycle}','{model.MeterNumber}',{model.LastReadingSr})";
                            }
                            else
                            {
                                sQuery = $@" INSERT INTO EBC.BC_PREPAID_CUST_INT (POST_PAID_CUST_NUM, PRE_PAID_CUST_NUM, CUSTOMER_NAME, LAST_RDG_DATE, LOCATION_CODE,CREATE_DATE,CREATE_BY,REC_STATUS,PRE_PAID_START_BILL_CYCLE,POST_METER_NUM,LAST_OPK_RDG,LAST_PK_RDG)
                                                      VALUES ('{model.CustomerNumber}','{model.PrepaidCustomerNumber}','{model.CustomerName}','{model.MeterRemoveDate}','{model.LocationCode}','{formattedDate}','MISCBILL','C','{billcycle}','{model.MeterNumber}',{model.LastReadingOffPeak},{model.LastReadingPeak})";

                            }
                            var results = connection.Execute(sQuery);
                            tran.Commit();
                            con.CloseAsync();
                            return "Save Successfully";
                        }
                        else
                        {
                            tran.Rollback();
                            con.CloseAsync();
                            return "Something Went Wrong";
                        }

                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }

            }
        }


        public async Task<List<ModPrepaidCustomerDTO>> GetModPrepaidCustomerr(string zoneCode, string locationCode, string fromDate, string toDate)
        {
            try
            {
                //bool isLocation = (locationCode != "0") ? true : false;
                List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
                if (zoneCode != "0")
                {
                    allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);
                }
                else
                {
                    allDatabase = await _dbConfigRepo.GetAllDbConfigListForFDMPrepaid();
                }
                var modPrepaidCustomer = new List<ModPrepaidCustomerDTO>();
                foreach (var database in allDatabase)
                {
                    using var connection = new OracleConnection(Connection.ConnectionString());
                    connection.Open();
                    string query = $@"select Z.NAME ZoneName from MISCBILLAPP_MS_DATABASE D
                                         INNER JOIN MISCBILLAPP_MS_ZONE Z ON D.ZONE_CODE=Z.CODE
                                            WHERE D.CODE='{database.CODE}'";
                    string zoneName = connection.QueryFirstOrDefault<string>(query);
                    connection.Close();
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database

                    using var ocConnection = new OracleConnection(connectionString);

                    ocConnection.Open();
                    string sQuery = $@"SELECT
                      MPTP.POST_PAID_CUST_NUM AS PostPaidCustomerNumber,
                      MPTP.PRE_PAID_CUST_NUM PrePaidCustomerNumber,
                      MPTP.CUSTOMER_NAME AS CustomerName,
                      L.DESCR AS LocationName,
                      MPTP.LAST_SR_RDG AS LastReadingSr,
                      MPTP.CLS_SR_RDG LastBillReadingSr,
                      MPTP.LAST_PK_RDG LastReadingPk ,
                      MPTP.CLS_PK_RDG LastBillReadingPk,
                      MPTP.LAST_OPK_RDG LastReadingOpk,
                      MPTP.CLS_OPK_RDG LastBillReadingOpk,
                      TO_CHAR(TO_DATE(MPTP.LAST_RDG_DATE, 'DD-MON-RR')) InstalationDate
                    FROM
                      BC_PREPAID_CUST_INT MPTP
                     INNER JOIN
                  BC_LOCATION_MASTER L ON MPTP.LOCATION_CODE = L.LOCATION_CODE
                 WHERE MPTP.LAST_RDG_DATE BETWEEN TO_DATE('{fromDate}', 'DD-MON-RR') AND TO_DATE('{toDate}', 'DD-MON-RR')
                   AND ('{locationCode}' = '0' OR MPTP.LOCATION_CODE ='{locationCode}')";

                    var result = await ocConnection.QueryAsync<ModPrepaidCustomerDTO>(sQuery);
                    modPrepaidCustomer.AddRange(result.ToList());
                    ocConnection.Close();
                }
                return modPrepaidCustomer;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        //public async Task<List<ModPrepaidCustomerDTO>> GetModPrepaidCustomerr(string zoneCode, string locationCode, string fromDate, string toDate)
        //{
        //    try
        //    {

        //        using var con = new OracleConnection(Connection.ConnectionString());
        //        OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
        //        oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        //        oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //        oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //        oracleDynamicParameters.Add("P_FROM_DATE", value: fromDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //        oracleDynamicParameters.Add("P_TO_DATE", value: toDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //        var result = con.Query<ModPrepaidCustomerDTO>("DPG_MISCBILL_POST_TO_PREPAID.DPD_MOD_PREPAID_CUSTOMER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public async Task<string> KWHToPostpaidSave(PostpaidCustFDMDTO model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            con.OpenAsync();
            using (var tran = con.BeginTransaction())
            {
                try
                {

                    string query = $"SELECT COUNT(*) FROM MISCBILL_POSTPAID_TO_PRE WHERE CUSTOMER_NUM='{model.CustomerNumber}'";
                    int count = con.QuerySingle<int>(query);
                    if (count > 0)
                    {
                        return ("Customer has existed");
                    }
                    else
                    {
                        OracleDynamicParameters param = new OracleDynamicParameters();
                        param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                        param.Add("P_TRANSACTION_ID", value: model.TransId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_UserName", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Password", value: model.Password, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerNumber", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerName", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_CustomerType", value: model.CustomerType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LocationCode", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_FatherName", value: model.FatherName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_NID", value: model.NID, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PowerUtility", value: model.PowerUtility, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MaxPower", value: model.MaxPower, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Mobile", value: model.Mobile, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_AreaCode", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterTypeCode", value: model.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterNumber", value: model.MeterNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterRemoveDate", value: model.MeterRemoveDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TinNumber", value: model.TinNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_SanctionLoad", value: model.SanctionLoad, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                        param.Add("P_Address", value: model.Address, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_BusinessType", value: model.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TariffName", value: model.TariffName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_ArrearAmount", value: model.ArrearAmount, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_ContactName", value: model.ContactName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Latitude", value: model.Latitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_Longitude", value: model.Longitude, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_Division", value: model.Division, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_District", value: model.District, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_Thana", value: model.Thana, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterSealLeft", value: model.MeterSealLeft, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_MeterSealRight", value: model.MeterSealRight, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_TerminalLoss", value: model.TerminalLoss, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PrepaidCustomerNumber", value: model.PrepaidCustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_PrepaidMeterNumber", value: model.PrepaidMeterNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingPeak", value: model.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingOffPeak", value: model.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastReadingSr", value: model.LastReadingSr, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_InstalationDate", value: model.InstalationDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingSr", value: model.LastBillReadingSr, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingPeak", value: model.LastBillReadingPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_LastBillReadingOffPeak", value: model.LastBillReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("P_WALK_ORDER", value: model.WalkOrder, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                        param.Add("P_TRANSFERBY", value: model.TransferBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        var result = con.Query<int>("DPG_MISCBILL_POST_TO_PREPAID.DPD_KWH_TO_POSTPAID_SAVE", param: param, commandType: CommandType.StoredProcedure);
                        bool success = param.Get<int>("O_Status") > 0;
                        if (success)
                        {
                            DateTime currentDate = DateTime.Now;
                            string formattedDate = currentDate.ToString("dd-MMM-yy").ToUpper();
                            string inputDate = model.MeterRemoveDate;
                            string desiredFormat = "yyMMM";
                            DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);

                            // Format the date as YYYYMM
                            string billcycle = parsedDate.ToString("yyyyMM");

                            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{model.LocationCode}'";
                            var dbCode = con.QueryFirstOrDefault<string>(querydb);
                            var db = await _dbConfigRepo.GetDatabaseInsertByDbCodeAsync(dbCode); // Getting database info by id.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
                            using var connection = new OracleConnection(connectionString);

                            //string billCycleQuery = $@"SELECT TO_CHAR(TO_DATE(MAX(BILL_CYCLE_CODE), 'YYYYMM') + INTERVAL '1' MONTH, 'YYYYMM') AS NEXT_BILL_CYCLE_CODE FROM BC_BILL_CYCLE_CODE WHERE AREA_CODE = '{model.AreaCode}' AND location_code = '{model.LocationCode}'";
                            //billcycle = connection.QueryFirstOrDefault<string>(billCycleQuery);
                            string sQuery;
                            if (model.LastReadingSr > 0)
                            {
                                sQuery = $@" INSERT INTO EBC.BC_PREPAID_CUST_INT (POST_PAID_CUST_NUM, PRE_PAID_CUST_NUM, CUSTOMER_NAME, LAST_RDG_DATE, LOCATION_CODE,CREATE_DATE,CREATE_BY,REC_STATUS,PRE_PAID_START_BILL_CYCLE,POST_METER_NUM,LAST_SR_RDG)
                                                      VALUES ('{model.CustomerNumber}','{model.PrepaidCustomerNumber}','{model.CustomerName}','{model.MeterRemoveDate}','{model.LocationCode}','{formattedDate}','MISCBILL','C','{billcycle}','{model.MeterNumber}',{model.LastReadingSr})";
                            }
                            else
                            {
                                sQuery = $@" INSERT INTO EBC.BC_PREPAID_CUST_INT (POST_PAID_CUST_NUM, PRE_PAID_CUST_NUM, CUSTOMER_NAME, LAST_RDG_DATE, LOCATION_CODE,CREATE_DATE,CREATE_BY,REC_STATUS,PRE_PAID_START_BILL_CYCLE,POST_METER_NUM,LAST_OPK_RDG,LAST_PK_RDG)
                                                      VALUES ('{model.CustomerNumber}','{model.PrepaidCustomerNumber}','{model.CustomerName}','{model.MeterRemoveDate}','{model.LocationCode}','{formattedDate}','MISCBILL','C','{billcycle}','{model.MeterNumber}',{model.LastReadingOffPeak},{model.LastReadingPeak})";

                            }
                            var results = connection.Execute(sQuery);
                            tran.Commit();
                            con.CloseAsync();
                            return " KWH to Postpaid transfer successful";
                        }
                        else
                        {
                            tran.Rollback();
                            con.CloseAsync();
                            return "Transfer failed. Invalid credentials provided.";
                        }

                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<LocationWiseCustomerDTO>> LocationWiseCustomerCount(string zoneCode, string locationCode)
        {
            try
            {
                bool isLocation = (locationCode != "0") ? true : false;
                List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
                if (zoneCode != "0")
                {
                    allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);
                }
                else
                {
                    allDatabase = await _dbConfigRepo.GetAllDbConfigListForFDMPrepaid();
                }
                var customerCount = new List<LocationWiseCustomerDTO>();
                foreach (var database in allDatabase)
                {
                    using var connection = new OracleConnection(Connection.ConnectionString());
                    connection.Open();
                    string query = $@"select Z.NAME ZoneName from MISCBILLAPP_MS_DATABASE D
                                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON D.ZONE_CODE=Z.CODE
                                               WHERE D.CODE='{database.CODE}'";
                    string zoneName = connection.QueryFirstOrDefault<string>(query);
                    connection.Close();
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseInsertByDbCodeAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                    
                    using var ocConnection=new OracleConnection(connectionString);

                    ocConnection.Open();
                    string sQuery = $@"select L.DESCR LocationName,L.LOCATION_CODE LocationCode, COUNT(P.POST_PAID_CUST_NUM) TotalCustomer , '{zoneName}' ZoneName from BC_PREPAID_CUST_INT P 
                                        INNER JOIN BC_LOCATION_MASTER L ON P.LOCATION_CODE=L.LOCATION_CODE
                                        WHERE ('{locationCode}'='0' OR P.LOCATION_CODE='{locationCode}' )
                                        GROUP BY L.DESCR,L.LOCATION_CODE,'{zoneName}'
                                        ORDER BY '{zoneName}' ASC ,L.LOCATION_CODE ASC";
                    var result=ocConnection.Query<LocationWiseCustomerDTO>(sQuery).ToList();
                    customerCount.AddRange(result);
                    ocConnection.Close();
                }
                    return customerCount;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
