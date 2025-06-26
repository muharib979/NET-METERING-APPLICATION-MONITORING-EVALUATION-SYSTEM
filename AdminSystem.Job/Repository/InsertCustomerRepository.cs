using AdminSystem.Job.Interface;
using AdminSystem.Job.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace AdminSystem.Job.Repository
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


        public InsertCustomerRepository(IConfiguration config, IDapperService dapperService, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _config = config;
            _dapperService = dapperService;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
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

                result.AddRange(await GetCustomerArrearDataAync(dbCode,custId));
            }
            return result;
        }

        private async Task<List<InsertCustomerArrearModel>> GetCustomerArrearDataAync(string dbCode,int?[] custId)
        {
            DatabaseConfig db = (DatabaseConfig)await _dbConfigRepo.GetDatabaseConfigByIDAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
           // string connectionString = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.18.94)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = ebc; Password = ebc";
            string maxBillMonth = await GetMaxBillCycleCode(connectionString);
            using IDbConnection conn = new OracleConnection(connectionString);
            var customers = new List<InsertCustomerArrearModel>();
            try
            {

                int arraySize = custId.Length;
                int chunkSize = 500;
                int skipChunkSize = 0;
                string createDateString = maxBillMonth.Substring(maxBillMonth.Length - 2)+ "/"  + "01/" +   maxBillMonth.Substring(0, 4);
                var convertBillMonth = DateTime.Parse(createDateString);
                string bill_cycle = convertBillMonth.AddMonths(-1).ToString("yyyyMM");

                while (skipChunkSize < arraySize)
                {
                    string query = " c.cust_id ";

                    string _custId = GetChunkCustomers(custId.Skip(skipChunkSize).Take(chunkSize).ToArray());
                    skipChunkSize += chunkSize;

                    query = query + " in (" + _custId + @") ";

                    var sQuery3 = @"select c.cust_id,
                                (c.customer_num || c.check_digit) CONSUMER_NO
                                , G.LOCATION_CODE
                                ,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) PRN,
                        sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS,
                        sum(VAT_AMT+VAT_ADJ-VAT_APPL) VAT,
                        SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL+LPS_AMT+LPS_ADJ-LPS_APPL+VAT_AMT+VAT_ADJ-VAT_APPL) TOTAL,
                        0 RECEIVE_PRN,
                        0 RECEIVE_LPS,
                        0 RECEIVE_VAT,
                        0 RECEIVE_TOTAL,
                        0 RECEIPT_AMT,
                        0 PREV_MONTH,
                        SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL+LPS_AMT+LPS_ADJ-LPS_APPL+VAT_AMT+VAT_ADJ-VAT_APPL) CURR_BILL,
                        SUM(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL+LPS_AMT+LPS_ADJ-LPS_APPL+VAT_AMT+VAT_ADJ-VAT_APPL) ARREAR_AMT,
                        :BillMonth BILL_CYCLE_CODE,
                        (ca.addr_descr1 ||  ca.addr_descr2 || ca.addr_descr3)  address,
                        c.customer_name CUSTOMER_NAME
                        from ebc.bc_invoice_hdr h
                        join ebc.bc_customers c on h.cust_id = c.cust_id
                        join ebc.bc_customer_addr ca on ca.cust_id = c.cust_id
                        join ebc.bc_location_master G ON G.LOCATION_CODE = C.LOCATION_CODE
                        where ca.addr_type = 'B'
                        and ca.addr_exp_date IS NULL
                            and  
                        bill_cycle_code <=:BillMonth  AND " +
                        query
                        + @" group by    G.LOCATION_CODE, 
                        c.cust_id,c.customer_num , c.check_digit,c.customer_name 
                        , ca.addr_descr1 , ca.addr_descr2 , ca.addr_descr3  ";

                    var result = await conn.QueryAsync<InsertCustomerArrearModel>(sQuery3, new { BillMonth = bill_cycle });

                    customers.AddRange(result.ToList());

                }
                return customers;

            }
            catch (Exception ex)
            {
                using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
                {
                    writer.WriteLine($"error:  connection problem");
                }
                throw;
            }
        }

        public async Task<int> InsertCustomerArrearDataAsync(List<InsertCustomerArrearModel> customerArrear)
        {
            int inserted;
            var currentDate = DateTime.Now.ToString("dd-MMM-y");
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using (IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var sQuery =
                        @$"INSERT INTO MISCBILLAPP_GC_CUSTOMER_ARR
                        (CUSTOMER_NO, NAME, LOCATION_CODE, LOCATION_DESC, PRINCIPAL, CURR_VAT, 
                        CURR_LPS, CURR_DATE, RUN_BILL_CYCLE_CODE, BILL_CYCLE_CODE, CURR_BILL, PREV_MONTH, 
                        RECEIPT_AMT, LOCATION_DESCBN, ARREAR_AMT, ADDRESS) VALUES
                        (:CONSUMER_NO, :CUSTOMER_NAME, :LOCATION_CODE,NULL, :PRN, :VAT, :LPS, " + @$" TO_DATE('" + $"{DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss")}" + "', 'YYYY-MM-DD HH24:MI:SS')," +
                        @" NULL, :BILL_CYCLE_CODE, :CURR_BILL, :PREV_MONTH, :RECEIPT_AMT, N'', :ARREAR_AMT, :ADDRESS )";


                        inserted = await _dapperService.ExecuteAsyncWithDapperAndTransaction(sQuery, customerArrear, conn, transaction);
                        if (inserted > 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
                        {
                            writer.WriteLine($"error:  {ex}");
                        }

                        throw;
                    }
                }
            }

            return inserted;
        }


        public async Task<string> GetMaxBillCycleCode(string connectionString)
        {
            try
            {
                using IDbConnection conn = new OracleConnection(connectionString);

                var sQuery = @" select max(bill_cycle_code) from bc_invoice_hdr ";
                var result = await conn.QueryAsync<string>(sQuery);
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
                using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
                {
                    writer.WriteLine($"error:  connection problem");
                }
                throw;
            }

        }

        public async Task<int> InsertCustIdDataAsync(List<CustIdInsertModel> customerArrear)
        {
            int inserted;
            var currentDate = DateTime.Now.ToString("dd-MMM-y");
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using (IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var sQuery =
                        @$"INSERT INTO MISCBILLAPP_GC_CUSTID
                        (CONSUMER_NO, CUST_ID) VALUES
                        (:CONSUMER_NO, :CUST_ID)";


                        inserted = await _dapperService.ExecuteAsyncWithDapperAndTransaction(sQuery, customerArrear, conn, transaction);
                        if (inserted > 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
                        {
                            writer.WriteLine($"error:  {ex}");
                        }

                        throw;
                    }
                }
            }

            return inserted;
        }

        #endregion


        #region Common Utility Section


        private async Task<int?[]> GetAllCustomerCustomerNo(string dbCode)
        {
            using IDbConnection conn = Connection;
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
            using IDbConnection conn = Connection;
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
    }
}
