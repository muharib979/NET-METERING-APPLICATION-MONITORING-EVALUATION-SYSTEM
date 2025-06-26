using Dapper;
using NonBengeliMisc.Interface;
using NonBengeliMisc.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace NonBengeliMisc.Repository
{
    public interface IInsertCustomerRepository
    {
        Task<List<InsertCustomerArrearModel>> GetAllCustomerDataAsync(string dbCode);
        Task<int> InsertCustomerArrearDataAsync(List<InsertCustomerArrearModel> customerArrear);
        Task<List<CustIdInsertModel>> GetAllCustIdDataAsync(string criteria, string dbCode);

        Task<int> InsertCustIdDataAsync(List<CustIdInsertModel> customerArrear);
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

        public async Task<List<InsertCustomerArrearModel>> GetAllCustomerDataAsync(string dbCode)
        {
            var custId = await GetAllCustId(dbCode);
            var result = new List<InsertCustomerArrearModel>();
            if (custId.Count() != 0)
            {

                result.AddRange(await GetCustomerArrearDataAync(dbCode, custId));
            }
            return result;
        }

        private async Task<List<InsertCustomerArrearModel>> GetCustomerArrearDataAync(string dbCode, int?[] custId)
        {

            using var conb = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString()); 
            DatabaseConfig db = (DatabaseConfig)await _dbConfigRepo.GetDatabaseConfigByIDAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                                                                                                                                    //string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
            string maxBillMonth = await GetMaxBillCycleCode(connectionString, dbCode, db.NAME);
            maxBillMonth = "202310";
            using var con = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            string dQuery = $"DELETE FROM MISCBILLAPP_GC_CUSTNB_ARR WHERE BILL_CYCLE_CODE= '{maxBillMonth}' AND DB_CODE='{dbCode}' ";
            var results = con.Execute(dQuery);
            using IDbConnection conn = new OracleConnection(connectionString);
            var customers = new List<InsertCustomerArrearModel>();
            try
            {
                int arraySize = custId.Length;
                int chunkSize = 500;
                int skipChunkSize = 0;
                string createDateString = maxBillMonth.Substring(maxBillMonth.Length - 2) + "/" + "01/" + maxBillMonth.Substring(0, 4);
                var convertBillMonth = DateTime.Parse(createDateString);
                string bill_cycle = maxBillMonth;
                string privious_bill_cycle = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
                while (skipChunkSize < arraySize)
                {
                    string query = " IH.CUST_ID ";
                    string _custId = GetChunkCustomers(custId.Skip(skipChunkSize).Take(chunkSize).ToArray());
                    skipChunkSize += chunkSize;
                    query = query + " in (" + _custId + @") ";
                    string sQuery3;

                        sQuery3 = @"
                                     SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :DB_CODE DB_CODE,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           0 CURR_RECEIPT_PRINCIPAL,
                                           0 CURR_RECEIPT_VAT,
                                           0 ARREAR_PRICIPAL,0 ARREAR_LPS,0 ARREAR_VAT,
                                           
                                           nvl(A.PRINCIPAL,0) CURR_PRINCIPAL,nvl(A.LPS,0) CURR_LPS,nvl(A.VAT,0) CURR_VAT,
                                           
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
                    

                    var result = await conn.QueryAsync<InsertCustomerArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_privious_bill_cycle = privious_bill_cycle, DB_CODE = dbCode });

                    customers.AddRange(result.ToList());

                }
                return customers;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> InsertCustomerArrearDataAsync(List<InsertCustomerArrearModel> customerArrear)
        {
            int inserted;
            var currentDate = DateTime.Now.ToString("dd -MMM-y");
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                var sQuery = $@" INSERT INTO MISCBILLAPP_GC_CUSTNB_ARR (CUST_ID,
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
                using var dbConn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());

                string inserQuery = $@"INSERT INTO MISCBILLAPP_GC_DATA_STATUS (DB_CODE, DB_NAME, START_DATE, END_DATE, IS_SUCCESS) VALUES
                                        (:dbCode,:dbName," + $" TO_DATE( '" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + $" TO_DATE( '" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @"0)";

                dbConn.Execute(inserQuery, new { dbCode, dbName });
                using var conn = new OracleConnection(connectionString);

                var sQuery = @" select max(bill_cycle_code) from bc_invoice_hdr ";
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
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
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
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
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
            using var conn = new OracleConnection(NonBengeliMisc.DbContext.Connection.ConnectionString());
            List<CustIdInsertModel> result = new List<CustIdInsertModel>();
            try
            {
                var sQuery = @" Select ci.cust_id from MISCBILLAPP_GC_CUSTOMER C 
                                inner join miscbillapp_gc_custid ci on c.customer_no = ci.consumer_no 
                                where  ci.cust_id > 0 and C.DB_CODE =:dbcode AND C.NON_BENGALI_CAMP_CODE IS NOT NULL";
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
    }
}
