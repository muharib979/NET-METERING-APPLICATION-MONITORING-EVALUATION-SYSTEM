using Dapper;
using Dapper.Oracle;
using MinistryData.WorkerService.Misc.Interface;
using MinistryData.WorkerService.Misc.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace MinistryData.WorkerService.Misc.Repository
{

    public interface IInsertCustomerRepository
    {
        //Task<List<InsertCustomerArrearModel>> GetAllCustomerDataAsync(string dbCode);
        Task<int> InsertCustomerArrearDataAsync(List<InsertCustomerArrearModel> customerArrear);
        Task<List<CustIdInsertModel>> GetAllCustIdDataAsync(string criteria, string dbCode);

        Task<int> InsertCustIdDataAsync(List<CustIdInsertModel> customerArrear);
        Task<List<InsertCustomerArrearModel>> GetCustomerArrearDataAync();
    }


    public class InsertCustomerRepository : IInsertCustomerRepository
    {
        private readonly IConfiguration _config;
        private readonly IDapperService _dapperService;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        private readonly ILogger<InsertCustomerRepository> _logger;


        public InsertCustomerRepository(IConfiguration config, IDapperService dapperService, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo, ILogger<InsertCustomerRepository> logger)
        {
            _config = config;
            _dapperService = dapperService;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
            _logger = logger;
        }

        private OracleConnection Connection
        {
            get
            {
                return new OracleConnection(_config.GetConnectionString("ConnectionString"));
            }
        }



        #region MISCBILLAPP_GC_CUSTOMER_ARREAR update

        //public async Task<List<InsertCustomerArrearModel>> GetAllCustomerDataAsync(string dbCode)
        //{
        //    var custId = await GetAllCustId(dbCode);
        //    var result = new List<InsertCustomerArrearModel>();
        //    if (custId.Count() != 0)
        //    {

        //        result.AddRange(await GetCustomerArrearDataAync(dbCode, custId));
        //    }
        //    return result;
        //}

        public async Task<List<InsertCustomerArrearModel>> GetCustomerArrearDataAync()
        {
            using var conb = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            var allDatabases = await _dbConfigRepo.GetAllActiveDatabaseConfigAsync();
            var ministryCustId = await GetMinistryArrearDetailsCustIdWithMinistry();
            List<InsertCustomerArrearModel> customers = new List<InsertCustomerArrearModel>();
            var mergeList = new List<InsertCustomerArrearModel>();
            try
            {
                foreach (var database in allDatabases)
                {
                    var db = await _dbConfigRepo.GetDatabaseConfigByIDAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                                                                                                                                            //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
                    string maxBillMonth = await GetMaxBillCycleCode(connectionString, database.CODE, db.NAME);
                   // maxBillMonth = "202311";
                    using var con = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
                    string dQuery = $"DELETE FROM MISCBILLAPP_GC_CUSTOMER_ARR WHERE BILL_CYCLE_CODE= '{maxBillMonth}' AND DB_CODE='{database.CODE}' ";
                    var results = con.Execute(dQuery);
                    try
                    {
                        int?[] custIds;
                       
                         custIds = await GetAllCustIdWithoutreligiousandnonMinistry(database.CODE, "0");

                        
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
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN') AND " + query + @"GROUP BY R.CUST_ID
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
                                var result = await conn.QueryAsync<InsertCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = maxBillMonth, p_bill_cycleRECEIPT = maxBillMonth, DB_CODE = database.CODE });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

                foreach (var database in allDatabases)
                {
                    var db = await _dbConfigRepo.GetDatabaseConfigByIDAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    string maxBillMonth = await GetMaxBillCycleCode(connectionString, database.CODE, db.NAME);
                  //  maxBillMonth = "202311";

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
                                var result = await conn.QueryAsync<InsertCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = maxBillMonth, DB_CODE = database.CODE });
                                customers.AddRange(result.ToList());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                foreach (var database in allDatabases)
                {
                    var db = await _dbConfigRepo.GetDatabaseConfigByIDAsync(database.CODE); // Getting database info by Id
                    string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                    string maxBillMonth = await GetMaxBillCycleCode(connectionString, database.CODE, db.NAME);
                    //maxBillMonth = "202311";
                    using var con = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());

                    string religiousQuery = $@"SELECT X.*,'{database.CODE}' DB_CODE,  0 CURR_RECEIPT_PRINCIPAL,0 CURR_RECEIPT_VAT ,0 ARREAR_LPS,0 CURR_PRINCIPAL,
                                        0 CURR_LPS ,0 CURR_VAT ,0 Total_Receipt_Arrear,0 ARREAR_RECEIPT_PRINCIPAL,  0 ARREAR_RECEIPT_VAT, '{maxBillMonth}' BILL_CYCLE_CODE from 
                                        (select ci.cust_id CUST_ID, C.CUSTOMER_NO CONSUMER_NO , c.NAME CUSTOMER_NAME , c.LOCATION_CODE , L.NAME  LOCATION_DESC,
                                        C.ADDRESS ,SUM(PRINCIPAL+DEMAND_CHARGE+SERVICE_CHARGE) ARREAR_PRICIPAL,
                                        SUM(VAT_AMOUNT)  ARREAR_VAT from MISCBILLAPP_GC_CUSTOMER C 
                                        inner join MISCBILLAPP_GC_CUSTID CI on c.customer_no=CI.CONSUMER_NO 
                                        inner join MISCBILLAPP_MS_LOCATION L on L.CODE=c.location_code 
                                        inner join RELIGIOUS_ARREAR R on R.CUSTOMER_NO=c.customer_no 
                                        where R.BILL_CYCLE_CODE BETWEEN '202307' AND '{maxBillMonth}' and c.ministry_code='29' 
                                        and C.DB_CODE='{database.CODE}' AND c.religious_code='1'
                                        group by ci.CUST_ID,C.CUSTOMER_NO,C.NAME,C.LOCATION_CODE,L.NAME,C.ADDRESS) X";

                    var results = await con.QueryAsync<InsertCustomerArrearModel>(religiousQuery);
                    customers.AddRange(results.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            mergeList.AddRange((from c in customers

                                from cust in ministryCustId
                                where c.CUST_ID == cust.CUST_ID && c.DB_CODE == cust.DB_CODE
                                orderby cust.ORDERNO ascending
                                select new InsertCustomerArrearModel
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    LOCATION_DESC = c.LOCATION_DESC,
                                    BILL_CYCLE_CODE= c.BILL_CYCLE_CODE,
                                    ADDRESS= c.ADDRESS,
                                    RECEIPT_BILLMONTH= c.RECEIPT_BILLMONTH,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_LPS = c.CURR_LPS,
                                    DB_CODE= c.DB_CODE,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS = c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT = c.ARREAR_VAT,
                                    Total_Receipt_Arrear = c.Total_Receipt_Arrear,
                                    ARREAR_RECEIPT_PRINCIPAL = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ARREAR_RECEIPT_VAT = c.ARREAR_RECEIPT_VAT,
                                    
                                }));

            return mergeList;
        }

        public async Task<int> InsertCustomerArrearDataAsync(List<InsertCustomerArrearModel> customerArrear)
        {
            int inserted;
            var currentDate = DateTime.Now.ToString("dd -MMM-y");
            using var conn = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                var sQuery = $@" INSERT INTO MISCBILLAPP_GC_CUSTOMER_ARR (CUST_ID,
         CONSUMER_NO, 
         CUSTOMER_NAME, 
         LOCATION_CODE, 
         LOCATION_DESC, 
         ADDRESS,
         CURR_RECEIPT_PRINCIPAL,
         CURR_RECEIPT_VAT,
         ARREAR_PRICIPAL,
         ARREAR_LPS,
         ARREAR_VAT,
         CURR_PRINCIPAL,
         CURR_LPS,
         CURR_VAT,
         TOTAL_RECEIPT_ARREAR,
         ARREAR_RECEIPT_PRINCIPAL,
         ARREAR_RECEIPT_VAT,
          CURR_DATE,
           RECEIPT_BILLMONTH,
            BILL_CYCLE_CODE,
            DB_CODE
                          ) 
                          VALUES
                (:CUST_ID,
                 :CONSUMER_NO,
                  :CUSTOMER_NAME,
                   :LOCATION_CODE,
                    :LOCATION_DESC,
                     :ADDRESS, 
                     :CURR_RECEIPT_PRINCIPAL, 
                     :CURR_RECEIPT_VAT, 
                     :ARREAR_PRICIPAL, 
                     :ARREAR_LPS, 
                     :ARREAR_VAT, 
                     :CURR_PRINCIPAL, 
                     :CURR_LPS, 
                     :CURR_VAT, 
                     :TOTAL_RECEIPT_ARREAR, 
                     :ARREAR_RECEIPT_PRINCIPAL, 
                     :ARREAR_RECEIPT_VAT, 
                     " + @$" TO_DATE('" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "', 'YYYY-MM-DD HH24:MI:SS')," + @" 
                      :RECEIPT_BILLMONTH, 
                      :BILL_CYCLE_CODE,:DB_CODE
                                 )";
                inserted = conn.Execute(sQuery, customerArrear, transaction: tran);
                if (inserted > 0)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }



            return inserted;
        }


        public async Task<string> GetMaxBillCycleCode(string connectionString, string dbCode, string dbName)
        {
            try
            {
                //DateTime date=DateTime.Now;
                var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
                var endDate = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
                //string startDate=date.ToString("")
                using var dbConn = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());

                string inserQuery = $@"INSERT INTO MISCBILLAPP_GC_DATA_STATUS (DB_CODE, DB_NAME, START_DATE, END_DATE, IS_SUCCESS) VALUES
                                        (:dbCode,:dbName," + $" TO_DATE( '" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + $" TO_DATE( '" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @"0)";

                dbConn.Execute(inserQuery, new { dbCode, dbName });
                using var conn = new OracleConnection(connectionString);

                var sQuery = @" select max(bill_cycle_code) from bc_invoice_hdr";
                var result = await conn.QueryAsync<string>(sQuery);
                string maxId = "SELECT MAX(ID) FROM MISCBILLAPP_GC_DATA_STATUS";
                int Id = dbConn.QueryFirstOrDefault<int>(maxId);
                string updatedQuery = $"UPDATE MISCBILLAPP_GC_DATA_STATUS SET  IS_SUCCESS=1 WHERE ID={Id}";
                dbConn.Execute(updatedQuery);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        #region MISCBILLAPP_GC_CUSTID update

        public async Task<List<CustIdInsertModel>> GetAllCustIdDataAsync(string criteria, string dbCode)
        {
            var customer = await GetAllCustomerCustomerNo(dbCode);
            var result = new List<CustIdInsertModel>();
            if (customer.Count() != 0)
            {

                result.AddRange(await GetCustIdDataAync(dbCode, criteria, customer));
            }
            return result;

        }

        private async Task<List<CustIdInsertModel>> GetCustIdDataAync(string dbCode, string billMonth, int?[] customers)
        {
            DatabaseConfig db = (DatabaseConfig)await _dbConfigRepo.GetDatabaseConfigByIDAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
            //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
            using IDbConnection conn = new OracleConnection(connectionString);

            var custIds = new List<CustIdInsertModel>();
            try
            {

                int arraySize = customers.Length;
                int chunkSize = 500;
                int skipChunkSize = 0;


                while (skipChunkSize < arraySize)
                {
                    string query = "(c.customer_num || c.check_digit) ";

                    string _customers = GetChunkCustomers(customers.Skip(skipChunkSize).Take(chunkSize).ToArray());
                    skipChunkSize += chunkSize;

                    query = query + " in (" + _customers + @")";

                    var sQuery3 = @"select (c.customer_num || c.check_digit) CONSUMER_NO
                                ,c.cust_id
                                FROM ebc.bc_customers c 
                                where  " + query + @" ";

                    var result = await conn.QueryAsync<CustIdInsertModel>(sQuery3);

                    custIds.AddRange(result.ToList());

                }

                return custIds;

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex} Connection Problem.");
                throw;
            }

        }

        public async Task<int> InsertCustIdDataAsync(List<CustIdInsertModel> customerArrear)
        {
            int inserted;
            var currentDate = DateTime.Now.ToString("dd-MMM-y");
            using var conn = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                var sQuery =
                @$"INSERT INTO MISCBILLAPP_GC_CUSTID
                        (CONSUMER_NO, CUST_ID) VALUES
                        (:CONSUMER_NO, :CUST_ID)";


                inserted = conn.Execute(sQuery, customerArrear, transaction: tran);
                if (inserted > 0)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();

                throw;
            }



            return inserted;
        }

        #endregion


        #region Common Utility Section


        private async Task<int?[]> GetAllCustomerCustomerNo(string dbCode)
        {
            using var conn = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            List<CustomerModel> result = new List<CustomerModel>();
            try
            {
                var sQuery = @" Select C.CUSTOMER_NO from MISCBILLAPP_GC_CUSTOMER C where C.CUSTOMER_NO > 0 AND C.DB_CODE =:dbcode";
                var resultt = conn.Query<CustomerModel>(sQuery, new { dbcode = dbCode });
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

        private async Task<int?[]> GetAllCustId(string dbCode)
        {
            using var conn = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {
                var sQuery = @" Select ci.cust_id from MISCBILLAPP_GC_CUSTOMER C 
                                inner join miscbillapp_gc_custid ci on c.customer_no = ci.consumer_no 
                                where  ci.cust_id > 0 and C.DB_CODE =:dbcode";
                var resultt = conn.Query<CustIdInsertModel>(sQuery, new { dbcode = dbCode });
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

        private async Task<List<MinistryCustomer>> GetMinistryArrearDetailsCustIdWithMinistry()
        {
            using var con = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
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
                           WHERE C.MINISTRY_CODE IS NOT NULL";
            var result = await con.QueryAsync<MinistryCustomer>(sQuery);
            return result.ToList();
        }
        private async Task<int?[]> GetAllCustIdWithoutreligiousandnonMinistry(string dbCode, string ministryCode, bool isRebate = false)
        {
            var rebateCode = isRebate == true ? "1" : null;
            using var con = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
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

                var resultt = await con.QueryAsync<CustIdInsertModel>("MISC_REPORT_MINISTRY_PKG.CUSTID_WITHOUT_RELIGIOUS", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
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
        private async Task<int?[]> GetNonBengaliLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(MinistryData.WorkerService.Misc.DbContext.Connection.ConnectionString());
            List<NonBengaliCustId> result = new List<NonBengaliCustId>();
            var sQuery = @" select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.NON_BENGALI_CAMP_CODE is not null and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<NonBengaliCustId>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int?[] custs = custIdArray;
            return custs;

        }
    }
}
