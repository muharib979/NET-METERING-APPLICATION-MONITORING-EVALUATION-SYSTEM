using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Castle.Core.Resource;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.MISReport;
using Core.Domain.CityCorporation;
using Core.Domain.Ministry;
using Core.Domain.Untracable;
using Core.Domain.ZoneCircle;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.MISReport;
using System.Collections.Generic;
using System.Security.Principal;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MISReport
{
    public class UntraceableCustomerRepository : IUntraceableCustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        private readonly ILocationRepository _locationRepoistory;
        public UntraceableCustomerRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo, ILocationRepository locationRepository)
        {
            _mapper = mapper;
            _commRepo = commRepo;
            _dbConfigRepo = dbConfigRepo;
            _locationRepoistory = locationRepository;
        }

        public async Task<List<MergeUntraceable>> GetAllIUntraceableCustomer(string reportDate)
        {
            List<UntraceableCustomerDTO> untraceableCustomersList = new List<UntraceableCustomerDTO>();
            var margeData = new List<MergeUntraceable>();
            var UntraceableList = await GetUntraceablesListByCreateDateAsync(reportDate);

            try
            {
                var custIds = await GetUCCustIdByReportDate(reportDate);
                var dbCodes = await GetDatabaseCode(reportDate);
                var locationCodes = await GetLocationCodeByReportDate(reportDate);
                var joinLocationCode = "'" + string.Join("', '", locationCodes) + "'";

                foreach (var dbCode in dbCodes)
                {
                    try
                    {

                        var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode.CODE); // Getting database info by id.

                        string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

                        //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 119.40.95.187)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";


                        using IDbConnection conn = new OracleConnection(connectionString);


                        int arraySize = custIds.Count();
                        int chunkSize = 500;
                        int skipChunkSize = 0;

                        while (skipChunkSize < arraySize)
                        {
                            string query = " C.cust_id ";

                            string _custIds = GetChunkCustomers(custIds.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;

                            query = query + " in (" + _custIds + @") ";

                            //var sQuery3 = @" select  (c.customer_num || c.check_digit) CONSUMER_NO
                            //                ,G.LOCATION_CODE
                            //                ,i.NOM
                            //                ,i.PRINCIPAL PRN
                            //                ,i.LPS
                            //                ,i.VAT
                            //                ,i.PRINCIPAL+i.LPS+i.VAT TOTAL
                            //                ,:BillMonth BILL_CYCLE_CODE
                            //                ,(ca.addr_descr1 ||  ca.addr_descr2 || ca.addr_descr3)  ADDRESS
                            //                ,c.customer_name CUSTOMER_NAME
                            //                ,G.DESCR LOCATION_DESC

                            //                FROM EBC.BC_CUSTOMERS C,
                            //                EBC.BC_CUSTOMER_ADDR CA,
                            //                EBC.BC_LOCATION_MASTER G,
                            //                ( SELECT CUST_ID,COUNT(BILL_CYCLE_CODE) NOM,
                            //                    SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) PRINCIPAL,
                            //                SUM(LPS_AMT+LPS_ADJ-LPS_APPL) LPS,
                            //                SUM(VAT_AMT+VAT_ADJ-VAT_APPL) VAT
                            //                from bc_invoice_hdr 
                            //                where  bill_cycle_code <= :BillMonth
                            //                AND ((PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) + (LPS_AMT+LPS_ADJ-LPS_APPL)+
                            //                (VAT_AMT+VAT_ADJ-VAT_APPL)) > 0
                            //                group by cust_id " +
                            //                        (NOMExist ? @"having COUNT(BILL_CYCLE_CODE)>= :noOfMonth " : " ") +
                            //                        @" ) I 

                            //                where  c.cust_id=CA.cust_id
                            //                and   G.LOCATION_CODE = C.LOCATION_CODE
                            //                and c.cust_id=I.cust_id
                            //                AND ca.addr_type = 'B' "+
                            //                        @" AND " + query;


                            var sQuery3 = @$"  select  (c.customer_num || c.check_digit) CONSUMER_NO
                                    ,G.LOCATION_CODE,
                                     C.CONS_EXTG_NUM  PV_AC,
                                    C.CUST_ID
                                    ,i.PRINCIPAL+i.LPS+i.VAT TOTAL_ARREAR,
                                (select distinct max(bill_cycle_code) from bc_invoice_hdr where cust_id = c.CUST_Id ) LAST_BILL_MONTH
       
                                    FROM EBC.BC_CUSTOMERS C,
                                    EBC.BC_CUSTOMER_ADDR CA,
                                    EBC.BC_LOCATION_MASTER G,
                                    ( SELECT CUST_ID,COUNT(BILL_CYCLE_CODE) NOM,
                                        SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) PRINCIPAL,
                                    SUM(LPS_AMT+LPS_ADJ-LPS_APPL) LPS,
                                    SUM(VAT_AMT+VAT_ADJ-VAT_APPL) VAT
                                    from bc_invoice_hdr 
                                    where  bill_cycle_code <= (select max(bill_cycle_code) from bc_invoice_hdr )
                                    AND ((PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) + (LPS_AMT+LPS_ADJ-LPS_APPL)+
                                    (VAT_AMT+VAT_ADJ-VAT_APPL)) > 0
                                    group by cust_id   ) I 
                    
                                    where  c.cust_id=CA.cust_id
                                    and   G.LOCATION_CODE = C.LOCATION_CODE
                                    and c.cust_id=I.cust_id
                                    AND ca.addr_type = 'B'  AND C.LOCATION_CODE in ({joinLocationCode}) 
                                     AND ca.addr_exp_date IS NULL  AND " + query + " ";



                            var result = await conn.QueryAsync<UntraceableCustomerDTO>(sQuery3);

                            untraceableCustomersList.AddRange(result.ToList());

                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                margeData.AddRange((from p in untraceableCustomersList

                                    from d in UntraceableList
                                    where p.CUST_ID == d.CUST_ID
                                    select new MergeUntraceable
                                    {
                                        NAME = d.CUSTOMER_NAME,
                                        CON_NO = d.CUSTOMER_NUMBER,

                                        ADDR = d.CUSTOMER_ADDRESS,
                                        PV_AC = p.PV_AC,
                                        LAST_BILL_MONTH = p.LAST_BILL_MONTH,
                                        DISC_DATE = d.CREATED_DATE,
                                        TOTAL_ARREAR = p.TOTAL_ARREAR,
                                        CUST_ID = p.CUST_ID,
                                        STATUS = p.STATUS,
                                        UC_TYPE = d.UC_TYPE
                                    }));
                return margeData;
            }
            catch (Exception ex)
            {
                throw;
            }





            //using var con = new OracleConnection(Context.Connection.ConnectionString());
            //string sQuery = @"";
            //var result = await con.QueryAsync<UntraceableCustomerDTO>(sQuery, new { reportDate = reportDate.ToString().ToUpper() });
            //untraceableCustomersDTO = _mapper.Map(result.ToList(), untraceableCustomersDTO);
            //return untraceableCustomersDTO;
        }

        #region utility method
        public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseCode(string reportDate)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @" Select distinct  db_code CODE from MISCBILLAPP_MS_LOCATION where CODE IN (
                                SELECT DISTINCT
                                UC.LOCATION_CODE
                                FROM
                                MISCBILLAPP_UC_CONSUMER UC  WHERE uc.created_date = :reportDate)";
            var result = await con.QueryAsync<Core.Domain.DatabaseConfig.DatabaseConfig>(sQuery, new { reportDate = reportDate.ToUpper() });
            return result.ToList();
        }

        public async Task<List<int>> GetUCCustIdByReportDate(string reportDate)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @" SELECT 
                                C.CUST_ID
                                FROM  
                                MISCBILLAPP_UC_CONSUMER UC 
                                INNER JOIN BC_CUSTOMERS C ON (C.CUSTOMER_NUM||C.CHECK_DIGIT) = ( UC.CUSTOMER_NUMBER) 
                                WHERE uc.created_date =:report_Date";
            var result = await con.QueryAsync<int>(sQuery, new { report_Date = reportDate.ToUpper() });
            return result.ToList();
        }
        public async Task<List<string>> GetLocationCodeByReportDate(string reportDate)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @" SELECT DISTINCT
                                UC.LOCATION_CODE
                                FROM
                                MISCBILLAPP_UC_CONSUMER UC  WHERE uc.created_date = :reportDate";
            var result = await con.QueryAsync<string>(sQuery, new { reportDate = reportDate.ToUpper() });
            return result.ToList();
        }

        public async Task<List<Untraceable>> GetUntraceablesListByCreateDateAsync(string reportDate)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @" SELECT uc.customer_name, uc.customer_number, uc.customer_address, uc.location_name, uc.previous_acc_no, uc.created_date ,c.cust_id, 
                                decode(uc.UC_TYPE ,'03','Regular','01','Temp_Discon','02','Perm_Discon')  UC_TYPE 
                                FROM
                                MISCBILLAPP_UC_CONSUMER UC
                                INNER JOIN BC_CUSTOMERS C ON(C.CUSTOMER_NUM || C.CHECK_DIGIT) = (UC.CUSTOMER_NUMBER)
                                WHERE uc.created_date = :reportDate";
            var result = await con.QueryAsync<Untraceable>(sQuery, new { reportDate = reportDate.ToUpper() });
            return result.ToList();

        }

        public string GetChunkCustomers(int[] customers)
        {
            string _customers = string.Join(", ", customers);

            if (_customers.Length > 0)
                _customers = "" + _customers + "";

            return _customers;
        }


        #endregion utility method


        #region mis-report untraceable customer multiple db and location
        public List<UntracebleCustReportDTO> UnTraceableCustomerData { get; set; } = new List<UntracebleCustReportDTO>();
        public async Task<List<UntracebleCustReportDTO>> GetMisUntraceableCustomer(string[] dbCodes, string?[] locationCodes, string billMonth, int userId, int roleId)
        {

            try
            {
                List<Task> listOfTasks = new List<Task>();
                if (dbCodes.Length > 0 && locationCodes != null)
                {
                    string locCode = (locationCodes != null) ? "'" + string.Join("', '", locationCodes) + "'" : " ";
                    foreach (var code in dbCodes)
                    {
                        try
                        {
                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                            listOfTasks.Add(Task.Run(() => GetUnTraceableCustomerAsync(code, locCode.TrimEnd(','), billMonth, connectionString)));
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            continue;
                        }
                    }

                }

                else if (dbCodes.Length > 0 && locationCodes == null)
                {
                    foreach (var center in dbCodes)
                    {
                        try
                        {

                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(center); // Getting database info by code.
                            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                            if (connectionString == null) throw new ArgumentNullException();

                            var locationString = " ";
                            var locations = await _locationRepoistory.GetLocationByDBMIdAsync(center, userId, roleId);

                            foreach (var res in locations)
                            {
                                if (res.ID != 0)
                                {

                                    locationString += "'" + res.CODE + "'" + ",";
                                }
                            }

                            listOfTasks.Add(Task.Run(() => GetUnTraceableCustomerAsync(center, locationString.TrimEnd(','), billMonth, connectionString)));

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            continue;
                        }
                    }
                }


                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                throw;
            }

            return UnTraceableCustomerData.ToList();

        }



        private async Task GetUnTraceableCustomerAsync(string CenterId, string LocationCode, string BillMonth, string connectionString)
        {
            try
            {
                
                using IDbConnection conn = new OracleConnection(connectionString);
                var isLocationCodeExists = (LocationCode != "0") ? true : false;        //loactionCode 0 for all

                try
                {
                    var sQuery = @$"select
                                    -- DENSE_RANK() OVER (ORDER BY a.CUSTOMER_NAME) AS SL,
                                    substr(a.CUSTOMER_NAME,1,35) name,
                                    e.addr_descr1 || e.addr_descr2 || e.addr_descr3 AS addr,
                                    a.CUSTOMER_NUM||a.CHECK_DIGIT con_no,
                                    a.CONS_EXTG_NUM pv_ac,
                                    i.total_arrear,
                                    decode(M.METER_STATUS,'2','Regular','1','Temp_Discon','3','Perm_Discon') STATUS,CASE METER_STATUS WHEN '1' THEN TEMP_DISCON_DATE
                                   WHEN '3' THEN PERM_DISCON_DATE
                                   ELSE PERM_DISCON_DATE END as DISC_DATE
                                    from ebc.bc_customers a,
                                    ebc.bc_customer_category c,
                                    ebc.bc_category_master cm,
                                    ebc.bc_customer_addr e,
                                    ebc.bc_monthly_likely f,
                                    ebc.bc_location_master g,
                                    (select cust_id,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL+ LPS_AMT+LPS_ADJ-LPS_APPL + VAT_AMT+VAT_ADJ-VAT_APPL) total_arrear
                                    from bc_invoice_hdr where  bill_cycle_code <= {BillMonth}
                                    group by cust_id) i,
                                    (select distinct M.CUST_ID , max(M.CONNECTION_ID) AS CONNECTION_ID  from EBC.BC_CUSTOMER_METER m  group by M.CUST_ID ) MT,
                                    EBC.BC_CUSTOMER_METER M
                                    where  a.cust_id=e.cust_id
                                    and  a.cust_id=f.cust_id
                                    and a.cust_id=i.cust_id
                                    and c.CUST_ID=a.CUST_ID
                                    and c.cat_id=cm.category_id and c.exp_date is null
                                    AND  cm.usage_category_code<>'I'
                                    and a.LOCATION_CODE=g.LOCATION_CODE
                                    and f.EXP_DATE is null and e.addr_type='B'
                                    and e.ADDR_EXP_DATE is null
                                    and i.total_arrear>1
                                    AND A.CUST_ID = MT.CUST_ID
                                    AND MT.CONNECTION_ID = M.CONNECTION_ID"
                                + (isLocationCodeExists ? @$" AND A.LOCATION_CODE IN ({LocationCode})" : "")
                                + " AND (M.METER_STATUS =1 OR M.METER_STATUS =3) ORDER BY a.CUSTOMER_NAME";

                    //var parameters = new
                    //{
                    //    LocationCode = LocationCode.ToUpper(),
                    //    BillMonth = BillMonth
                    //};
                    var result = await conn.QueryAsync<UntracebleCustReportDTO>(sQuery);
                    UnTraceableCustomerData.AddRange(result);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
                throw ex;
            }

        }

        #endregion #region mis-report untraceable customer multiple db and location

        //#region GetUntracedCustArrearCollection Old

        //public async Task<List<UntracebleCustArrearReportDTO>> GetUntracedCustArrearCollection(string zoneCode, string circleCode, string locationCode, string billMonth)
        //{

        //    var ucData = await GetUCData();
        //    var ucDataCountByBillMonth = await GetUCArrearDataByBillmonth(billMonth);
        //    var maxBillmonthUcData = await GetMaxBillmonthUcData();
        //    using var conb = new OracleConnection(Connection.ConnectionString());
        //    var dbCodeList = ucData.Select(c => c.DB_CODE).Distinct().ToList();
        //    if (ucDataCountByBillMonth.Count == 0)
        //    {
        //        var ucArrearCustData = await GetUcArrearSaveData(dbCodeList, billMonth);
        //        if (ucArrearCustData.Count > 0)
        //        {
        //            var saveData = await InsertUcCustomerArrearDataAsync(ucArrearCustData);
        //        }
        //        return await GetUntraceableOfflineDate(zoneCode, circleCode, locationCode, billMonth);
        //    }
        //    else if (ucDataCountByBillMonth.Count != maxBillmonthUcData.Count())
        //    {
        //        var deleteData = await DeleteUCArrearDataByBillmonth(billMonth);

        //        var ucArrearCustData = await GetUcArrearSaveData(dbCodeList, billMonth);
        //        if (ucArrearCustData.Count > 0 && deleteData > 0)
        //        {
        //            var saveData = await InsertUcCustomerArrearDataAsync(ucArrearCustData);
        //        }
        //        return await GetUntraceableOfflineDate(zoneCode, circleCode, locationCode, billMonth);
        //    }
        //    else
        //    {
        //        query excute from offline MISCBILLAPP_UC_CUST_ARR
        //       return await GetUntraceableOfflineDate(zoneCode, circleCode, locationCode, billMonth);
        //    }
        //}

        //#region untraceableExtraMethods
        //private async Task<List<UntracebleCustArrear>> GetUCArrearDataByBillmonth(string billMonth)
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    string query1 = @"SELECT * FROM MISCBILLAPP_UC_CUST_ARR WHERE BILL_CYCLE_CODE = :BillMonth";
        //    var result = await con.QueryAsync<UntracebleCustArrear>(query1, new { BillMonth = billMonth });
        //    return result.ToList();
        //}

        //private async Task<List<UntracebleCustArrear>> GetMaxBillmonthUcData()
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    string query1 = @"SELECT * FROM MISCBILLAPP_UC_CUST_ARR where BILL_CYCLE_CODE = (SELECT MAX(BILL_CYCLE_CODE) FROM MISCBILLAPP_UC_CUST_ARR)";
        //    var result = await con.QueryAsync<UntracebleCustArrear>(query1);
        //    return result.ToList();
        //}

        //private async Task<List<Untraceable>> GetUCData()
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    string query1 = @"SELECT * FROM MISCBILLAPP_UC_CONSUMER";
        //    var result = await con.QueryAsync<Untraceable>(query1);
        //    return result.ToList();
        //}

        //private async Task<List<UntracedCustomerArrearModel>> GetUcArrearSaveData(List<string> dbCodeList, string billMonth)
        //{
        //    var ucData = await GetUCData();
        //    var customers = new List<UntracedCustomerArrearModel>();
        //    foreach (var dbCode in dbCodeList)
        //    {
        //        var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
        //        var dbWiseCustId = ucData.Where(c => c.DB_CODE.Equals(dbCode)).Select(s => s.CUST_ID).ToList();
        //        if (dbWiseCustId.Count > 0)
        //        {
        //            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
        //            if (connectionString == null) throw new ArgumentNullException();

        //            string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
        //            string maxBillMonth = await GetMaxBillCycleCode(connectionString, dbCode, db.NAME);
        //            maxBillMonth = "202302";


        //            using IDbConnection conn = new OracleConnection(connectionString);

        //            try
        //            {
        //                int arraySize = dbWiseCustId.Count;
        //                int chunkSize = 500;
        //                int skipChunkSize = 0;
        //                string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
        //                var convertBillMonth = DateTime.Parse(createDateString);
        //                string bill_cycle = billMonth;
        //                string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

        //                while (skipChunkSize < arraySize)
        //                {
        //                    string query = " IH.CUST_ID ";
        //                    string Bccc = " BCC.CUST_ID ";
        //                    string _custId = GetChunkCustomers(dbWiseCustId.Skip(skipChunkSize).Take(chunkSize).ToArray());
        //                    skipChunkSize += chunkSize;
        //                    query = query + " in (" + _custId + @") ";
        //                    Bccc = query + " in (" + _custId + @") ";

        //                    var sQuery3 = @"
        //                            SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
        //                                   X.LOCATION_CODE , 
        //                                   X.LOCATION_DESC,
        //                                   :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
        //                                   X.ADDRESS,
        //                                   C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
        //                                   C.VAT_AMT CURR_RECEIPT_VAT,
        //                                   D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
        //                                   D.VAT_AMT ARREAR_RECEIPT_VAT,
        //                                   nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,nvl(A.LPS,0) ARREAR_LPS,nvl(A.VAT,0) ARREAR_VAT,
                                           
        //                                   nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,nvl(B.LPS,0) CURR_LPS,nvl(B.VAT,0) CURR_VAT,
                                           
        //                                   nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0) AS TOTAL_PRINCIPAL_ARREAR,
        //                                   nvl(a.LPS,0) + nvl(B.LPS,0) AS TOTAL_LPS_ARREAR,
        //                                   nvl(a.VAT,0) +nvl( B.VAT,0)  AS TOTAL_VAT_ARREAR,
                                           
        //                                   ( nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0)+nvl(a.LPS,0) +nvl( B.LPS,0)+nvl(a.VAT,0) +nvl( B.VAT,0)) TOTAL_AMOUNT,
    
        //                                    NVL(C.RECEIPT_AMT,0) TOTAL_RECEIPT_PRINCIPAL,NVL(C.VAT_AMT,0) TOTAL_RECEIPT_VAT,
        //                               CASE 
        //                                    WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.CURR_RECEIPT_AMT)
        //                               ELSE 0 
        //                                     END AS ARREAR_RECEIPT_AMOUNT,
        //                               CASE 
        //                                    WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (B.CURR_RECEIPT_AMT)
        //                               ELSE (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) 
        //                                     END AS CURR_RECEIPT_AMT
        //                                FROM 
        //                                    (
        //                                       SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
        //                                              :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
        //                                       from EBC.BC_CUSTOMERS IH  
                                                
        //                                       INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=IH.CUST_ID
        //                                       INNER JOIN EBC.BC_LOCATION_MASTER BCLM ON BCLM.LOCATION_CODE=IH.LOCATION_CODE 
        //                                                WHERE BCA.ADDR_TYPE = 'B' AND BCA.ADDR_EXP_DATE IS NULL  AND " + query
        //                                                        + @"
        //                                       GROUP BY IH.CUST_ID, BCLM.LOCATION_CODE, IH.cust_id,IH.customer_num , IH.check_digit,IH.customer_name,
        //                                        BCA.addr_descr1 , BCA.addr_descr2 , BCA.addr_descr3,BCLM.DESCR
        //                                    ) X

        //                                LEFT JOIN
        //                                     (

        //                                       SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                              SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                              SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT  

        //                                       FROM BC_INVOICE_HDR IH 
        //                                            WHERE BILL_CYCLE_CODE <:P_BILL_CYCLE_CODE AND " + query + @"
        //                                       GROUP BY IH.CUST_ID) 

        //                                       A ON A.CUST_ID=X.CUST_ID

        //                               LEFT JOIN
        //                                      (
        //                                        SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
        //                                               SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
        //                                               SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT,
        //                                               sum(NVL(IH.INVOICE_AMT,0)) CURR_RECEIPT_AMT

        //                                        FROM BC_INVOICE_HDR IH 
        //                                            WHERE BILL_CYCLE_CODE = :P_BILL_CYCLE_CODE AND  " + query + @"
        //                                       GROUP BY IH.CUST_ID)
                                               
        //                                        B ON X.CUST_ID = B.CUST_ID

        //                              LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) C ON X.CUST_ID = C.CUST_ID

        //                              LEFT JOIN 
        //                                     ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

        //                                        FROM BC_RECEIPT_HDR R
        //                                        INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
        //                                                WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
        //                                       ) D ON X.CUST_ID = D.CUST_ID";

        //                    var result = await conn.QueryAsync<UntracedCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_bill_cycleRECEIPT = bill_cycleRECEIPT });

        //                    customers.AddRange(result.ToList());

        //                }

        //            }
        //            catch (Exception ex)
        //            {

        //                throw;
        //            }
        //        }

        //    }

        //    return customers;
        //}

        //private async Task<int> DeleteUCArrearDataByBillmonth(string billMonth)
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    string query1 = @"DELETE FROM MISCBILLAPP_UC_CUST_ARR WHERE BILL_CYCLE_CODE = :BillMonth";
        //    var result = await con.ExecuteAsync(query1, new { BillMonth = billMonth });
        //    return result;
        //}

        //public async Task<int> InsertUcCustomerArrearDataAsync(List<UntracedCustomerArrearModel> customerArrear)
        //{
        //    int inserted;
        //    var currentDate = DateTime.Now.ToString("dd -MMM-y");
        //    using var conn = new OracleConnection(Connection.ConnectionString());
        //    conn.Open();
        //    using var tran = conn.BeginTransaction();

        //    try
        //    {

        //        var sQuery = $@" INSERT INTO MISCBILLAPP_UC_CUST_ARR (CUSTOMER_NO, NAME, LOCATION_CODE, PRINCIPAL, CURR_VAT, CURR_LPS, CURR_DATE, RUN_BILL_CYCLE_CODE, BILL_CYCLE_CODE, CURR_BILL, PREV_MONTH, RECEIPT_AMT, LOCATION_DESCBN, ARREAR_AMT, ADDRESS, ARREAR_PRINCIPAL, ARREAR_LPS, ARREAR_VAT, TOTAL_PRINCIPAL_ARREAR, TOTAL_LPS_ARREAR, TOTAL_VAT_ARREAR, TOTAL_RECEIPT_PRINCIPAL, TOTAL_RECEIPT_VAT, ARREAR_RECEIPT_AMOUNT,FINANCIAL_AMOUNT, CURR_RECEIPT_VAT, CURR_RECEIPT_AMT,CURR_RECEIPT_PRINCIPAL, ARREAR_RECEIPT_PRINCIPAL, ARREAR_RECEIPT_VAT) VALUES
        //        (:CONSUMER_NO, :CUSTOMER_NAME, :LOCATION_CODE, :CURR_PRINCIPAL, :CURR_VAT, :CURR_LPS, " + @$" TO_DATE('" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "', 'YYYY-MM-DD HH24:MI:SS')," + @" NULL, :BILL_CYCLE_CODE, :TOTAL_AMOUNT, 0, :TOTAL_AMOUNT, N' ',:TOTAL_AMOUNT, :ADDRESS, :ARREAR_PRICIPAL, :ARREAR_LPS, :ARREAR_VAT, :TOTAL_PRINCIPAL_ARREAR, :TOTAL_LPS_ARREAR, :TOTAL_VAT_ARREAR, :TOTAL_RECEIPT_PRINCIPAL, :TOTAL_RECEIPT_VAT, :ARREAR_RECEIPT_AMOUNT,:FINANCIAL_AMOUNT, :CURR_RECEIPT_VAT,:CURR_RECEIPT_AMT,:CURR_RECEIPT_PRINCIPAL, :ARREAR_RECEIPT_PRINCIPAL, :ARREAR_RECEIPT_VAT)";

        //        inserted = conn.Execute(sQuery, customerArrear, transaction: tran);
        //        if (inserted > 0)
        //        {
        //            tran.Commit();
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        throw;
        //    }
        //    return inserted;
        //}

        //public async Task<List<UntracebleCustArrearReportDTO>> GetUntraceableOfflineDate(string zoneCode, string circleCode, string locationCode, string billMonth)
        //{
        //    var isZone = zoneCode != "0" ? true : false;
        //    var isCircle = circleCode != "0" ? true : false;
        //    var isLocation = locationCode != "0" ? true : false;

        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
        //    oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        //    oracleDynamicParameters.Add("BILLMONTH", value: billMonth.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //    oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //    oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //    oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
        //    var result = con.Query<UntracebleCustArrearReportDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.UNTRACED_CUST_ARR_COLLECTION", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();

        //    return result;
        //}
        //#endregion

        //#endregion

        #region Untracebale Summary

        public async Task<Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustArrearMergeSummaryDto>, List<UntracedCustArrearMergeSummaryDto>>> GetUntracedArrearSummary(string zoneCode, string billMonth)
        {
            var ucData = await GetUCTraceDataWithZone(zoneCode, billMonth);
            var ucCurrMonthData = await GetUCTraceDataCurrMonthWithZone(zoneCode, billMonth);
            var ucPreMonthData = await GetUCTracePreMonthDataWithZone(zoneCode, billMonth);
            var dbCodeList = ucData.Select(c => c.DB_CODE).Distinct().ToList();
            var previousMergeList = new List<UntracedCustomerArrearModel>();
            var currMergeList = new List<UntracedCustomerArrearModel>();


            var customers = new List<UntracedCustomerArrearModel>();

            foreach (var dbCode in dbCodeList)
            {
                var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
                var dbWiseCustId = ucData.Where(c => c.DB_CODE.Equals(dbCode)).Select(s => s.CUST_ID).ToList();
                if (dbWiseCustId.Count > 0)
                {
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    if (connectionString == null) throw new ArgumentNullException();

                    //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
                    //string maxBillMonth = await GetMaxBillCycleCode(connectionString, dbCode, db.NAME);
                    // maxBillMonth = "202302";


                    using IDbConnection conn = new OracleConnection(connectionString);

                    try
                    {
                        int arraySize = dbWiseCustId.Count;
                        int chunkSize = 500;
                        int skipChunkSize = 0;
                        string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
                        var convertBillMonth = DateTime.Parse(createDateString);
                        string bill_cycle = billMonth;
                        string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";
                            string _custId = GetChunkCustomers(dbWiseCustId.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;
                            query = query + " in (" + _custId + @") ";
                            Bccc = query + " in (" + _custId + @") ";

                            var sQuery3 = @"
                                    SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,nvl(A.LPS,0) ARREAR_LPS,nvl(A.VAT,0) ARREAR_VAT,
                                           
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,nvl(B.LPS,0) CURR_LPS,nvl(B.VAT,0) CURR_VAT,
                                           
                                           nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0) AS TOTAL_PRINCIPAL_ARREAR,
                                           nvl(a.LPS,0) + nvl(B.LPS,0) AS TOTAL_LPS_ARREAR,
                                           nvl(a.VAT,0) +nvl( B.VAT,0)  AS TOTAL_VAT_ARREAR,
                                           
                                           ( nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0)+nvl(a.LPS,0) +nvl( B.LPS,0)+nvl(a.VAT,0) +nvl( B.VAT,0)) TOTAL_AMOUNT,
    
                                            NVL(C.RECEIPT_AMT,0) TOTAL_RECEIPT_PRINCIPAL,NVL(C.VAT_AMT,0) TOTAL_RECEIPT_VAT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.CURR_RECEIPT_AMT)
                                       ELSE 0 
                                             END AS ARREAR_RECEIPT_AMOUNT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (B.CURR_RECEIPT_AMT)
                                       ELSE (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) 
                                             END AS CURR_RECEIPT_AMT
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

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                       SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                       SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT,
                                                       sum(NVL(IH.INVOICE_AMT,0)) CURR_RECEIPT_AMT

                                                FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE <= :P_BILL_CYCLE_CODE AND  " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID";

                            var result = await conn.QueryAsync<UntracedCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_bill_cycleRECEIPT = bill_cycleRECEIPT });

                            customers.AddRange(result.ToList());

                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

            }
            //UntracedCustArrearMergeSummaryDto
            currMergeList.AddRange((from c in customers
                                    from cust in ucCurrMonthData
                                    where c.CUST_ID == cust.CUST_ID
                                    select new UntracedCustomerArrearModel
                                    {
                                        CUST_ID = c.CUST_ID,
                                        CONSUMER_NO = c.CONSUMER_NO,
                                        LOCATION_CODE = c.LOCATION_CODE,
                                        PRN = c.PRN,
                                        LPS = c.LPS,
                                        VAT = c.VAT,
                                        TOTAL = c.TOTAL,
                                        RECEIVE_PRN = c.RECEIVE_PRN,
                                        RECEIVE_LPS = c.RECEIVE_LPS,
                                        RECEIVE_VAT = c.RECEIVE_VAT,
                                        RECEIVE_TOTAL = c.RECEIVE_TOTAL,
                                        CUSTOMER_NAME = c.CUSTOMER_NAME,
                                        ADDRESS = c.ADDRESS,
                                        BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
                                        ARREAR_AMT = c.ARREAR_AMT,
                                        RECEIPT_AMT = c.RECEIPT_AMT,
                                        CURR_BILL = c.CURR_BILL,
                                        PREV_MONTH = c.PREV_MONTH,
                                        LOCATION_DESC = c.LOCATION_DESC,
                                        ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                        ARREAR_LPS = c.ARREAR_LPS,
                                        ARREAR_VAT = c.ARREAR_VAT,
                                        CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                        CURR_LPS = c.CURR_LPS,
                                        CURR_VAT = c.CURR_VAT,
                                        TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
                                        TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
                                        TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
                                        TOTAL_AMOUNT = c.TOTAL_AMOUNT,
                                        TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                        TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                        ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                        FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
                                        CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                        CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                        CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                        ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
                                        ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
                                        ZONE_CODE = cust.ZONE_CODE,
                                        ZONE_NAMEBN = cust.ZONE_NAMEBN
                                    }));

            previousMergeList.AddRange((from c in customers
                                        from cust in ucPreMonthData
                                        where c.CUST_ID == cust.CUST_ID
                                        select new UntracedCustomerArrearModel
                                        {
                                            CUST_ID = c.CUST_ID,
                                            CONSUMER_NO = c.CONSUMER_NO,
                                            LOCATION_CODE = c.LOCATION_CODE,
                                            PRN = c.PRN,
                                            LPS = c.LPS,
                                            VAT = c.VAT,
                                            TOTAL = c.TOTAL,
                                            RECEIVE_PRN = c.RECEIVE_PRN,
                                            RECEIVE_LPS = c.RECEIVE_LPS,
                                            RECEIVE_VAT = c.RECEIVE_VAT,
                                            RECEIVE_TOTAL = c.RECEIVE_TOTAL,
                                            CUSTOMER_NAME = c.CUSTOMER_NAME,
                                            ADDRESS = c.ADDRESS,
                                            BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
                                            ARREAR_AMT = c.ARREAR_AMT,
                                            RECEIPT_AMT = c.RECEIPT_AMT,
                                            CURR_BILL = c.CURR_BILL,
                                            PREV_MONTH = c.PREV_MONTH,
                                            LOCATION_DESC = c.LOCATION_DESC,
                                            ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                            ARREAR_LPS = c.ARREAR_LPS,
                                            ARREAR_VAT = c.ARREAR_VAT,
                                            CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                            CURR_LPS = c.CURR_LPS,
                                            CURR_VAT = c.CURR_VAT,
                                            TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
                                            TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
                                            TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
                                            TOTAL_AMOUNT = c.TOTAL_AMOUNT,
                                            TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                            TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                            ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                            FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
                                            CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                            CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                            CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                            ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
                                            ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
                                            ZONE_CODE = cust.ZONE_CODE,
                                            ZONE_NAMEBN = cust.ZONE_NAMEBN
                                        }));

            var penaltyDataList = await GetPenaltyAndSupplimentryDataListZoneWise("1",zoneCode,billMonth);


            var supplementaryDataList = await GetPenaltyAndSupplimentryDataListZoneWise("2", zoneCode, billMonth);


            return new Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustArrearMergeSummaryDto>, List<UntracedCustArrearMergeSummaryDto>>(currMergeList, previousMergeList,penaltyDataList, supplementaryDataList);

        }

        #region Penalty Summary 
        private async Task<List<UntracedCustArrearMergeSummaryDto>> GetPenaltyAndSupplimentryDataListZoneWise(string billTypeCode, string zoneCode, string billMonth)
        {
            var isZone = zoneCode != "0" ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            string query1 = @$"SELECT

                                case when X.ZoneCode is null then y.ZoneCode
                                else X.ZoneCode end   ZoneCode,
                                case when x.zonenamebn is null then y.zonenamebn
                                else x.zonenamebn end   ZoneName,
                                nvl(x.totalamount,0) PreMonTracedCustArrear,
                                nvl(X.totalconsumer,0) PreMonTracedCustCount,
                                nvl(x.recieptamount,0) PreMonTracedCustReceipt,
                                NVL(Y.totalamount,0) CurrMonTracedCustArrear,
                                NVL(Y.totalconsumer,0) CurrMonTracedCustCount,
                                NVL(Y.recieptamount,0) CurrMonTracedCustReceipt
                                ,NVL(x.totalconsumer,0)+NVL(y.totalconsumer,0) TotalCurrMonTracedCustCount,
                                 NVL(x.totalamount,0)+NVL(y.totalamount,0) TotalCurrMonTracedCustArrear,
                                NVL(x.recieptamount,0)+NVL(y.recieptamount,0) TotalCurrMonTracedCustReceipt
                                from
                                ( SELECT  COUNT (MS.CUSTOMER_NUM)  TotalConsumer , SUM(MS.TOTAL_BILL_AMOUNT) TotalAmount, L.ZONE_CODE   ZoneCode, z.namebn ZoneNameBn,
                              0 RecieptAmount
                                from MISCBILLAPP_MISC_BILL_MST MS
                                 LEFT JOIN MISCBILLAPP_MS_LOCATION L ON L.CODE=MS.LOCATION_CODE
                                 left JOIN MISCBILLAPP_MS_ZONE Z on Z.CODE = l.Zone_code
                                WHERE   TO_CHAR(TO_DATE(MS.READING_DATE, 'DD-MON-RR'), 'YYYYMM')=:P_BILL_MONTH
                                and ms.BILL_TYPE_CODE=:billTypeCode " + (isZone ? @$"and Z.CODE =:zoneCode " : " ") +
                                 @" GROUP BY 
                                 L.ZONE_CODE,  z.namebn)Y
                                 FUll JOIN
                                 (SELECT  COUNT (MS.CUSTOMER_NUM) TotalConsumer  , SUM(MS.TOTAL_BILL_AMOUNT) TotalAmount, L.ZONE_CODE   ZoneCode,z.namebn ZoneNameBn, 
                                0 RecieptAmount
                                from MISCBILLAPP_MISC_BILL_MST MS
                                 LEFT JOIN MISCBILLAPP_MS_LOCATION L ON L.CODE=MS.LOCATION_CODE
                                left JOIN MISCBILLAPP_MS_ZONE Z on Z.CODE = l.Zone_code
                                WHERE   TO_CHAR(TO_DATE(MS.READING_DATE, 'DD-MON-RR'), 'YYYYMM')<:P_BILL_MONTH
                                 and ms.BILL_TYPE_CODE=:billTypeCode " + (isZone ? @$"and Z.CODE =:zoneCode " : " ") +
                                 @"
                                 GROUP BY 
                                 L.ZONE_CODE, z.namebn )X
                                 ON x.zonecode=y.zonecode ";
            var result = await con.QueryAsync<UntracedCustArrearMergeSummaryDto>(query1, new { zoneCode = zoneCode, P_BILL_MONTH = billMonth, billTypeCode = billTypeCode });
            return result.ToList();
        }

        #endregion Penalty Summary

        #region methods
        private async Task<List<Untraceable>> GetUCTraceDataWithZone(string zoneCode, string billMonth)
        {
            var isZone = zoneCode != "0" ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            string query1 = @$"SELECT UC.*, z.code ZONE_CODE, z.namebn ZONE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            where UC.STATUS=0 and UC.UC_TYPE = '0' AND (TO_CHAR(TO_DATE(UC.updated_date, 'DD-MON-RR'), 'YYYYMM')  <=:BILLMONTH OR  TO_CHAR(TO_DATE(UC.created_date, 'DD-MON-RR'), 'YYYYMM')  <=:BILLMONTH)   "
                            + (isZone ? @" and z.code=:zoneCode " : " ");
            var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, BILLMONTH = billMonth });
            return result.ToList();
        }

        private async Task<List<Untraceable>> GetUCTracePreMonthDataWithZone(string zoneCode, string billMonth)
        {
            var isZone = zoneCode != "0" ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            string query1 = @$"SELECT UC.*, z.code ZONE_CODE, z.namebn ZONE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            where UC.STATUS=0 and UC.UC_TYPE = '0' AND (TO_CHAR(TO_DATE(UC.updated_date, 'DD-MON-RR'), 'YYYYMM')  <:BILLMONTH OR  TO_CHAR(TO_DATE(UC.created_date, 'DD-MON-RR'), 'YYYYMM')  <:BILLMONTH) " + (isZone ? @" and z.code=:zoneCode " : " ");
            var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, BILLMONTH = billMonth });
            return result.ToList();
        }

        public async Task<List<Untraceable>> GetUCTraceDataCurrMonthWithZone(string zoneCode, string billMonth)
        {
            var isZone = zoneCode != "0" ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            string query1 = @$"SELECT UC.*, z.code ZONE_CODE, z.namebn ZONE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            where UC.STATUS=0 and UC.UC_TYPE = '0'  AND (TO_CHAR(TO_DATE(UC.updated_date, 'DD-MON-RR'), 'YYYYMM')  =:BILLMONTH OR  TO_CHAR(TO_DATE(UC.created_date, 'DD-MON-RR'), 'YYYYMM')  =:BILLMONTH) " + (isZone ? @" and z.code=:zoneCode " : " ");
            var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, BILLMONTH = billMonth });
            return result.ToList();
        }

        #endregion

        #endregion

        #region UntracedCustArrearDetails
        public async Task<Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>>> GetUntracedArrearDetails(string zoneCode, string circleCode, string locationCode, string billMonth)
		{
			var ucData = await GetUCTraceDataWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
			var ucPreMonthData = await GetUCTraceDataCurrMonthWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
			var ucCurrMonthData = await GetUCTracePreMonthDataWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
			var dbCodeList = ucData.Select(c => c.DB_CODE).Distinct().ToList();
			var allCustMergeList = new List<UntracedCustomerArrearModel>();
			var previousMergeList = new List<UntracedCustomerArrearModel>();
			var currMergeList = new List<UntracedCustomerArrearModel>();
			var customers = new List<UntracedCustomerArrearModel>();

			foreach (var dbCode in dbCodeList)
			{
				var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
				var dbWiseCustId = ucData.Where(c => c.DB_CODE.Equals(dbCode)).Select(s => s.CUST_ID).ToList();
				if (dbWiseCustId.Count > 0)
				{
					string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
					if (connectionString == null) throw new ArgumentNullException();

					//string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
					//string maxBillMonth = await GetMaxBillCycleCode(connectionString, dbCode, db.NAME);
					// maxBillMonth = "202302";


					using IDbConnection conn = new OracleConnection(connectionString);

					try
					{
						int arraySize = dbWiseCustId.Count;
						int chunkSize = 500;
						int skipChunkSize = 0;
						string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
						var convertBillMonth = DateTime.Parse(createDateString);
						string bill_cycle = billMonth;
						string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

						while (skipChunkSize < arraySize)
						{
							string query = " IH.CUST_ID ";
							string Bccc = " BCC.CUST_ID ";
							string _custId = GetChunkCustomers(dbWiseCustId.Skip(skipChunkSize).Take(chunkSize).ToArray());
							skipChunkSize += chunkSize;
							query = query + " in (" + _custId + @") ";
							Bccc = query + " in (" + _custId + @") ";

							var sQuery3 = @"
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

							var result = await conn.QueryAsync<UntracedCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_bill_cycleRECEIPT = bill_cycleRECEIPT });

							customers.AddRange(result.ToList());

						}

					}
					catch (Exception ex)
					{

						throw;
					}
				}

			}

			allCustMergeList.AddRange((from c in customers
										from cust in ucData
									   where c.CUST_ID == cust.CUST_ID
										select new UntracedCustomerArrearModel
										{
											CUST_ID = c.CUST_ID,
											CONSUMER_NO = c.CONSUMER_NO,
											LOCATION_CODE = c.LOCATION_CODE,
											PRN = c.PRN,
											LPS = c.LPS,
											VAT = c.VAT,
											TOTAL = c.TOTAL,
											RECEIVE_PRN = c.RECEIVE_PRN,
											RECEIVE_LPS = c.RECEIVE_LPS,
											RECEIVE_VAT = c.RECEIVE_VAT,
											RECEIVE_TOTAL = c.RECEIVE_TOTAL,
											CUSTOMER_NAME = c.CUSTOMER_NAME,
											ADDRESS = c.ADDRESS,
											BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
											ARREAR_AMT = c.ARREAR_AMT,
											RECEIPT_AMT = c.RECEIPT_AMT,
											CURR_BILL = c.CURR_BILL,
											PREV_MONTH = c.PREV_MONTH,
											LOCATION_DESC = c.LOCATION_DESC,
											ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
											ARREAR_LPS = c.ARREAR_LPS,
											ARREAR_VAT = c.ARREAR_VAT,
											CURR_PRINCIPAL = c.CURR_PRINCIPAL,
											CURR_LPS = c.CURR_LPS,
											CURR_VAT = c.CURR_VAT,
											TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
											TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
											TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
											TOTAL_AMOUNT = c.TOTAL_AMOUNT,
											TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
											TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
											ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
											FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
											CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
											CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
											CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
											ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
											ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
											ZONE_CODE = cust.ZONE_CODE,
											ZONE_NAMEBN = cust.ZONE_NAMEBN,
											LOCATION_NAMEBN = cust.LOCATION_NAMEBN,
											CIRCLE_CODE = cust.CIRCLE_CODE,
											CIRCLE_NAMEBN = cust.CIRCLE_NAMEBN
										}));

			currMergeList.AddRange((from c in customers
									from cust in ucCurrMonthData
									where c.CUST_ID == cust.CUST_ID
									select new UntracedCustomerArrearModel
									{
										CUST_ID = c.CUST_ID,
										CONSUMER_NO = c.CONSUMER_NO,
										LOCATION_CODE = c.LOCATION_CODE,
										PRN = c.PRN,
										LPS = c.LPS,
										VAT = c.VAT,
										TOTAL = c.TOTAL,
										RECEIVE_PRN = c.RECEIVE_PRN,
										RECEIVE_LPS = c.RECEIVE_LPS,
										RECEIVE_VAT = c.RECEIVE_VAT,
										RECEIVE_TOTAL = c.RECEIVE_TOTAL,
										CUSTOMER_NAME = c.CUSTOMER_NAME,
										ADDRESS = c.ADDRESS,
										BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
										ARREAR_AMT = c.ARREAR_AMT,
										RECEIPT_AMT = c.RECEIPT_AMT,
										CURR_BILL = c.CURR_BILL,
										PREV_MONTH = c.PREV_MONTH,
										LOCATION_DESC = c.LOCATION_DESC,
										ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
										ARREAR_LPS = c.ARREAR_LPS,
										ARREAR_VAT = c.ARREAR_VAT,
										CURR_PRINCIPAL = c.CURR_PRINCIPAL,
										CURR_LPS = c.CURR_LPS,
										CURR_VAT = c.CURR_VAT,
										TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
										TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
										TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
										TOTAL_AMOUNT = c.TOTAL_AMOUNT,
										TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
										TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
										ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
										FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
										CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
										CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
										CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
										ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
										ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
										ZONE_CODE = cust.ZONE_CODE,
										ZONE_NAMEBN = cust.ZONE_NAMEBN,
										LOCATION_NAMEBN= cust.LOCATION_NAMEBN,
										CIRCLE_CODE = cust.CIRCLE_CODE,
										CIRCLE_NAMEBN = cust.CIRCLE_NAMEBN
									}));

			previousMergeList.AddRange((from c in customers
										from cust in ucPreMonthData
										where c.CUST_ID == cust.CUST_ID
										select new UntracedCustomerArrearModel
										{
											CUST_ID = c.CUST_ID,
											CONSUMER_NO = c.CONSUMER_NO,
											LOCATION_CODE = c.LOCATION_CODE,
											PRN = c.PRN,
											LPS = c.LPS,
											VAT = c.VAT,
											TOTAL = c.TOTAL,
											RECEIVE_PRN = c.RECEIVE_PRN,
											RECEIVE_LPS = c.RECEIVE_LPS,
											RECEIVE_VAT = c.RECEIVE_VAT,
											RECEIVE_TOTAL = c.RECEIVE_TOTAL,
											CUSTOMER_NAME = c.CUSTOMER_NAME,
											ADDRESS = c.ADDRESS,
											BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
											ARREAR_AMT = c.ARREAR_AMT,
											RECEIPT_AMT = c.RECEIPT_AMT,
											CURR_BILL = c.CURR_BILL,
											PREV_MONTH = c.PREV_MONTH,
											LOCATION_DESC = c.LOCATION_DESC,
											ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
											ARREAR_LPS = c.ARREAR_LPS,
											ARREAR_VAT = c.ARREAR_VAT,
											CURR_PRINCIPAL = c.CURR_PRINCIPAL,
											CURR_LPS = c.CURR_LPS,
											CURR_VAT = c.CURR_VAT,
											TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
											TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
											TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
											TOTAL_AMOUNT = c.TOTAL_AMOUNT,
											TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
											TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
											ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
											FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
											CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
											CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
											CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
											ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
											ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
											ZONE_CODE = cust.ZONE_CODE,
											ZONE_NAMEBN = cust.ZONE_NAMEBN,
											LOCATION_NAMEBN = cust.LOCATION_NAMEBN,
											CIRCLE_CODE = cust.CIRCLE_CODE,
											CIRCLE_NAMEBN = cust.CIRCLE_NAMEBN
										}));

			return new Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>>(allCustMergeList, currMergeList, previousMergeList);

			
			
		}

		#region Methods
		private async Task<List<Untraceable>> GetUCTraceDataWithZoneCircleLocation(string zoneCode, string circleCode, string locationCode, string billMonth)
		{
			var isZone = zoneCode != "0" ? true : false;
			var isCircle = circleCode != "0" ? true : false;
			var isLocation = locationCode != "0" ? true : false;
			using var con = new OracleConnection(Connection.ConnectionString());
			string query1 = @$"SELECT UC.*,L.NAMEBN LOCATION_NAMEBN, z.code ZONE_CODE, z.namebn ZONE_NAMEBN, CIR.CODE CIRCLE_CODE, CIR.NAMEBN CIRCLE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            INNER JOIN MISCBILLAPP_MS_CIRCLE CIR ON CIR.CODE =  L.CIRCLE_CODE
                            " + (isZone ? @" where z.code=:zoneCode " : " ") + (isCircle ? @" and CIR.CODE=:circleCode " : " ") + (isLocation ? @" and L.CODE=:locationCode " : " ");
			var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, circleCode, locationCode, BILLMONTH=billMonth });
			return result.ToList();
		}
		private async Task<List<Untraceable>> GetUCTraceDataCurrMonthWithZoneCircleLocation(string zoneCode, string circleCode, string locationCode, string billMonth)
		{
			var isZone = zoneCode != "0" ? true : false;
			var isCircle = circleCode != "0" ? true : false;
			var isLocation = locationCode != "0" ? true : false;
			using var con = new OracleConnection(Connection.ConnectionString());
			string query1 = @$"SELECT UC.*,L.NAMEBN LOCATION_NAMEBN, z.code ZONE_CODE, z.namebn ZONE_NAMEBN, CIR.CODE CIRCLE_CODE, CIR.NAMEBN CIRCLE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            INNER JOIN MISCBILLAPP_MS_CIRCLE CIR ON CIR.CODE =  L.CIRCLE_CODE
                            where UC.STATUS=0 and UC.UC_TYPE = '0' AND (TO_CHAR(TO_DATE(UC.updated_date, 'DD-MON-RR'), 'YYYYMM')  <:BILLMONTH OR  TO_CHAR(TO_DATE(UC.created_date, 'DD-MON-RR'), 'YYYYMM')  <:BILLMONTH) " + (isZone ? @" and z.code=:zoneCode " : " ") +(isCircle ? @" and CIR.CODE=:circleCode " : " ") + (isLocation ? @" and L.CODE=:locationCode " : " ");
			var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, circleCode, locationCode, BILLMONTH = billMonth });
			return result.ToList();
		}

		public async Task<List<Untraceable>> GetUCTracePreMonthDataWithZoneCircleLocation(string zoneCode, string circleCode, string locationCode, string billMonth)
		{
			var isZone = zoneCode != "0" ? true : false;
			var isCircle = circleCode != "0" ? true : false;
			var isLocation = locationCode != "0" ? true : false;
			using var con = new OracleConnection(Connection.ConnectionString());
			string query1 = @$"SELECT UC.*,L.NAMEBN LOCATION_NAMEBN, z.code ZONE_CODE, z.namebn ZONE_NAMEBN, CIR.CODE CIRCLE_CODE, CIR.NAMEBN CIRCLE_NAMEBN FROM MISCBILLAPP_UC_CONSUMER UC 
                            INNER JOIN MISCBILLAPP_MS_LOCATION L ON UC.LOCATION_CODE = L.CODE
                            INNER JOIN MISCBILLAPP_MS_ZONE Z ON z.code =  L.ZONE_CODE
                            INNER JOIN MISCBILLAPP_MS_CIRCLE CIR ON CIR.CODE =  L.CIRCLE_CODE
                            where UC.STATUS=0 and UC.UC_TYPE = '0'  AND (TO_CHAR(TO_DATE(UC.updated_date, 'DD-MON-RR'), 'YYYYMM')  =:BILLMONTH OR  TO_CHAR(TO_DATE(UC.created_date, 'DD-MON-RR'), 'YYYYMM')  =:BILLMONTH) " + (isZone ? @" and z.code=:zoneCode " : " ") + (isCircle ? @" and CIR.CODE=:circleCode " : " ") + (isLocation ? @" and L.CODE=:locationCode " : " ");
			var result = await con.QueryAsync<Untraceable>(query1, new { zoneCode, circleCode, locationCode, BILLMONTH = billMonth });
			return result.ToList();
		}

        public async Task<List<UntracebleCustArrearDetailsReportDTO>> GetUntraceableArrearDetails(string zoneCode, string circleCode, string locationCode, string billMonth)
        {
            var ucData = await GetUCTraceDataWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
            var ucPreMonthData = await GetUCTraceDataCurrMonthWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
            var ucCurrMonthData = await GetUCTracePreMonthDataWithZoneCircleLocation(zoneCode, circleCode, locationCode, billMonth);
            var dbCodeList = ucData.Select(c => c.DB_CODE).Distinct().ToList();
            var allCustMergeList = new List<UntracebleCustArrearDetailsReportDTO>();
            //var previousMergeList = new List<UntracedCustomerArrearDetailsModel>();
            //var currMergeList = new List<UntracedCustomerArrearDetailsModel>();
            var customers = new List<UntracedCustomerArrearDetailsModel>();

            foreach (var dbCode in dbCodeList)
            {
                var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
                var dbWiseCustId = ucData.Where(c => c.DB_CODE.Equals(dbCode)).Select(s => s.CUST_ID).ToList();
                if (dbWiseCustId.Count > 0)
                {
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    if (connectionString == null) throw new ArgumentNullException();

                    //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
                    //string maxBillMonth = await GetMaxBillCycleCode(connectionString, dbCode, db.NAME);
                    // maxBillMonth = "202302";


                    using IDbConnection conn = new OracleConnection(connectionString);

                    try
                    {
                        int arraySize = dbWiseCustId.Count;
                        int chunkSize = 500;
                        int skipChunkSize = 0;
                        string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
                        var convertBillMonth = DateTime.Parse(createDateString);
                        string bill_cycle = billMonth;
                        string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

                        while (skipChunkSize < arraySize)
                        {
                            string query = " IH.CUST_ID ";
                            string Bccc = " BCC.CUST_ID ";
                            string _custId = GetChunkCustomers(dbWiseCustId.Skip(skipChunkSize).Take(chunkSize).ToArray());
                            skipChunkSize += chunkSize;
                            query = query + " in (" + _custId + @") ";
                            Bccc = query + " in (" + _custId + @") ";

                            var sQuery3 = @"
                                    SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,nvl(A.LPS,0) ARREAR_LPS,nvl(A.VAT,0) ARREAR_VAT,
                                           
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,nvl(B.LPS,0) CURR_LPS,nvl(B.VAT,0) CURR_VAT,
                                           
                                           nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0) AS TOTAL_PRINCIPAL_ARREAR,
                                           nvl(a.LPS,0) + nvl(B.LPS,0) AS TOTAL_LPS_ARREAR,
                                           nvl(a.VAT,0) +nvl( B.VAT,0)  AS TOTAL_VAT_ARREAR,
                                           
                                           ( nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0)+nvl(a.LPS,0) +nvl( B.LPS,0)+nvl(a.VAT,0) +nvl( B.VAT,0)) TOTAL_AMOUNT,
    
                                            NVL(C.RECEIPT_AMT,0) TOTAL_RECEIPT_PRINCIPAL,NVL(C.VAT_AMT,0) TOTAL_RECEIPT_VAT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.CURR_RECEIPT_AMT)
                                       ELSE 0 
                                             END AS ARREAR_RECEIPT_AMOUNT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (B.CURR_RECEIPT_AMT)
                                       ELSE (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) 
                                             END AS CURR_RECEIPT_AMT
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

                                               SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                      SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT  

                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE <:P_BILL_CYCLE_CODE AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)) PRINCIPAL,
                                                       SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)) LPS,
                                                       SUM( nvl(IH.VAT_AMT,0)+nvl(IH.VAT_ADJ,0)) VAT,
                                                       sum(NVL(IH.INVOICE_AMT,0)) CURR_RECEIPT_AMT

                                                FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE <= :P_BILL_CYCLE_CODE AND  " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID";

                            var result = await conn.QueryAsync<UntracedCustomerArrearDetailsModel>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_bill_cycleRECEIPT = bill_cycleRECEIPT });

                            customers.AddRange(result.ToList());

                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

            }

            allCustMergeList.AddRange((from c in customers
                                       from cust in ucData
                                       where c.CUST_ID == cust.CUST_ID
                                       select new UntracebleCustArrearDetailsReportDTO
                                       {
                                           CUST_ID = c.CUST_ID,
                                           CONSUMER_NO = c.CONSUMER_NO,
                                           LOCATION_CODE = c.LOCATION_CODE,
                                           PRN = c.PRN,
                                           LPS = c.LPS,
                                           VAT = c.VAT,
                                           TOTAL = c.TOTAL,
                                           RECEIVE_PRN = c.RECEIVE_PRN,
                                           RECEIVE_LPS = c.RECEIVE_LPS,
                                           RECEIVE_VAT = c.RECEIVE_VAT,
                                           RECEIVE_TOTAL = c.RECEIVE_TOTAL,
                                           CUSTOMER_NAME = c.CUSTOMER_NAME,
                                           ADDRESS = c.ADDRESS,
                                           BILL_CYCLE_CODE = c.BILL_CYCLE_CODE,
                                           ARREAR_AMT = c.ARREAR_AMT,
                                           RECEIPT_AMT = c.RECEIPT_AMT,
                                           CURR_BILL = c.CURR_BILL,
                                           PREV_MONTH = c.PREV_MONTH,
                                           LOCATION_DESC = c.LOCATION_DESC,
                                           ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                           ARREAR_LPS = c.ARREAR_LPS,
                                           ARREAR_VAT = c.ARREAR_VAT,
                                           CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                           CURR_LPS = c.CURR_LPS,
                                           CURR_VAT = c.CURR_VAT,
                                           TOTAL_PRINCIPAL_ARREAR = c.TOTAL_PRINCIPAL_ARREAR,
                                           TOTAL_LPS_ARREAR = c.TOTAL_LPS_ARREAR,
                                           TOTAL_VAT_ARREAR = c.TOTAL_VAT_ARREAR,
                                           TOTAL_AMOUNT = c.TOTAL_AMOUNT,
                                           TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                           TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                           ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                           FINANCIAL_AMOUNT = c.FINANCIAL_AMOUNT,
                                           CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                           CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                           CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                           ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
                                           ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
                                           ZoneCode = cust.ZONE_CODE,
                                           //ZONE_NAMEBN = cust.ZONE_NAMEBN,
                                           LOCATIONNAMEBN = cust.LOCATION_NAMEBN,
                                           CircleCode = cust.CIRCLE_CODE,
                                           CircleNameBn = cust.CIRCLE_NAMEBN,
                                           LocationCode = cust.LOCATION_CODE,
                                       }));        

            return new List<UntracebleCustArrearDetailsReportDTO>(allCustMergeList);


        }
        #endregion

        #endregion
    }
}
 