using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Ministry;
using Core.Application.Interfaces.ZoneCircle;
using Core.Domain.Ministry;
using Core.Domain.NonBengali;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Ministry;
using Shared.DTOs.Railway;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Ministry
{
    public class MinistryRepository : IMinistryRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        private readonly IZoneCircleRepository _zoneRepo;

        public MinistryRepository(DapperContext context, IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo, IZoneCircleRepository zoneRepo)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
            _zoneRepo = zoneRepo;
        }

        #region all ministry arrear
        public async Task<List<MinistryArrear>> GetAllMinistrybyCurrDate(string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("report_date", value: billMonth.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistryArrear>("MISC_REPORT_MINISTRY_PKG.ALL_MINISTRY_BY_DATE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        #endregion all ministry arrear

        #region Methods
        public async Task<List<MinistryDepartment>> GetAllMinistryDept()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<MinistryDepartment>("MISC_REPORT_MINISTRY_PKG.ALL_MINISTRY_DEPT_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;


        }

        public async Task<List<Core.Domain.Ministry.MinistryData>> GetAllMinistry()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Core.Domain.Ministry.MinistryData>("MISC_REPORT_MINISTRY_PKG.ALL_MINISTRY_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        #endregion

        #region ministry details
        public async Task<List<MinistryDetails>> GetMinistryAndDateWiseMinistryDetails(string code, bool? isRebate, string billMonth, string zoneCode, string locationCode, string circleCode, string reportType)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var isMinistryCode = code == "0" ? 1 : 0;
            string rebateCode = isRebate == true ? code == "29" ? "1" : null : null;

            var isZoneCode = zoneCode != "0" ? 1 : 0;
            var isCircleCode = circleCode != "0" ? 1 : 0;
            var isLocationCode = locationCode.Trim() != "0" ? isZoneCode == 1 ? 1 : 0 : 0;
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            param.Add("P_REPORT_DATE", value: billMonth.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //param.Add("P_IS_MINISTRY_CODE", value: isMinistryCode, dbType: (OracleMappingType?)OracleDbType.Int16, direction: ParameterDirection.Input);
            param.Add("P_MINISTRY_CODE", value: code, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_RELIGIOUS_CODE", value: rebateCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_IS_ZONE_CODE", value: isZoneCode, dbType: (OracleMappingType?)OracleDbType.Int16, direction: ParameterDirection.Input);
            param.Add("P_IS_LOCATION_CODE", value: isLocationCode, dbType: (OracleMappingType?)OracleDbType.Int16, direction: ParameterDirection.Input);
            param.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var result = con.Query<MinistryDetails>("MISC_REPORT_MINISTRY_PKG.MINISTRY_DATE_DETAILS_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        #endregion

        #region ministry summary
        public async Task<List<MinistrySummary>> GetMinistrySummaryByDate(string zoneCode, string reportDate, string? locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            locationCode = (locationCode == null || locationCode == "0") ? "0" : locationCode;


            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistrySummary>("MISC_REPORT_MINISTRY_PKG.MINISTRY_SUMMARY_ZON_WISE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        #endregion


        #region ministry details (online)
        private async Task<List<GetCustomerArrearModel>> GetAllCustomerDataAsync(string dbCode, string billMonth, int NoOfMonth, int ArrrearFrom, int ArrrearTo, int?[] custIds, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string locationCode)
        {

            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.

            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

            if (billMonth == null)
            {
                string maxBillCycleNumber = await _commRepo.GetMaxBillCycleCode(connectionString);
                string createDateString = maxBillCycleNumber.Substring(maxBillCycleNumber.Length - 2) + "/" + "01/" + maxBillCycleNumber.Substring(0, 4);
                var convertBillMonth = DateTime.Parse(createDateString);
                //billMonth = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
                billMonth = maxBillCycleNumber;

            }


            using IDbConnection conn = new OracleConnection(connectionString);

            bool fromAmountExist = (ArrrearFrom > 0) ? true : false;
            bool toAmountExist = (ArrrearTo > 0) ? true : false;
            bool NOMExist = (NoOfMonth != 0) ? true : false;
            bool isLocation = (locationCode != "0") ? true : false;
            string conditions = !isAll ? " and i.PRINCIPAL " + (isPrincipal ? ">" : "=") + @" 0
                    and i.LPS " + (isLPS ? ">" : "=") + @" 0
                    and i.VAT " + (isVAT ? ">" : "=") + @" 0" : " ";


            var customers = new List<GetCustomerArrearModel>();
            try
            {

                int arraySize = custIds.Length;
                int chunkSize = 500;
                int skipChunkSize = 0;

                //while (skipChunkSize < arraySize)
                //{
                    string query = " c.cust_id ";

                    string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                    skipChunkSize += chunkSize;

                    query = query + " in (" + _custIds + @") ";

                var sQuery3 = @" select  (c.customer_num || c.check_digit) CONSUMER_NO
                ,G.LOCATION_CODE
                ,i.NOM
                ,i.PRINCIPAL PRN
                ,i.LPS
                ,i.VAT
                ,i.PRINCIPAL+i.LPS+i.VAT TOTAL
                ,:BillMonth BILL_CYCLE_CODE
                ,(ca.addr_descr1 ||  ca.addr_descr2 || ca.addr_descr3)  ADDRESS
                ,c.customer_name CUSTOMER_NAME
                ,G.DESCR LOCATION_DESC
       
                FROM EBC.BC_CUSTOMERS C,
                EBC.BC_CUSTOMER_ADDR CA,
                EBC.BC_LOCATION_MASTER G,
                ( SELECT CUST_ID,COUNT(BILL_CYCLE_CODE) NOM,
                    SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) PRINCIPAL,
                SUM(LPS_AMT+LPS_ADJ-LPS_APPL) LPS,
                SUM(VAT_AMT+VAT_ADJ-VAT_APPL) VAT
                from bc_invoice_hdr 
                where  bill_cycle_code <= :BillMonth
                AND ((PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) + (LPS_AMT+LPS_ADJ-LPS_APPL)+
                (VAT_AMT+VAT_ADJ-VAT_APPL)) > 100
                group by cust_id " +
                (NOMExist ? @"having COUNT(BILL_CYCLE_CODE)>= :noOfMonth " : " ") +
                @" ) I 
                    
                where  c.cust_id=CA.cust_id
                and   G.LOCATION_CODE = C.LOCATION_CODE
                and c.cust_id=I.cust_id
                AND ca.addr_type = 'B' " + (isLocation ? "AND C.LOCATION_CODE =:location_code " : " ") +
            @" AND ca.addr_exp_date IS NULL " +
                (fromAmountExist ? " AND  i.PRINCIPAL+i.LPS+i.VAT >=:FromAmount " : " ") +
                (toAmountExist ? " AND  i.PRINCIPAL+i.LPS+i.VAT <=:ToAmount " : " ") + conditions;
                //+ @" AND " + query;

                // var sQuery3 = @" select 
                // a.CUSTOMER_NUM||a.CHECK_DIGIT CONSUMER_NO,
                // a.location_code LOCATION_CODE,
                // i.NOM,
                // i.Principal_Arrear PRN,
                // i.LPS_arrear LPS,
                // i.vat_Arrear VAT,
                // i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear TOTAL,
                // :BillMonth bill_cycle_code,
                // e.addr_descr1 || e.addr_descr2 || e.addr_descr3 AS ADDRESS,
                // substr(a.CUSTOMER_NAME,1,35) CUSTOMER_NAME,
                // g.DESCR LOCATION_DESC
                // from ebc.bc_customers a,
                // ebc.bc_customer_category c,
                // ebc.bc_category_master cm,
                // ebc.bc_customer_addr e,
                // ebc.bc_monthly_likely f,
                // ebc.bc_location_master g,
                // (select cust_id,count(1) NOM,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                // sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear, 
                // sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr where  bill_cycle_code <= :BillMonth
                // group by cust_id) i,
                //  (select DISTINCT cust_id,max(bill_cycle_code) as BILL_CYCLE_CODE from bc_invoice_hdr where bill_cycle_code <= :BillMonth 
                // group by cust_id) k,(select distinct M.CUST_ID , max(M.CONNECTION_ID) AS CONNECTION_ID  from EBC.BC_CUSTOMER_METER m  group by M.CUST_ID ) MT,
                // EBC.BC_CUSTOMER_METER M
                // where  a.cust_id=e.cust_id
                // and  a.cust_id=f.cust_id 
                // and a.cust_id=i.cust_id 
                // and c.CUST_ID=a.CUST_ID 
                // and c.cat_id=cm.category_id and c.exp_date is null
                // and a.LOCATION_CODE=g.LOCATION_CODE 
                // and f.EXP_DATE is null and e.addr_type='B'
                // and e.ADDR_EXP_DATE is null and i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear>1 
                // AND A.CUST_ID = MT.CUST_ID
                // AND MT.CONNECTION_ID = M.CONNECTION_ID
                // --and a.customer_status_code<>'D' 
                // and i.cust_id=k.cust_id
                //-- AND BI.INVOICE_NUM is not null
                //  AND  k.bill_cycle_code <= :BillMonth  AND  i.NOM >= :noOfMonth
                //  " + (isLocation ? "AND A.LOCATION_CODE =:location_code " : " ") +
                // (fromAmountExist ? " AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear >=:FromAmount " : " ") +
                // (toAmountExist ? " AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear <= :ToAmount" : " ") +
                // @" order by    k.bill_cycle_code ASC";

                var result = await conn.QueryAsync<GetCustomerArrearModel>(sQuery3, new { BillMonth = billMonth, noOfMonth = NoOfMonth, ToAmount = ArrrearTo, FromAmount = ArrrearFrom, location_code = locationCode });

                    customers.AddRange(result.ToList());

                //}
                return customers;

            }
            catch (Exception)
            {
                throw;
            }

        }
        private async Task<List<GetCustomerArrearModel>> GetAllCustomerSummaryDataAsync(string dbCode, string billMonth, int NoOfMonth, int ArrrearFrom, int ArrrearTo, int?[] custIds, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string locationCode)
        {

            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.

            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

            if (billMonth == null)
            {
                string maxBillCycleNumber = await _commRepo.GetMaxBillCycleCode(connectionString);
                string createDateString = maxBillCycleNumber.Substring(maxBillCycleNumber.Length - 2) + " / " + "01/" + maxBillCycleNumber.Substring(0, 4);
                var convertBillMonth = DateTime.Parse(createDateString);
                billMonth = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

            }


            using IDbConnection conn = new OracleConnection(connectionString);

            bool fromAmountExist = (ArrrearFrom > 0) ? true : false;
            bool toAmountExist = (ArrrearTo > 0) ? true : false;
            bool NOMExist = (NoOfMonth != 0) ? true : false;
            bool isLocation = (locationCode != "0") ? true : false;
            string conditions = !isAll ? " and i.PRINCIPAL " + (isPrincipal ? ">" : "=") + @" 0
                    and i.LPS " + (isLPS ? ">" : "=") + @" 0
                    and i.VAT " + (isVAT ? ">" : "=") + @" 0" : " ";


            var customers = new List<GetCustomerArrearModel>();
            try
            {

                int arraySize = custIds.Length;
                int chunkSize = 500;
                int skipChunkSize = 0;

                while (skipChunkSize < arraySize)
                {
                    string query = " c.cust_id ";

                    string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                    skipChunkSize += chunkSize;

                    query = query + " in (" + _custIds + @") ";

                    var sQuery3 = @" select  (c.customer_num || c.check_digit) CONSUMER_NO
                ,G.LOCATION_CODE
                ,i.NOM
                ,i.PRINCIPAL PRN
                ,i.LPS
                ,i.VAT
                ,i.PRINCIPAL+i.LPS+i.VAT TOTAL
                ,:BillMonth BILL_CYCLE_CODE
                ,(ca.addr_descr1 ||  ca.addr_descr2 || ca.addr_descr3)  ADDRESS
                ,c.customer_name CUSTOMER_NAME
                ,G.DESCR LOCATION_DESC
       
                FROM EBC.BC_CUSTOMERS C,
                EBC.BC_CUSTOMER_ADDR CA,
                EBC.BC_LOCATION_MASTER G,
                ( SELECT CUST_ID,COUNT(BILL_CYCLE_CODE) NOM,
                    SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) PRINCIPAL,
                SUM(LPS_AMT+LPS_ADJ-LPS_APPL) LPS,
                SUM(VAT_AMT+VAT_ADJ-VAT_APPL) VAT
                from bc_invoice_hdr 
                where  bill_cycle_code <= :BillMonth
                AND ((PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) + (LPS_AMT+LPS_ADJ-LPS_APPL)+
                (VAT_AMT+VAT_ADJ-VAT_APPL)) > 0
                group by cust_id " +
                    (NOMExist ? @"having COUNT(BILL_CYCLE_CODE)>= :noOfMonth " : " ") +
                    @" ) I 
                    
                where  c.cust_id=CA.cust_id
                and   G.LOCATION_CODE = C.LOCATION_CODE
                and c.cust_id=I.cust_id
                AND ca.addr_type = 'B' " + (isLocation ? "AND C.LOCATION_CODE =:location_code " : " ") +
                @" AND ca.addr_exp_date IS NULL " +
                    (fromAmountExist ? " AND  i.PRINCIPAL+i.LPS+i.VAT >=:FromAmount " : " ") +
                    (toAmountExist ? " AND  i.PRINCIPAL+i.LPS+i.VAT <=:ToAmount " : " ") + conditions +
                    @" AND " + query;



                    var result = await conn.QueryAsync<GetCustomerArrearModel>(sQuery3, new { BillMonth = billMonth, noOfMonth = NoOfMonth, ToAmount = ArrrearTo, FromAmount = ArrrearFrom, location_code = locationCode });

                    customers.AddRange(result.ToList());

                }
                return customers;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<GetCustomerArrearModel>> GetCustomerArrearDetails(string billMonth,  int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string dbCode, string locationCode)
        {
            var result = new List<GetCustomerArrearModel>();
            var zoneData = new List<CustomerZoneModel>();
            var margeData = new List<GetCustomerArrearModel>();
            if (dbCode != "0")
            {
                var custIdDataList = await GetAllCustIdByDbCode(dbCode, locationCode);
                var zoneWithConsumer = await GetCusttomerWithZoneByDbCode( dbCode, locationCode);

                if (custIdDataList.Count() != 0)
                {
                    result.AddRange(await GetAllCustomerDataAsync(dbCode, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                }
                if (zoneWithConsumer.Count() != 0)
                {
                    zoneData.AddRange(zoneWithConsumer);
                }
            }
            else
            {
                var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
                foreach (var database in allDatabases)
                {
                    var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, "2");
                    var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(database.CODE, "2");

                    if (custIdDataList.Count() != 0)
                    {
                        result.AddRange(await GetAllCustomerDataAsync(database.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                    }
                    if (zoneWithConsumer.Count() != 0)
                    {
                        zoneData.AddRange(zoneWithConsumer);
                    }
                }

            }
            margeData.AddRange((from p in result

                                from d in zoneData
                                where p.CONSUMER_NO == d.CUSTOMER_NO
                                select new GetCustomerArrearModel
                                {

                                    CONSUMER_NO = d.CUSTOMER_NO,
                                    LOCATION_CODE = p.LOCATION_CODE,
                                    PRN = p.PRN,
                                    LPS = p.LPS,
                                    VAT = p.VAT,
                                    TOTAL = p.TOTAL,
                                    CUSTOMER_NAME = p.CUSTOMER_NAME,
                                    ADDRESS = p.ADDRESS,
                                    BILL_CYCLE_CODE = p.BILL_CYCLE_CODE,
                                    LOCATION_DESC = p.LOCATION_DESC,
                                    NOM = p.NOM,
                                    ZONE_NAME = d.ZONE_NAME,
                                    ZONE_CODE = d.ZONE_CODE
                                }));
            return margeData;

        }
        #endregion ministry details (online)
        public async Task<List<GetCustomerArrearModel>> GetOldMinistryDetails(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string dbCode, string locationCode)
        {
            var result = new List<GetCustomerArrearModel>();
            var zoneData = new List<CustomerZoneModel>();
            var margeData = new List<GetCustomerArrearModel>();
            if (dbCode != "0")
            {
                var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(dbCode, ministryCode);

                var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(dbCode, ministryCode);

                if (custIdDataList.Count() != 0)
                {
                    result.AddRange(await GetAllCustomerDataAsync(dbCode, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                }
                if (zoneWithConsumer.Count() != 0)
                {
                    zoneData.AddRange(zoneWithConsumer);
                }
            }
            else
            {
                var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
                foreach (var database in allDatabases)
                {
                    var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, ministryCode);
                    var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(database.CODE, ministryCode);

                    if (custIdDataList.Count() != 0)
                    {
                        result.AddRange(await GetAllCustomerDataAsync(database.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                    }
                    if (zoneWithConsumer.Count() != 0)
                    {
                        zoneData.AddRange(zoneWithConsumer);
                    }
                }

            }
            margeData.AddRange((from p in result

                                from d in zoneData
                                where p.CONSUMER_NO == d.CUSTOMER_NO
                                select new GetCustomerArrearModel
                                {

                                    CONSUMER_NO = d.CUSTOMER_NO,
                                    LOCATION_CODE = p.LOCATION_CODE,
                                    PRN = p.PRN,
                                    LPS = p.LPS,
                                    VAT = p.VAT,
                                    TOTAL = p.TOTAL,
                                    CUSTOMER_NAME = p.CUSTOMER_NAME,
                                    ADDRESS = p.ADDRESS,
                                    BILL_CYCLE_CODE = p.BILL_CYCLE_CODE,
                                    LOCATION_DESC = p.LOCATION_DESC,
                                    NOM = p.NOM,
                                    ZONE_NAME = d.ZONE_NAME,
                                    ZONE_CODE = d.ZONE_CODE
                                }));
            return margeData;

        }

        #region ministry details zone wise(online)
        public async Task<List<GetCustomerArrearModel>> GetOnlineMinistryZoneWiseDetails(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string zoneCode, string locationCode, bool isRebate)
        {
            var result = new List<GetCustomerArrearModel>();
            var zoneData = new List<CustomerZoneModel>();
            var margeData = new List<GetCustomerArrearModel>();
            if (zoneCode != "0")
            {
                var custIdDataList = await GetAllCustIdByzoneCodeAndMinistryCode(zoneCode, ministryCode, isRebate);
                var zoneWithConsumer = await GetCusttomerByZoneCodeAndMinistryCode(zoneCode, ministryCode);
                var dbCodes = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);
                foreach (var item in dbCodes)
                {
                    if (custIdDataList.Count() != 0)
                    {
                        result.AddRange(await GetAllCustomerDataAsync(item.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                    }

                }
                if (zoneWithConsumer.Count() != 0)
                {
                    zoneData.AddRange(zoneWithConsumer);
                }

            }
            else
            {

                var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
                foreach (var database in allDatabases)
                {
                    try
                    {
                        var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, ministryCode, isRebate);
                        var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(database.CODE, ministryCode);

                        if (custIdDataList.Count() != 0)
                        {
                            result.AddRange(await GetAllCustomerDataAsync(database.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                        }
                        if (zoneWithConsumer.Count() != 0)
                        {
                            zoneData.AddRange(zoneWithConsumer);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }



            }
            margeData.AddRange((from p in result

                                from d in zoneData
                                where p.CONSUMER_NO == d.CUSTOMER_NO
                                select new GetCustomerArrearModel
                                {

                                    CONSUMER_NO = d.CUSTOMER_NO,
                                    LOCATION_CODE = p.LOCATION_CODE,
                                    PRN = p.PRN,
                                    LPS = p.LPS,
                                    VAT = p.VAT,
                                    TOTAL = p.TOTAL,
                                    CUSTOMER_NAME = p.CUSTOMER_NAME,
                                    ADDRESS = p.ADDRESS,
                                    BILL_CYCLE_CODE = p.BILL_CYCLE_CODE,
                                    LOCATION_DESC = p.LOCATION_DESC,
                                    NOM = p.NOM,
                                    ZONE_NAME = d.ZONE_NAME,
                                    ZONE_CODE = d.ZONE_CODE
                                }));
            return margeData;

        }
        #endregion ministry details Zone Wise (online)

        #region Common Utility Section


        private async Task<int?[]> GetAllCustomerNobyMinistryCodeandDbCode(string dbCode, string ministryCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<CustomerModel> result = new List<CustomerModel>();
            try
            {
                var sQuery = @" Select C.CUSTOMER_NO from MISCBILLAPP_GC_CUSTOMER C where C.CUSTOMER_NO > 0 AND C.DB_CODE =:dbcode AND C.MINISTRY_CODE =:ministrycode";
                var resultt = await con.QueryAsync<CustomerModel>(sQuery, new { dbcode = dbCode, ministrycode = ministryCode });
                result = resultt.ToList();
                var customerArray = result.Select(p => p.CUSTOMER_NO).ToArray();
                int?[] customers = customerArray;
                return customers;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<int?[]> GetAllCustIdByDbCodeAndMinistryCode(string dbCode, string ministryCode, bool isRebate = false)
        {
            var rebateCode = isRebate == true ? "1" : null;
            using var con = new OracleConnection(Connection.ConnectionString());
            //var isMinistryExist = ministryCode != "0" ? true : false;
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {
                //var sQuery = @" Select ci.cust_id from MISCBILLAPP_GC_CUSTOMER C 
                //                inner join miscbillapp_gc_custid ci on c.customer_no = ci.consumer_no 
                //                where  ci.cust_id > 0 and C.DB_CODE =:dbcode " + (isMinistryExist ? @" and C.MINISTRY_CODE =:ministry_code " : " ");
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_DB_CODE", value: dbCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REBATE_CODE", value: rebateCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var resultt = await con.QueryAsync<CustIdInsertModel>("MISC_REPORT_MINISTRY_PKG.CUSTID_MINISTRY_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                result = resultt.ToList();
                var custIdArray = result.Select(p => p.CUST_ID).ToArray();
                int?[] custs = custIdArray;
                return custs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<int?[]> GetAllCustIdByzoneCodeAndMinistryCode(string zoneCode, string ministryCode, bool isRebate)
        {
            var rebateCode = isRebate == true ? "1" : null;
            using var con = new OracleConnection(Connection.ConnectionString());
            //var isMinistryExist = ministryCode != "0" ? true : false;
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {

                //var sQuery = @" Select ci.cust_id from MISCBILLAPP_GC_CUSTOMER C 
                //                inner join miscbillapp_gc_custid ci on c.customer_no = ci.consumer_no 
                //                where  ci.cust_id > 0 and C.ZONE_CODE =:zoneCode " + (isMinistryExist ? @" and C.MINISTRY_CODE =:ministry_code " : " ");
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_DB_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REBATE_CODE", value: rebateCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var resultt = await con.QueryAsync<CustIdInsertModel>("MISC_REPORT_MINISTRY_PKG.CUSTID_BY_ZONE_MINI_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                result = resultt.ToList();
                var custIdArray = result.Select(p => p.CUST_ID).ToArray();
                int?[] custs = custIdArray;
                return custs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<CustomerZoneModel>> GetCusttomerWithZoneByDbCodeAndMinistryCode(string dbCode, string ministryCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            //var isMinistryExist = ministryCode != "0" ? true : false;
            try
            {
                //var sQuery = @"
                //    SELECT C.CUSTOMER_NO, C.NAME CUSTOMER_NAME, C.MINISTRY_CODE, Z.NAME ZONE_NAME, C.ZONE_CODE FROM MISCBILLAPP_GC_CUSTOMER C
                //    LEFT JOIN MISCBILLAPP_MS_ZONE Z ON C.ZONE_CODE = Z.CODE 
                //    WHERE C.DB_CODE =:dbcode " + (isMinistryExist ? @" and C.MINISTRY_CODE =:ministry_code " : " ");
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_DB_CODE", value: dbCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = await con.QueryAsync<CustomerZoneModel>("MISC_REPORT_MINISTRY_PKG.CUSTOMER_ZONE_BY_DB_MINI", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<CustomerZoneModel>> GetCusttomerByZoneCodeAndMinistryCode(string zoneCode, string ministryCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            //var isMinistryExist = ministryCode != "0" ? true : false;
            try
            {
                //var sQuery = @"
                //    SELECT C.CUSTOMER_NO, C.NAME CUSTOMER_NAME, C.MINISTRY_CODE, Z.NAME ZONE_NAME, C.ZONE_CODE, C.DB_CODE FROM MISCBILLAPP_GC_CUSTOMER C
                //    LEFT JOIN MISCBILLAPP_MS_ZONE Z ON C.ZONE_CODE = Z.CODE 
                //    WHERE C.ZONE_CODE =:zoneCode " + (isMinistryExist ? @" and C.MINISTRY_CODE =:ministry_code " : " ");
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_DB_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = await con.QueryAsync<CustomerZoneModel>("MISC_REPORT_MINISTRY_PKG.CUSTOMER_MINISTRY_CODE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GenerateChunckCondition(string query, int?[] customers)
        {
            string _buildQuery = "";
            int arraySize = customers.Length;
            int chunkSize = 500;
            int skipChunkSize = 0;

            while (skipChunkSize < arraySize)
            {
                string _customers = GetChunkCustomers(customers.Skip(skipChunkSize).Take(chunkSize).ToArray());
                skipChunkSize += chunkSize;

                if (_buildQuery.Length == 0)
                {
                    _buildQuery = query + "in (" + _customers + @")";
                }
                else
                {
                    _buildQuery += " OR " + query + "in (" + _customers + @")";
                }
            }

            if (_buildQuery.Length > 0)
                _buildQuery = " ( " + _buildQuery + " ) ";

            return _buildQuery;
        }

        public string GetChunkCustomers(int?[] customers)
        {
            string _customers = string.Join(", ", customers);

            if (_customers.Length > 0)
                _customers = "" + _customers + "";

            return _customers;
        }

        #endregion

        public async Task<List<ViewMinistryDTO>> GetMinistryList()
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<ViewMinistryDTO>("DPG_MISCAPP_MINISTRY.GET_MINISTRY_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;

        }

        public async Task<bool> SaveMinistryBill(ViewMinistryDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CODE", value: model.Code, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_HAS_DEPARTMENT", value: model.HasDepartment, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCAPP_MINISTRY.UPDATE_MINISTRY_NAME", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);
        }

        public async Task<List<MinistryLedgerDTO>> GetMinistryLedgerByDate(string currDate)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CURR_DATE", value: currDate.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistryLedgerDTO>("MISC_REPORT_MINISTRY_PKG.MINISTRY_WISE_MINISTRY_LEDGER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PoliceArrearDetailsDTO>> GetPoliceArrearReportDetails(string zoneCode, string reportDate, string? locationCode, string reportType)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PoliceArrearDetailsDTO>("MISC_REPORT_MINISTRY_PKG.POLICE_REPORT_SUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<RailwayDTO>> GetRailwayDetails(string zoneCode, string reportDate, string? locationCode, string reportType, string circleCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<RailwayDTO>("MISC_REPORT_MINISTRY_PKG.RAILWAY_REPORT_SUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PoliceArrearSummaryDTO>> GetPoliceArrearReportSummary(string zoneCode, string reportDate, string? locationCode, string reportType)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PoliceArrearSummaryDTO>("MISC_REPORT_MINISTRY_PKG.POLICE_REPORT_SUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<RailwaySummaryDTO>> GetRailwaySummary(string zoneCode, string reportDate, string locationCode, string reportType, string circleCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<RailwaySummaryDTO>("MISC_REPORT_MINISTRY_PKG.RAILWAY_REPORT_SUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PouroshovaDTO>> GetAllPouroshova()
        {

            List<PouroshovaDTO> Pouroshova = new List<PouroshovaDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Pouroshova>("DPG_GC_CUSTOMER.ALL_POUROSHOVA_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            Pouroshova = _mapper.Map(result, Pouroshova);
            return Pouroshova;

        }

        public async Task<bool> SavePouroshovaBill(PouroshovaDTO model)
        {
            try
            {

                bool isSuccess = true;
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_ZONE_CODE", value: model.ZoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CIRCLE_CODE", value: model.CircleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                if (model.Id != null)
                {
                    var result = con.Query<int>("DPG_GC_CUSTOMER.SAVE_POURASHOVA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                    isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
                }
                else
                {
                    var result = con.Query<int>("DPG_GC_CUSTOMER.SAVE_POURASHOVA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                    isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
                }

                return (isSuccess);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> DeletePouroshovaBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_GC_CUSTOMER.DELETE_POUROSHOVA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

        public async Task<List<UnionPorishodDTO>> GetAllUnionPorishod()
        {
            List<UnionPorishodDTO> Union = new List<UnionPorishodDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<UnionPorishod>("DPG_GC_CUSTOMER.ALL_UNION_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            Union = _mapper.Map(result, Union);
            return Union;
        }

        public async Task<bool> SaveUnionPorishodBill(UnionPorishodDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: model.ZoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: model.CircleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ORDER_NO", value: model.OrderNo, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            if (model.Id != null)
            {
                var result = con.Query<int>("DPG_GC_CUSTOMER.SAVE_UNIONPORISHOD", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }
            else
            {
                var result = con.Query<int>("DPG_GC_CUSTOMER.SAVE_UNIONPORISHOD", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            }

            return (isSuccess);
        }

        public async Task<int> DeleteUnionBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_GC_CUSTOMER.DELETE_UNIONPORISHOD", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

        public async Task<List<LocationWiseArrearDTO>> GetLocationWiseMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: billMonth.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationWiseArrearDTO>("MISC_REPORT_MINISTRY_PKG.LOCATION_WISE_MINISTRY_ARR", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<LocationWiseArrearDTO>> GetTotalLocationWiseMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: billMonth.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationWiseArrearDTO>("MISC_REPORT_MINISTRY_PKG.TOT_LOC_WISE_MINISTRY_ARR", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<LocationWiseArrearDTO>> GetTotalMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: billMonth.ToString(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationWiseArrearDTO>("MISC_REPORT_MINISTRY_PKG.TOTAL_MINISTRY_ARR", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<PreModDataDTO>> GetPreModDataByBillMonth(string billMonth, string deptCode)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            con.Open();
            string query = $@"SELECT NAME,CODE,DB_CODE  from MISCBILLAPP_MS_LOCATION where DEPTCODE='{deptCode}'";
            var locationResponse = con.QueryFirstOrDefault<MODLocationModel>(query);
            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(locationResponse.DB_CODE); // Getting database info by Id
            con.Close();
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
            using var conn = new OracleConnection(connectionString);

            string modQuery = $@"select USAGE_CATEGORY_CODE TariffName,LIVE_CONSUMER_MONTH_END_MNL ConsumerNo,'' VendCust,
0 VandingCustomer,TOTAL_ENERGY_SOLD_MNL SoldUnit,TOTAL_PRINCIPAL_BILLED_MNL netAmount,TOTAL_VAT_BILLED_MNL VatAmount,
TOTAL_PRINCIPAL_BILLED_MNL repayAmount,TOTAL_PRINCIPAL_BILLED_MNL energyAmount,TOTAL_PRINCIPAL_BILLED_MNL demandCharge
,TOTAL_PRINCIPAL_BILLED_MNL rebateAmount,TOTAL_PRINCIPAL_BILLED_MNL rebateAmount,TOTAL_PRINCIPAL_BILLED_MNL penaltyAmount,TOTAL_PRINCIPAL_BILLED_MNL arrearAmount,
TOTAL_PRINCIPAL_BILLED_MNL totalAmount,TOTAL_PRINCIPAL_BILLED_MNL meterRent1p,TOTAL_PRINCIPAL_BILLED_MNL meterRent3p,TOTAL_PRINCIPAL_BILLED_MNL rate
,CREATE_BY createdBy,'{locationResponse.DB_CODE}' DeptCode,BILL_CYCLE_CODE BillCycleCode,
'{locationResponse.CODE}' LocationCode,'{locationResponse.NAME}' LocationName

 from BC_PRE_PAID_MOD_INPUT WHERE LOCATION_CODE='{locationResponse.CODE}' and bill_cycle_code='{billMonth}'";
           var result = conn.Query<PreModDataDTO>(modQuery).ToList();
            return result;
        }


        #region Ministry Arrear Up to Date

        public Task<List<MinistryArrearUpToDateMergeDataDTO>> OnlineMinistyArrearUpToDateInfo(string billMonth, string zoneCode, string locationCode)
        {
            throw new NotImplementedException();
        }

        #endregion Ministry Arrear Up to Date





        #region Online Ministry Arrear Details
        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetails(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }
            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, ministryCode);
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();

            foreach (var database in allDatabase)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                try
                {
                    int?[] custIds;
                    if (zoneCode != "0")
                    {
                        custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, ministryCode);

                    }
                    else
                    {
                        custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, ministryCode);

                    }
                    if (custIds.Count() > 0)
                    {
                        using IDbConnection conn = new OracleConnection(connectionString);

                        int arraySize = custIds.Count();
                        int chunkSize = 500;
                        int skipChunkSize = 0;

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";

                            string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;

                            query = query + " in (" + _custIds + @") ";
                            string sQuery3;
                            if (database.CODE == "3")
                            {
                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           :DB_CODE DB_CODE,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                          + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL   AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            //AND(R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')
                            else
                            {

                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           :DB_CODE DB_CODE,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                           + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth, DB_CODE=database.CODE });
                            customers.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }


            if (ministryCode == "0" || ministryCode == "39")
            {


                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    try
                    {

                        var custIds = await GetNonBengaliLocationDesc(database.CODE);
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);
                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;
                                query = query + " in (" + _custIds + @") ";

                                var sQuery3 = @"SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           :DB_CODE DB_CODE,
                                           0 CURR_RECEIPT_PRINCIPAL,
                                           0 CURR_RECEIPT_VAT,
                                           0 ARREAR_PRICIPAL,0 ARREAR_LPS,0 ARREAR_VAT,
                                           
                                           nvl(A.PRINCIPAL,0)+1618012.422 CURR_PRINCIPAL,nvl(A.LPS,0) CURR_LPS,nvl(A.VAT,0) CURR_VAT,
                                           
                                           0 Total_Receipt_Arrear, 0 ARREAR_RECEIPT_PRINCIPAL,0 ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                            + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  (BILL_CYCLE_CODE BETWEEN '201604' AND :P_BILL_CYCLE_CODE) AND    ((IH.PRINCIPAL_AMT+IH.PRINCIPAL_ADJ-IH.PRINCIPAL_APPL) + (IH.LPS_AMT+IH.LPS_ADJ-IH.LPS_APPL)+
                                                    (IH.VAT_AMT+IH.VAT_ADJ-VAT_APPL)) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID";
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, DB_CODE=database.CODE });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            if (ministryCode == "0" || ministryCode == "29")
            {
                foreach (var database in allDatabase)
                {
                    using var con = new OracleConnection(Connection.ConnectionString());

                    string religiousQuery = $@"SELECT X.*,'{database.CODE}' DB_CODE,  0 CURR_RECEIPT_PRINCIPAL,0 CURR_RECEIPT_VAT ,0 ARREAR_LPS,0 CURR_PRINCIPAL,
                                        0 CURR_LPS ,0 CURR_VAT ,0 Total_Receipt_Arrear,0 ARREAR_RECEIPT_PRINCIPAL,  0 ARREAR_RECEIPT_VAT, '{billMonth}' BILL_CYCLE_CODE from 
                                        (select ci.cust_id CUST_ID, C.CUSTOMER_NO CONSUMER_NO , c.NAME CUSTOMER_NAME , c.LOCATION_CODE , L.NAME  LOCATION_DESC,
                                        C.ADDRESS ,SUM(PRINCIPAL+DEMAND_CHARGE+SERVICE_CHARGE) ARREAR_PRICIPAL,
                                        SUM(VAT_AMOUNT)  ARREAR_VAT from MISCBILLAPP_GC_CUSTOMER C ,MISCBILLAPP_GC_CUSTID CI,MISCBILLAPP_MS_LOCATION L,RELIGIOUS_ARREAR R
                                        where c.customer_no=CI.CONSUMER_NO 
                                        AND L.CODE=c.location_code 
                                        AND L.CODE=R.LOCATION_CODE
                                        AND R.CUSTOMER_NO=c.customer_no 
                                        AND R.CUSTOMER_NO=CI.CONSUMER_NO
                                        AND R.BILL_CYCLE_CODE BETWEEN '202309' AND '{billMonth}' 
                                        and c.ministry_code='29' 
                                        and c.religious_code='1' and C.DB_CODE='{database.CODE}'
                                        group by ci.CUST_ID,C.CUSTOMER_NO,C.NAME,C.LOCATION_CODE,L.NAME,C.ADDRESS) X";

                    var results = await con.QueryAsync<OnlineMinistryArearDetails>(religiousQuery);
                    customers.AddRange(results.ToList());
                }
            }
            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID && c.DB_CODE==cust.DB_CODE
                                select new OnlineMinistryArrearDetailsMergeDTO
                                {
                                    CustId = c.CUST_ID,
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationDesc = c.LOCATION_DESC,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    ZoneNameBn = cust.ZONE_NAME_BN,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    MinistryName = cust.MINISTRY_NAME,
                                    MinistryNameBn = cust.MINISTRY_NAME_BN,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,
                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                    HasDepartment = cust.HAS_DEPARTMENT
                                }));

            return mergeList;
        }
        #endregion Online Ministry Arrear Details

        #region Online Ministry Arrear Details
        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetailsWithCRV(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }
            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, ministryCode);
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();

            foreach (var database in allDatabase)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                try
                {
                    int?[] custIds;
                    if (zoneCode != "0")
                    {
                        custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, ministryCode);

                    }
                    else
                    {
                        custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, ministryCode);

                    }
                    if (custIds.Count() > 0)
                    {
                        using IDbConnection conn = new OracleConnection(connectionString);

                        int arraySize = custIds.Count();
                        int chunkSize = 500;
                        int skipChunkSize = 0;

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";

                            string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;

                            query = query + " in (" + _custIds + @") ";
                            string sQuery3;
                            if (database.CODE == "3")
                            {
                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                          + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')  AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            // AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')
                            else
                            {

                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE, 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                           + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            

                            var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                            customers.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }


            if (ministryCode == "0" || ministryCode == "39")
            {


                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    try
                    {

                        var custIds = await GetNonBengaliLocationDesc(database.CODE);
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);
                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;
                                query = query + " in (" + _custIds + @") ";

                                var sQuery3 = @"SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           0 CURR_RECEIPT_PRINCIPAL,
                                           0 CURR_RECEIPT_VAT,
                                           0 ARREAR_PRICIPAL,0 ARREAR_LPS,0 ARREAR_VAT,
                                           
                                           nvl(A.PRINCIPAL,0)+1618012.422 CURR_PRINCIPAL,nvl(A.LPS,0) CURR_LPS,nvl(A.VAT,0) CURR_VAT,
                                           
                                           0 Total_Receipt_Arrear, 0 ARREAR_RECEIPT_PRINCIPAL,0 ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                            + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  (BILL_CYCLE_CODE BETWEEN '201604' AND :P_BILL_CYCLE_CODE) AND    ((IH.PRINCIPAL_AMT+IH.PRINCIPAL_ADJ-IH.PRINCIPAL_APPL) + (IH.LPS_AMT+IH.LPS_ADJ-IH.LPS_APPL)+
                                                    (IH.VAT_AMT+IH.VAT_ADJ-VAT_APPL)) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID";
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            if (ministryCode == "0" || ministryCode == "29")
            {
                using var con = new OracleConnection(Connection.ConnectionString());

                string religiousQuery = $@"SELECT X.*,  0 CURR_RECEIPT_PRINCIPAL,0 CURR_RECEIPT_VAT ,0 ARREAR_LPS,0 CURR_PRINCIPAL,
                                        0 CURR_LPS ,0 CURR_VAT ,0 Total_Receipt_Arrear,0 ARREAR_RECEIPT_PRINCIPAL,  0 ARREAR_RECEIPT_VAT, '{billMonth}' BILL_CYCLE_CODE from 
                                        (select ci.cust_id CUST_ID, C.CUSTOMER_NO CONSUMER_NO , c.NAME CUSTOMER_NAME , c.LOCATION_CODE , L.NAME  LOCATION_DESC,
                                        C.ADDRESS ,SUM(PRINCIPAL+DEMAND_CHARGE+SERVICE_CHARGE) ARREAR_PRICIPAL,
                                        SUM(VAT_AMOUNT)  ARREAR_VAT from MISCBILLAPP_GC_CUSTOMER C 
                                        inner join MISCBILLAPP_GC_CUSTID CI on c.customer_no=CI.CONSUMER_NO 
                                        inner join MISCBILLAPP_MS_LOCATION L on L.CODE=c.location_code 
                                        inner join RELIGIOUS_ARREAR R on R.CUSTOMER_NO=c.customer_no 
                                        where R.BILL_CYCLE_CODE BETWEEN '202307' AND '{billMonth}' and c.ministry_code='29' 
                                        and c.religious_code='1' and ('{zoneCode}'='0' OR C.zone_code='{zoneCode}')
                                        group by ci.CUST_ID,C.CUSTOMER_NO,C.NAME,C.LOCATION_CODE,L.NAME,C.ADDRESS) X";

                var results = await con.QueryAsync<OnlineMinistryArearDetails>(religiousQuery);
                customers.AddRange(results.ToList());
            }
            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineMinistryArrearDetailsMergeDTO
                                {
                                    CustId = c.CUST_ID,
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationDesc = c.LOCATION_DESC,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    ZoneNameBn = cust.ZONE_NAME_BN,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    MinistryName = cust.MINISTRY_NAME,
                                    MinistryNameBn = cust.MINISTRY_NAME_BN,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,
                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                    HasDepartment = cust.HAS_DEPARTMENT
                                }));

            return mergeList;
        }
        #endregion Online Ministry Arrear Details


        private async Task<List<MinistryArrearDetailsCustId>> GetMinistryArrearDetailsCustId(string zoneCode, string locationCode)
        {
            //var isMinistry = ministryCode == "0" ? false : true;
            var isZone = zoneCode == "0" ? false : true;
            var isLocation = locationCode == "0" ? false : true;

            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT
                                CD.CUST_ID CUST_ID,
                                C.ZONE_CODE ZONE_CODE,
                                C.MINISTRY_CODE MINISTRY_CODE,
                                (SELECT M.NAME FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME,
                                (SELECT Z.NAME FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME
                           FROM MISCBILLAPP_GC_CUSTOMER C
                           INNER JOIN MISCBILLAPP_GC_CUSTID CD ON C.CUSTOMER_NO = CD.CONSUMER_NO
                           WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
                           (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ");
            var result = await con.QueryAsync<MinistryArrearDetailsCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode });
            return result.ToList();
        }

        private async Task<int[]> GetNonCustLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<MinistryArrearDetailsCustId> list = new List<MinistryArrearDetailsCustId>();
            var sQuery = @"SELECT 
                            CID.CUST_ID
                            FROM MISCBILLAPP_GC_CUSTOMER C
                            INNER JOIN MISCBILLAPP_GC_CUSTID CID ON C.CUSTOMER_NO = CID.CONSUMER_NO
                            WHERE C.MINISTRY_CODE IS NOT NULL AND C.DB_CODE = :dbCode";
            var result = await con.QueryAsync<MinistryArrearDetailsCustId>(sQuery, new { dbCode });
            list = result.ToList();
            var custIdArray = list.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;
        }

        public string GetChunkCustomers(int[] customers)
        {
            string _customers = string.Join(",", customers);
            if (_customers.Length > 0)
            {
                _customers = "" + _customers + "";
            }
            return _customers;
        }


        private async Task<List<MinistryArrearDetailsCustId>> GetMinistryArrearDetailsCustIdWithMinistry(string zoneCode, string locationCode, string ministryCode)
        {
            var isMinistry = ministryCode == "0" ? false : true;
            var isZone = zoneCode == "0" ? false : true;
            var isLocation = locationCode == "0" ? false : true;

            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT
                                CD.CUST_ID CUST_ID,
                                C.ZONE_CODE ZONE_CODE,
                                C.MINISTRY_CODE MINISTRY_CODE,
                                C.CUSTOMER_NO ,
                                C.DB_CODE ,
                                (SELECT M.NAME FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME,
                                (SELECT M.NAMEBN FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME_BN,
                                (SELECT M.ORDERNO FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) ORDERNO,
                                (SELECT D.NAMEBN FROM GC_DEPARTMENT D WHERE C.DEPARTMENT_CODE = D.CODE) DEPT_NAME,
                                (SELECT M.HAS_DEPARTMENT FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) HAS_DEPARTMENT,
                                (SELECT Z.NAME FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME,
                                (SELECT Z.NAMEBN FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME_BN
                           FROM MISCBILLAPP_GC_CUSTOMER C
                           INNER JOIN MISCBILLAPP_GC_CUSTID CD ON C.CUSTOMER_NO = CD.CONSUMER_NO
                           WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
                           (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (isMinistry ? @" AND C.MINISTRY_CODE = :ministry_code" : @" ") + @" ORDER BY ORDERNO ASC";
            var result = await con.QueryAsync<MinistryArrearDetailsCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode, ministry_code = ministryCode });
            return result.ToList();
        }

        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearByBillMonth(string zoneCode, string locationCode, string billMonth)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;

            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }


            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, "0");
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();
            try
            {
                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                    try
                    {
                        //var custIds = await GetNonCustLocationDesc(database.CODE);

                        int?[] custIds;
                        if (zoneCode != "0")
                        {
                            custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, "0");

                        }
                        else
                        {
                            custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, "0");

                        }
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);

                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;

                                query = query + " in (" + _custIds + @") ";
                                string sQuery3;
                                if (database.CODE == "3")
                                {
                                    sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           :DB_CODE DB_CODE,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                               + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,
                                                                SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                                }
                                //AND(R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')
                                else
                                {
                                    sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           :DB_CODE DB_CODE,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                               + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,
                                               SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT
                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                                }
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth,DB_CODE=database.CODE });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    try
                    {

                        var custIds = await GetNonBengaliLocationDesc(database.CODE);
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);
                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;
                                query = query + " in (" + _custIds + @") ";

                                var sQuery3 = @"SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :DB_CODE DB_CODE,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           0 CURR_RECEIPT_PRINCIPAL,
                                           0 CURR_RECEIPT_VAT,
                                           0 ARREAR_PRICIPAL,0 ARREAR_LPS,0 ARREAR_VAT,
                                           
                                           nvl(A.PRINCIPAL,0)+1618012.422 CURR_PRINCIPAL,nvl(A.LPS,0) CURR_LPS,nvl(A.VAT,0) CURR_VAT,
                                           
                                           0 Total_Receipt_Arrear, 0 ARREAR_RECEIPT_PRINCIPAL,0 ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                            + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  (BILL_CYCLE_CODE BETWEEN '201604' AND :P_BILL_CYCLE_CODE) AND    ((IH.PRINCIPAL_AMT+IH.PRINCIPAL_ADJ-IH.PRINCIPAL_APPL) + (IH.LPS_AMT+IH.LPS_ADJ-IH.LPS_APPL)+
                                                    (IH.VAT_AMT+IH.VAT_ADJ-VAT_APPL)) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID";
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, DB_CODE=database.CODE });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                foreach (var database in allDatabase)
                {
                    using var con = new OracleConnection(Connection.ConnectionString());

                    string religiousQuery = $@"SELECT X.*,'{database.CODE}' DB_CODE,  0 CURR_RECEIPT_PRINCIPAL,0 CURR_RECEIPT_VAT ,0 ARREAR_LPS,0 CURR_PRINCIPAL,
                                        0 CURR_LPS ,0 CURR_VAT ,0 Total_Receipt_Arrear,0 ARREAR_RECEIPT_PRINCIPAL,  0 ARREAR_RECEIPT_VAT, '{billMonth}' BILL_CYCLE_CODE from 
                                        (select ci.cust_id CUST_ID, C.CUSTOMER_NO CONSUMER_NO , c.NAME CUSTOMER_NAME , c.LOCATION_CODE , L.NAME  LOCATION_DESC,
                                        C.ADDRESS ,SUM(PRINCIPAL+DEMAND_CHARGE+SERVICE_CHARGE) ARREAR_PRICIPAL,
                                        SUM(VAT_AMOUNT)  ARREAR_VAT from MISCBILLAPP_GC_CUSTOMER C ,MISCBILLAPP_GC_CUSTID CI,MISCBILLAPP_MS_LOCATION L,RELIGIOUS_ARREAR R
                                        where c.customer_no=CI.CONSUMER_NO 
                                        AND L.CODE=c.location_code 
                                        AND L.CODE=R.LOCATION_CODE
                                        AND R.CUSTOMER_NO=c.customer_no 
                                        AND R.CUSTOMER_NO=CI.CONSUMER_NO
                                        AND R.BILL_CYCLE_CODE BETWEEN '202309' AND '{billMonth}' 
                                        and c.ministry_code='29' 
                                        and c.religious_code='1' and C.DB_CODE='{database.CODE}'
                                        group by ci.CUST_ID,C.CUSTOMER_NO,C.NAME,C.LOCATION_CODE,L.NAME,C.ADDRESS) X";


                    var results = await con.QueryAsync<OnlineMinistryArearDetails>(religiousQuery);
                    customers.AddRange(results.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID && c.DB_CODE== cust.DB_CODE
                                orderby cust.ORDERNO ascending
                                select new OnlineMinistryArrearDetailsMergeDTO
                                {
                                    CustId = c.CUST_ID,
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationDesc = c.LOCATION_DESC,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    ZoneNameBn = cust.ZONE_NAME_BN,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    MinistryName = cust.MINISTRY_NAME,
                                    MinistryNameBn = cust.MINISTRY_NAME_BN,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,
                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                    HasDepartment = cust.HAS_DEPARTMENT,
                                    DeptName = cust.DEPT_NAME,
                                    ORDERNO = cust.ORDERNO
                                }));

            return mergeList;
        }

        public async Task<List<MinistryDetailsSummaryMergeDTO>> GetOnlineMinistryZoneWiseSummary(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string zoneCode, string locationCode, bool isRebate)
        {
            var result = new List<GetCustomerArrearModel>();
            var zoneData = new List<CustomerZoneModel>();
            var margeData = new List<MinistryDetailsSummaryMergeDTO>();
            if (zoneCode != "0")
            {
                var custIdDataList = await GetAllCustIdByzoneCodeAndMinistryCode(zoneCode, ministryCode, isRebate);
                var zoneWithConsumer = await GetCusttomerByZoneCodeAndMinistryCode(zoneCode, ministryCode);
                var dbCodes = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);
                foreach (var item in dbCodes)
                {
                    if (custIdDataList.Count() != 0)
                    {
                        result.AddRange(await GetAllCustomerSummaryDataAsync(item.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                    }

                }
                if (zoneWithConsumer.Count() != 0)
                {
                    zoneData.AddRange(zoneWithConsumer);
                }

            }
            else
            {

                var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
                foreach (var database in allDatabases)
                {
                    try
                    {
                        var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, ministryCode, isRebate);
                        var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(database.CODE, ministryCode);

                        if (custIdDataList.Count() != 0)
                        {
                            result.AddRange(await GetAllCustomerSummaryDataAsync(database.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                        }
                        if (zoneWithConsumer.Count() != 0)
                        {
                            zoneData.AddRange(zoneWithConsumer);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }



            }
            margeData.AddRange((from p in result

                                from d in zoneData
                                where p.CONSUMER_NO == d.CUSTOMER_NO
                                select new MinistryDetailsSummaryMergeDTO
                                {

                                    ConsumerNo = d.CUSTOMER_NO,
                                    Prn = p.PRN,
                                    Lps = p.LPS,
                                    Vat = p.VAT,
                                    Total = p.TOTAL,
                                    ZoneName = d.ZONE_NAME,
                                    ZoneNameBn = d.ZONE_NAME_BN,
                                    MinistryCode = d.MINISTRY_CODE,
                                    MinistryNameBn = d.MINISTRY_NAME_BN,
                                    ZoneCode = d.ZONE_CODE,
                                    TotalCustomer = p.TOTAL_CUSTOMER,
                                    TotalMinistryArrear = p.TOTAL_MINISTRY_ARREAR
                                }));
            return margeData;
        }


        private async Task<List<MinistryArrearDetailsCustId>> GetMinistryCustIdWithoutreligiousandnonMinistry(string zoneCode, string locationCode, string ministryCode)
        {
            var isMinistry = ministryCode == "0" ? false : true;
            var isZone = zoneCode == "0" ? false : true;
            var isLocation = locationCode == "0" ? false : true;

            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT
                                CD.CUST_ID CUST_ID,
                                C.ZONE_CODE ZONE_CODE,
                                C.MINISTRY_CODE MINISTRY_CODE,
                                (SELECT M.NAME FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME,
                                (SELECT M.NAMEBN FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME_BN,
                                (SELECT M.ORDERNO FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) ORDERNO,
                                (SELECT D.NAMEBN FROM GC_DEPARTMENT D WHERE C.DEPARTMENT_CODE = D.CODE) DEPT_NAME,
                                (SELECT M.HAS_DEPARTMENT FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) HAS_DEPARTMENT,
                                (SELECT Z.NAME FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME,
                                (SELECT Z.NAMEBN FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME_BN
                           FROM MISCBILLAPP_GC_CUSTOMER C
                           INNER JOIN MISCBILLAPP_GC_CUSTID CD ON C.CUSTOMER_NO = CD.CONSUMER_NO
                           WHERE C.MINISTRY_CODE IS NOT NULL and C.MINISTRY_CODE NOT IN ('29',39) " + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
                           (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (isMinistry ? @" AND C.MINISTRY_CODE = :ministry_code" : @" ") + @" ORDER BY ORDERNO ASC";
            var result = await con.QueryAsync<MinistryArrearDetailsCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode, ministry_code = ministryCode });
            return result.ToList();
        }

        //private async Task<int[]> GetcustIdWithoutreligiousandnonMinistry(string dbCode)
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    List<MinistryArrearDetailsCustId> list = new List<MinistryArrearDetailsCustId>();
        //    var sQuery = @"SELECT 
        //                    CID.CUST_ID
        //                    FROM MISCBILLAPP_GC_CUSTOMER C
        //                    INNER JOIN MISCBILLAPP_GC_CUSTID CID ON C.CUSTOMER_NO = CID.CONSUMER_NO
        //                    WHERE C.MINISTRY_CODE IS NOT NULL AND C.DB_CODE = :dbCode";
        //    var result = await con.QueryAsync<MinistryArrearDetailsCustId>(sQuery, new { dbCode });
        //    list = result.ToList();
        //    var custIdArray = list.Select(p => p.CUST_ID).ToArray();
        //    int[] custs = custIdArray;
        //    return custs;
        //}

        private async Task<int?[]> GetAllCustIdWithoutreligiousandnonMinistry(string dbCode, string ministryCode, bool isRebate = false)
        {
            var rebateCode = isRebate == true ? "1" : null;
            using var con = new OracleConnection(Connection.ConnectionString());
            var isMinistryExist = ministryCode != "0" ? true : false;
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {
                //var sQuery = $@" Select ci.cust_id from MISCBILLAPP_GC_CUSTOMER C 
                //                inner join miscbillapp_gc_custid ci on c.customer_no = ci.consumer_no 
                //                where  ci.cust_id > 0 and C.DB_CODE ='{dbCode}' " + (isMinistryExist ? $@" and C.MINISTRY_CODE ='{ministryCode}' " : " ");
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_DB_CODE", value: dbCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REBATE_CODE", value: rebateCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var resultt = await con.QueryAsync<CustIdInsertModel>("MISC_REPORT_MINISTRY_PKG.CUSTID_WITHOUT_RELIGIOUS", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                //var resultt = await con.QueryAsync<CustIdInsertModel>(sQuery);
                result = resultt.ToList();
                var custIdArray = result.Select(p => p.CUST_ID).ToArray();
                int?[] custs = custIdArray;
                return custs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetailsByCode(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }
            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, ministryCode);
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();
            foreach (var database in allDatabase)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                try
                {
                    //int?[] custIds;
                    //if (zoneCode != "0")
                    //{
                    //    custIds = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, ministryCode);

                    //}
                    //else
                    //{
                    //    custIds = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, ministryCode);

                    //}
                    var custIds = await GetNonCustLocationDesc(database.CODE);
                    if (custIds.Count() > 0)
                    {
                        using IDbConnection conn = new OracleConnection(connectionString);

                        int arraySize = custIds.Count();
                        int chunkSize = 500;
                        int skipChunkSize = 0;

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";

                            string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;

                            query = query + " in (" + _custIds + @") ";
                            string sQuery3;
                            if (database.CODE == "3")
                            {
                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                          + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')  AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            else
                            {

                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                           + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                            customers.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }

            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineMinistryArrearDetailsMergeDTO
                                {
                                    CustId = c.CUST_ID,
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationDesc = c.LOCATION_DESC,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    ZoneNameBn = cust.ZONE_NAME_BN,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    MinistryName = cust.MINISTRY_NAME,
                                    MinistryNameBn = cust.MINISTRY_NAME_BN,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,
                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                    HasDepartment = cust.HAS_DEPARTMENT
                                }));

            return mergeList;
        }

        private async Task<int[]> GetNonBengaliLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<NonBengaliCustId> result = new List<NonBengaliCustId>();
            var sQuery = @" select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.NON_BENGALI_CAMP_CODE is not null and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<NonBengaliCustId>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;

        }

        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearDetailsWithCRV(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }
            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, ministryCode);
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();
            foreach (var database in allDatabase)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                try
                {
                    var custIds = await GetNonCustLocationDesc(database.CODE);
                    if (custIds.Count() > 0)
                    {
                        using IDbConnection conn = new OracleConnection(connectionString);

                        int arraySize = custIds.Count();
                        int chunkSize = 500;
                        int skipChunkSize = 0;

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";

                            string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;

                            query = query + " in (" + _custIds + @") ";
                            string sQuery3;
                            
                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                           + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                            customers.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }
            mergeList.AddRange((from c in customers

                               from cust in ministryCustId
                               where c.CUST_ID == cust.CUST_ID
                               select new OnlineMinistryArrearDetailsMergeDTO
                               {
                                   CustId = c.CUST_ID,
                                   ConsumerNo = c.CONSUMER_NO,
                                   CustomerName = c.CUSTOMER_NAME,
                                   LocationCode = c.LOCATION_CODE,
                                   LocationDesc = c.LOCATION_DESC,
                                   ZoneCode = cust.ZONE_CODE,
                                   ZoneName = cust.ZONE_NAME,
                                   ZoneNameBn = cust.ZONE_NAME_BN,
                                   MinistryCode = cust.MINISTRY_CODE,
                                   MinistryName = cust.MINISTRY_NAME,
                                   MinistryNameBn = cust.MINISTRY_NAME_BN,
                                   CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                   CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                   CurrLps = c.CURR_LPS,
                                   CurrPrincipal = c.CURR_PRINCIPAL,
                                   CurrVat = c.CURR_VAT,
                                   ArrearLps = c.ARREAR_LPS,
                                   ArrearPrincipal = c.ARREAR_PRICIPAL,
                                   ArrearVat = c.ARREAR_VAT,
                                   TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                   ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                   ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                   HasDepartment = cust.HAS_DEPARTMENT
                               }));

            return mergeList;
        }

        public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearSummaryWithCRV(string zoneCode, string locationCode, string billMonth)
        {
            List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;

            if (zoneCode != "0")
            {
                allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

            }
            else
            {
                allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

            }


            List<OnlineMinistryArearDetails> customers = new List<OnlineMinistryArearDetails>();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry(zoneCode, locationCode, "0");
            var mergeList = new List<OnlineMinistryArrearDetailsMergeDTO>();
            try
            {
                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                    try
                    {
                        //var custIds = await GetNonCustLocationDesc(database.CODE);

                        int?[] custIds;
                        if (zoneCode != "0")
                        {
                            custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, "0");

                        }
                        else
                        {
                            custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, "0");

                        }
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);

                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;

                                query = query + " in (" + _custIds + @") ";
                                string sQuery3;
                                if (database.CODE == "3")
                                {
                                    sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                         + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL  AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN') AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                                }
                                else
                                {
                                    sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
                                           nvl(B.LPS,0) CURR_LPS,
                                           nvl(B.VAT,0) CURR_VAT,
                                           D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                          + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                      LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV'  AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                                }
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

                foreach (var database in allDatabase)
                {
                    var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    try
                    {

                        var custIds = await GetNonBengaliLocationDesc(database.CODE);
                        if (custIds.Count() > 0)
                        {
                            using IDbConnection conn = new OracleConnection(connectionString);
                            int arraySize = custIds.Count();
                            int chunkSize = 500;
                            int skipChunkSize = 0;

                            while (skipChunkSize < arraySize)
                            {
                                string query = " IH.CUST_ID ";
                                string Bccc = " BCC.CUST_ID ";

                                string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                                skipChunkSize += chunkSize;
                                query = query + " in (" + _custIds + @") ";

                                var sQuery3 = @"SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           0 CURR_RECEIPT_PRINCIPAL,
                                           0 CURR_RECEIPT_VAT,
                                           0 ARREAR_PRICIPAL,0 ARREAR_LPS,0 ARREAR_VAT,
                                           
                                           nvl(A.PRINCIPAL,0)+1618012.422 CURR_PRINCIPAL,nvl(A.LPS,0) CURR_LPS,nvl(A.VAT,0) CURR_VAT,
                                           
                                           0 Total_Receipt_Arrear, 0 ARREAR_RECEIPT_PRINCIPAL,0 ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
                                               from EBC.BC_CUSTOMERS IH  
                                                
                                               INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
                                               INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
                                                        WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
                                                            + @"
                                               GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
                                                BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
                                            ) X

                                        LEFT JOIN
                                             (

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE  (BILL_CYCLE_CODE BETWEEN '201604' AND :P_BILL_CYCLE_CODE) AND    ((IH.PRINCIPAL_AMT+IH.PRINCIPAL_ADJ-IH.PRINCIPAL_APPL) + (IH.LPS_AMT+IH.LPS_ADJ-IH.LPS_APPL)+
                                                    (IH.VAT_AMT+IH.VAT_ADJ-VAT_APPL)) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID";
                                var result = await conn.QueryAsync<OnlineMinistryArearDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                using var con = new OracleConnection(Connection.ConnectionString());

                string religiousQuery = $@"SELECT X.*,  0 CURR_RECEIPT_PRINCIPAL,0 CURR_RECEIPT_VAT ,0 ARREAR_LPS,0 CURR_PRINCIPAL,
                                        0 CURR_LPS ,0 CURR_VAT ,0 Total_Receipt_Arrear,0 ARREAR_RECEIPT_PRINCIPAL,  0 ARREAR_RECEIPT_VAT, '{billMonth}' BILL_CYCLE_CODE from 
                                        (select ci.cust_id CUST_ID, C.CUSTOMER_NO CONSUMER_NO , c.NAME CUSTOMER_NAME , c.LOCATION_CODE , L.NAME  LOCATION_DESC,
                                        C.ADDRESS ,SUM(PRINCIPAL+DEMAND_CHARGE+SERVICE_CHARGE) ARREAR_PRICIPAL,
                                        SUM(VAT_AMOUNT)  ARREAR_VAT from MISCBILLAPP_GC_CUSTOMER C 
                                        inner join MISCBILLAPP_GC_CUSTID CI on c.customer_no=CI.CONSUMER_NO 
                                        inner join MISCBILLAPP_MS_LOCATION L on L.CODE=c.location_code 
                                        inner join RELIGIOUS_ARREAR R on R.CUSTOMER_NO=c.customer_no 
                                        where R.BILL_CYCLE_CODE BETWEEN '202307' AND '{billMonth}' and c.ministry_code='29' 
                                        and c.religious_code='1' and ('{zoneCode}'='0' OR C.zone_code='{zoneCode}')
                                        group by ci.CUST_ID,C.CUSTOMER_NO,C.NAME,C.LOCATION_CODE,L.NAME,C.ADDRESS) X";

                var results = await con.QueryAsync<OnlineMinistryArearDetails>(religiousQuery);
                customers.AddRange(results.ToList());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID
                                orderby cust.ORDERNO ascending
                                select new OnlineMinistryArrearDetailsMergeDTO
                                {
                                    CustId = c.CUST_ID,
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationDesc = c.LOCATION_DESC,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    ZoneNameBn = cust.ZONE_NAME_BN,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    MinistryName = cust.MINISTRY_NAME,
                                    MinistryNameBn = cust.MINISTRY_NAME_BN,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,
                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
                                    HasDepartment = cust.HAS_DEPARTMENT,
                                    DeptName = cust.DEPT_NAME,
                                    ORDERNO = cust.ORDERNO
                                }));

            return mergeList;
        }


        //private async Task<List<PublicSecurityDivisionCustId>> GetSecurityserviceDivisionCustId(string zoneCode, string locationCode)
        //{

        //    var isZone = zoneCode == "0" ? false : true;
        //    var isLocation = locationCode == "0" ? false : true;

        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    var sQuery = @"SELECT
        //                        CD.CUST_ID CUST_ID,
        //                        C.ZONE_CODE ZONE_CODE,
        //                        C.MINISTRY_CODE MINISTRY_CODE,
        //                        (SELECT M.NAME FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME,
        //                        (SELECT M.NAMEBN FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME_BN,
        //                        (SELECT M.ORDERNO FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) ORDERNO,
        //                        (SELECT D.NAMEBN FROM GC_DEPARTMENT D WHERE C.DEPARTMENT_CODE = D.CODE) DEPT_NAME,
        //                        (SELECT M.HAS_DEPARTMENT FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) HAS_DEPARTMENT,
        //                        (SELECT Z.NAME FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME,
        //                        (SELECT Z.NAMEBN FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME_BN
        //                   FROM MISCBILLAPP_GC_CUSTOMER C
        //                   INNER JOIN MISCBILLAPP_GC_CUSTID CD ON C.CUSTOMER_NO = CD.CONSUMER_NO
        //                   WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
        //                   (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (@" AND C.MINISTRY_CODE = '17' " ) + @" ORDER BY ORDERNO ASC";
        //    var result = await con.QueryAsync<PublicSecurityDivisionCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode });
        //    return result.ToList();

        //}
        //private async Task<int[]> GetSecurityserviceDivisionCustIdlocwise(string zoneCode, string locationCode)
        //{

        //    var isZone = zoneCode == "0" ? false : true;
        //    var isLocation = locationCode == "0" ? false : true;

        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    var sQuery = @"SELECT
        //                        CD.CUST_ID CUST_ID,
        //                        C.ZONE_CODE ZONE_CODE,
        //                        C.MINISTRY_CODE MINISTRY_CODE,
        //                        (SELECT M.NAME FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME,
        //                        (SELECT M.NAMEBN FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) MINISTRY_NAME_BN,
        //                        (SELECT M.ORDERNO FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) ORDERNO,
        //                        (SELECT D.NAMEBN FROM GC_DEPARTMENT D WHERE C.DEPARTMENT_CODE = D.CODE) DEPT_NAME,
        //                        (SELECT M.HAS_DEPARTMENT FROM MISCBILLAPP_GC_MINISTRY M WHERE M.CODE = C.MINISTRY_CODE) HAS_DEPARTMENT,
        //                        (SELECT Z.NAME FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME,
        //                        (SELECT Z.NAMEBN FROM MISCBILLAPP_MS_ZONE Z WHERE Z.CODE = C.ZONE_CODE) ZONE_NAME_BN
        //                   FROM MISCBILLAPP_GC_CUSTOMER C
        //                   INNER JOIN MISCBILLAPP_GC_CUSTID CD ON C.CUSTOMER_NO = CD.CONSUMER_NO
        //                   WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
        //                   (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (@" AND C.MINISTRY_CODE = '17' ") + @" ORDER BY ORDERNO ASC";
        //    var result = await con.QueryAsync<PublicSecurityDivisionCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode });
        //    //return result.ToList();
        //    var custIdArray = result.Select(p => p.CUST_ID).ToArray();
        //    int[] custs = custIdArray;
        //    return custs;
        //}

        //public async Task<List<PublicSecurityDivisionMergeDTO>> OnlineSecurityServiceDivArrearDetails(string billMonth, string zoneCode, string locationCode, string reportType)
        //{
        //    List<Core.Domain.DatabaseConfig.DatabaseConfig> allDatabase;
        //    if (zoneCode != "0")
        //    {
        //        allDatabase = await _dbConfigRepo.GetDatabaseByZoneCode(zoneCode);

        //    }
        //    else
        //    {
        //        allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();

        //    }
        //    List<PublicSecurityDivisionDetails> customers = new List<PublicSecurityDivisionDetails>();
        //    var ministryCustId = await GetSecurityserviceDivisionCustId(zoneCode, locationCode);
        //    var mergeList = new List<PublicSecurityDivisionMergeDTO>();

        //    foreach (var database in allDatabase)
        //    {
        //        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
        //        string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
        //        try
        //        {
        //            int[] custIds;
        //            if (zoneCode != "0")
        //            {
        //                custIds = await GetSecurityserviceDivisionCustIdlocwise(zoneCode, locationCode);

        //            }
        //            else
        //            {
        //                custIds = await GetSecurityserviceDivisionCustIdlocwise(zoneCode, locationCode);

        //            }
        //            if (custIds.Count() > 0)
        //            {
        //                using IDbConnection conn = new OracleConnection(connectionString);

        //                int arraySize = custIds.Count();
        //                int chunkSize = 500;
        //                int skipChunkSize = 0;

        //                while (skipChunkSize < arraySize)
        //                {
        //                    string query = " IH.CUST_ID ";
        //                    string Bccc = " BCC.CUST_ID ";

        //                    string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
        //                    skipChunkSize += chunkSize;

        //                    query = query + " in (" + _custIds + @") ";
        //                    string sQuery3;
        //                    if (database.CODE == "3")
        //                    {
        //                        sQuery3 = @"
        //                             Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
        //                                   X.LOCATION_CODE , 
        //                                   X.LOCATION_DESC,
        //                                   :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
        //                                   X.ADDRESS,
        //                                    NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
        //                                   C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
        //                                   C.VAT_AMT CURR_RECEIPT_VAT,
        //                                   nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
        //                                   nvl(A.LPS,0) ARREAR_LPS,
        //                                   nvl(A.VAT,0) ARREAR_VAT,
        //                                   nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
        //                                   nvl(B.LPS,0) CURR_LPS,
        //                                   nvl(B.VAT,0) CURR_VAT,
        //                                   D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
        //                                   D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
        //                                   D.VAT_AMT ARREAR_RECEIPT_VAT
        //                                FROM 
        //                                    (
        //                                       SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
        //                                              :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
        //                                       from EBC.BC_CUSTOMERS IH  

        //                                       INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
        //                                       INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
        //                                                WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
        //                                                  + @"
        //                                       GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
        //                                        BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
        //                                    ) X

        //                                LEFT JOIN
        //                                     (

        //                                       SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                              SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                              SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
        //                                       FROM BC_INVOICE_HDR IH 
        //                                            WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
        //                                       GROUP BY IH.CUST_ID) 

        //                                       A ON A.CUST_ID=X.CUST_ID

        //                              LEFT JOIN
        //                                      (
        //                                        SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                              SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                              SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
        //                                       FROM BC_INVOICE_HDR IH 
        //                                            WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
        //                                       GROUP BY IH.CUST_ID)

        //                                        B ON X.CUST_ID = B.CUST_ID

        //                              LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
        //                                                        ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
        //                                                END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) C ON X.CUST_ID = C.CUST_ID

        //                               LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
        //                                                        ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
        //                                                            END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) D ON X.CUST_ID = D.CUST_ID

        //                                    ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
        //                    }
        //                    else
        //                    {

        //                        sQuery3 = @"
        //                             Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
        //                                   X.LOCATION_CODE , 
        //                                   X.LOCATION_DESC,
        //                                   :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
        //                                   X.ADDRESS,
        //                                    NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
        //                                   C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
        //                                   C.VAT_AMT CURR_RECEIPT_VAT,
        //                                   nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
        //                                   nvl(A.LPS,0) ARREAR_LPS,
        //                                   nvl(A.VAT,0) ARREAR_VAT,
        //                                   nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,
        //                                   nvl(B.LPS,0) CURR_LPS,
        //                                   nvl(B.VAT,0) CURR_VAT,
        //                                   D.RECEIPT_AMT+D.VAT_AMT Total_Receipt_Arrear,
        //                                   D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
        //                                   D.VAT_AMT ARREAR_RECEIPT_VAT
        //                                FROM 
        //                                    (
        //                                       SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
        //                                              :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
        //                                       from EBC.BC_CUSTOMERS IH  

        //                                       INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
        //                                       INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
        //                                                WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
        //                                                   + @"
        //                                       GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
        //                                        BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
        //                                    ) X

        //                                LEFT JOIN
        //                                     (

        //                                       SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                              SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                              SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
        //                                       FROM BC_INVOICE_HDR IH 
        //                                            WHERE  IH.bill_cycle_code <:P_BILL_CYCLE_CODE AND " + query + @"
        //                                       GROUP BY IH.CUST_ID) 

        //                                       A ON A.CUST_ID=X.CUST_ID

        //                              LEFT JOIN
        //                                      (
        //                                        SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                              SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                              SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)) VAT 
        //                                       FROM BC_INVOICE_HDR IH 
        //                                            WHERE  IH.bill_cycle_code =:P_BILL_CYCLE_CODE AND " + query + @"
        //                                       GROUP BY IH.CUST_ID)

        //                                        B ON X.CUST_ID = B.CUST_ID

        //                              LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
        //                                                        ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
        //                                                END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) C ON X.CUST_ID = C.CUST_ID

        //                               LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,      SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
        //                                                        ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
        //                                                            END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) D ON X.CUST_ID = D.CUST_ID

        //                                    ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
        //                    }
        //                    var result = await conn.QueryAsync<PublicSecurityDivisionDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
        //                    customers.AddRange(result.ToList());

        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            continue;
        //        }
        //        mergeList.AddRange((from c in customers

        //                            from cust in ministryCustId
        //                            where c.CUST_ID == cust.CUST_ID
        //                            select new PublicSecurityDivisionMergeDTO
        //                            {
        //                                CustId = c.CUST_ID,
        //                                ConsumerNo = c.CONSUMER_NO,
        //                                CustomerName = c.CUSTOMER_NAME,
        //                                LocationCode = c.LOCATION_CODE,
        //                                LocationDesc = c.LOCATION_DESC,
        //                                ZoneCode = cust.ZONE_CODE,
        //                                ZoneName = cust.ZONE_NAME,
        //                                ZoneNameBn = cust.ZONE_NAME_BN,
        //                                MinistryCode = cust.MINISTRY_CODE,
        //                                MinistryName = cust.MINISTRY_NAME,
        //                                MinistryNameBn = cust.MINISTRY_NAME_BN,
        //                                CurrReceiptVat = c.CURR_RECEIPT_VAT,
        //                                CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
        //                                CurrLps = c.CURR_LPS,
        //                                CurrPrincipal = c.CURR_PRINCIPAL,
        //                                CurrVat = c.CURR_VAT,
        //                                ArrearLps = c.ARREAR_LPS,
        //                                ArrearPrincipal = c.ARREAR_PRICIPAL,
        //                                ArrearVat = c.ARREAR_VAT,
        //                                TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR,
        //                                ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
        //                                ArrearReceiptVat = c.ARREAR_RECEIPT_VAT,
        //                                HasDepartment = cust.HAS_DEPARTMENT
        //                            }));


        //    }


        //    return mergeList;

        //}

        private async Task<int?[]> GetAllCustIdByDbCode(string dbCode,string locationCode)
        {
            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
            using var con = new OracleConnection(connectionString);
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {
                con.Open();
                string query = $@"SELECT CUST_ID,CUSTOMER_NUM || CHECK_DIGIT CONSUMER_NO FROM EBC.BC_CUSTOMERS WHERE LOCATION_CODE='{locationCode}'";
                result =con.Query<CustIdInsertModel>(query).ToList();
                var custIdArray = result.Select(p => p.CUST_ID).ToArray();
                con.Close();
                int?[] custs = custIdArray;
                return custs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<CustomerZoneModel>> GetCusttomerWithZoneByDbCode(string dbCode, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            try
            {
                string query = $@"select L.ZONE_CODE,Z.NAME ZONE_NAME ,Z.NAMEBN ZONE_NAME_BN, L.DB_CODE from MISCBILLAPP_MS_LOCATION L , MISCBILLAPP_MS_ZONE Z
                                  where L.ZONE_CODE=Z.CODE  AND L.CODE='{locationCode}'";
                var zoneResult = await con.QueryFirstOrDefaultAsync<CustomerZoneModel>(query);

                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.

                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

                using var conn = new OracleConnection(connectionString);
                string squery = $@"SELECT CUSTOMER_NUM || CHECK_DIGIT CUSTOMER_NO,'{zoneResult.ZONE_CODE}' ZONE_CODE,
                                    '{zoneResult.ZONE_NAME}' ZONE_NAME,'{zoneResult.ZONE_NAME_BN}' ZONE_NAME_BN,'{zoneResult.DB_CODE}' DB_CODE
                                    FROM EBC.BC_CUSTOMERS WHERE LOCATION_CODE='{locationCode}'";
                var result = conn.Query<CustomerZoneModel>(squery).ToList();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<List<CustomerZoneModel>> GetMinistryCusttomerWithZoneByDbCode(string dbCode, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            try
            {
                con.Open();
                string query = $@"select C.CUSTOMER_NO, C.ZONE_CODE,Z.NAME ZONE_NAME ,Z.NAMEBN ZONE_NAME_BN, C.DB_CODE from MISCBILLAPP_GC_CUSTOMER C,MISCBILLAPP_GC_CUSTID CU,MISCBILLAPP_MS_ZONE Z
                                    WHERE  C.CUSTOMER_NO=CU.CONSUMER_NO
                                    AND C.ZONE_CODE=Z.CODE  AND C.LOCATION_CODE='{locationCode}'";
                var zoneResult =  con.Query<CustomerZoneModel>(query).ToList();
                return zoneResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<AllCustomerMode> GetAllCustomerDetails(string billMonth, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string dbCode, string locationCode)
        {
            AllCustomerMode allCustomer = new AllCustomerMode();
            var result = new List<GetCustomerArrearModel>();
            var zoneData = new List<CustomerZoneModel>();
            var minZoneData = new List<CustomerZoneModel>();
            var ministryCustomer = new List<GetCustomerArrearModel>();
            var privateCustomer = new List<GetCustomerArrearModel>();
            if (dbCode != "0")
            {
                var custIdDataList = await GetAllCustIdByDbCode(dbCode, locationCode);

                var zoneWithConsumer = await GetCusttomerWithZoneByDbCode(dbCode, locationCode);
                var zoneWithMinistryConsumer = await GetMinistryCusttomerWithZoneByDbCode(dbCode, locationCode);

                if (custIdDataList.Count() != 0)
                {
                    result.AddRange(await GetAllCustomerDataAsync(dbCode, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                }
                if (zoneWithConsumer.Count() != 0)
                {
                    zoneData.AddRange(zoneWithConsumer);
                    minZoneData.AddRange(zoneWithMinistryConsumer);
                }
            }
            else
            {
                var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
                foreach (var database in allDatabases)
                {
                    var custIdDataList = await GetAllCustIdByDbCodeAndMinistryCode(database.CODE, "2");
                    var zoneWithConsumer = await GetCusttomerWithZoneByDbCodeAndMinistryCode(database.CODE, "2");

                    if (custIdDataList.Count() != 0)
                    {
                        result.AddRange(await GetAllCustomerDataAsync(database.CODE, billMonth, NoOfMonth, ArrrearFrom, ArrrearTo, custIdDataList, isAll, isPrincipal, isLPS, isVAT, locationCode));
                    }
                    if (zoneWithConsumer.Count() != 0)
                    {
                        zoneData.AddRange(zoneWithConsumer);
                    }
                }

            }
            var excludedCustomers = (from m in minZoneData
                                     where result.Any(r => r.CONSUMER_NO == m.CUSTOMER_NO) &&
                                           zoneData.Any(z => z.CUSTOMER_NO == m.CUSTOMER_NO)
                                     select m.CUSTOMER_NO).ToList();
            privateCustomer.AddRange((from p in result
                                      from d in zoneData
                                      where p.CONSUMER_NO == d.CUSTOMER_NO &&
                                            !excludedCustomers.Contains(p.CONSUMER_NO)
                                      select new GetCustomerArrearModel
                                      {
                                          CONSUMER_NO = d.CUSTOMER_NO,
                                          LOCATION_CODE = p.LOCATION_CODE,
                                          PRN = p.PRN,
                                          LPS = p.LPS,
                                          VAT = p.VAT,
                                          TOTAL = p.TOTAL,
                                          CUSTOMER_NAME = p.CUSTOMER_NAME,
                                          ADDRESS = p.ADDRESS,
                                          BILL_CYCLE_CODE = p.BILL_CYCLE_CODE,
                                          LOCATION_DESC = p.LOCATION_DESC,
                                          NOM = p.NOM,
                                          ZONE_NAME = d.ZONE_NAME,
                                          ZONE_CODE = d.ZONE_CODE,
                                          IS_GOVT_CUSTOMER = "0"
                                      }));

            ministryCustomer.AddRange((from p in result
                                      from d in minZoneData
                                       where p.CONSUMER_NO == d.CUSTOMER_NO
                                      select new GetCustomerArrearModel
                                      {
                                          CONSUMER_NO = d.CUSTOMER_NO,
                                          LOCATION_CODE = p.LOCATION_CODE,
                                          PRN = p.PRN,
                                          LPS = p.LPS,
                                          VAT = p.VAT,
                                          TOTAL = p.TOTAL,
                                          CUSTOMER_NAME = p.CUSTOMER_NAME,
                                          ADDRESS = p.ADDRESS,
                                          BILL_CYCLE_CODE = p.BILL_CYCLE_CODE,
                                          LOCATION_DESC = p.LOCATION_DESC,
                                          NOM = p.NOM,
                                          ZONE_NAME = d.ZONE_NAME,
                                          ZONE_CODE = d.ZONE_CODE,
                                          IS_GOVT_CUSTOMER = "1"
                                      }));
            allCustomer.PrivateCustomer = privateCustomer;
            allCustomer.MinistryCustomer=ministryCustomer;
            return (allCustomer);

        }
    }
}
