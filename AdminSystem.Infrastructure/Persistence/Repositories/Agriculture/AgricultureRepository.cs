
using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Domain.Agricultures;
using Core.Domain.UnionPorishad;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Agriculture;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection = AdminSystem.Infrastructure.Persistence.Context.Connection;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Agriculture
{
    public class AgricultureRepository : IAgricultureRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;

        public AgricultureRepository(DapperContext context,IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _mapper = mapper;
            _commRepo = commRepo;
            _dbConfigRepo = dbConfigRepo;
        }

        #region methods
        public Task<int> AddAsync(AgricultureDto entity) => throw new NotImplementedException();
        public Task<int> AddListAsync(List<AgricultureDto> entity) => throw new NotImplementedException();

        public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

        public Task<AgricultureDto> GetByIdAsync(int id) => throw new NotImplementedException();

        public Task<int> GetTotalCountAsync(string searchBy) => throw new NotImplementedException();

        public Task<int> UpdateAsync(AgricultureDto entity) => throw new NotImplementedException();

        public Task<List<AgricultureDto>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }
        #endregion methods

        #region agriculture and poultry
        //Agriculture And Poultry Both Data
        public async Task<List<AgricultureDto>> GetAllAsyncByDate(string validDate)
        {
            List<AgricultureDto> agricultureDtos = new List<AgricultureDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("report_date", value: validDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.Agricultures.Agriculture>("MISC_REPORT_AGRICULTURE_PKG.GET_ALL_POUL_AGRI_CON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            agricultureDtos = _mapper.Map(result.ToList(), agricultureDtos);
            return agricultureDtos;
        }
        #endregion agriculture and poultry

        #region agriculture
        public async Task<List<AgricultureDto>> GetAllAgricultureAsyncByDate(string validDate)
        {
            List<AgricultureDto> agricultureDtos = new List<AgricultureDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("report_date", value: validDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.Agricultures.Agriculture>("MISC_REPORT_AGRICULTURE_PKG.GET_AGRICULTURE_CON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            agricultureDtos = _mapper.Map(result.ToList(), agricultureDtos);
            return agricultureDtos;
        }

        #endregion agriculture

        #region poultry
        public async Task<List<AgricultureDto>> GetAllPoultryAsyncByDate(string validDate)
        {
            List<AgricultureDto> agricultureDtos = new List<AgricultureDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("report_date", value: validDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.Agricultures.Agriculture>("MISC_REPORT_AGRICULTURE_PKG.GET_ALL_POULTY_CON_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            agricultureDtos = _mapper.Map(result.ToList(), agricultureDtos);
            return agricultureDtos;
        }

        #endregion  poultry

        #region Online Agriculture

        public async Task<List<OnlineAgricultureMergeDTO>> GetOnlineAgriPoultryAndMinistryData(string billMonth, string zoneCode, string locationCode, string reportType)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<AgricultureArrearModel> customers = new List<AgricultureArrearModel>();
            var agriCustId = new List<AgricultureCustIdModel>();
            if (reportType == "1")
            {
                agriCustId = await GetMinistryWiseAgriCustId(zoneCode, locationCode);
            }
            else if (reportType == "2")
            {
                agriCustId = await GetKrishiPoultryWiseKrishiPoultryCustId(zoneCode, locationCode);
            }
            else if (reportType == "3")
            {
                agriCustId = await GetKrishiWiseAgriCustId(zoneCode, locationCode);
            }
            else if (reportType == "4")
            {
                agriCustId = await GetPoultryWisePoultryCustId(zoneCode, locationCode);
            }
            var mergeList = new List<OnlineAgricultureMergeDTO>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            foreach (var database in allDatabases)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

                try
                {

                    int[] custIds;
                    if (reportType == "1")
                    {
                        custIds = await GetNonMinistryCustLocationDesc(database.CODE);
                    }
                    else 
                    {
                        custIds = await GetNonCustLocationDesc(database.CODE);
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

                            //if (database.CODE == "3" || database.CODE == "5" || database.CODE == "6" || database.CODE == "7")
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
                                           NVL(D.Total_Receipt_Arrear,0) Total_Receipt_Arrear,
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
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) +SUM(NVL(R.VAT_AMT, 0)) Total_Receipt_Arrear,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL  AND " + query + @"GROUP BY R.CUST_ID
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
                                           NVL(D.Total_Receipt_Arrear,0) Total_Receipt_Arrear,
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
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) +SUM(NVL(R.VAT_AMT, 0)) Total_Receipt_Arrear,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            }
                            var result = await conn.QueryAsync<AgricultureArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth, P_BILL_CYCLE_CODES = bill_cycleRECEIPT });
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

                               from cust in agriCustId
                               where c.CUST_ID == cust.CUST_ID
                               select new OnlineAgricultureMergeDTO
                               {
                                   ConsumerNo = c.CONSUMER_NO,
                                   CustomerName = c.CUSTOMER_NAME,
                                   //Address = c.ADDRESS,
                                   CustId = cust.CUST_ID,
                                   KrishiDepartmentCode = cust.KRISHI_DEPARTMRNT_CODE,
                                   MinistryCode = cust.MINISTRY_CODE,
                                   KrishiDeptNameBn = cust.KRISHI_DEPT_NAME_BN,
                                   LocationNameBn = cust.LOCATION_NAME_BN,
                                   LocationName = cust.LOCATION_NAME,
                                   IsKrishi = cust.IS_KRISHI,
                                   IsPoultry = cust.IS_POULTRY,
                                   ConExtgNum = cust.CON_EXTG_NUM,

                                   CurrLps = c.CURR_LPS,
                                   CurrPrincipal = c.CURR_PRINCIPAL,
                                   CurrVat = c.CURR_VAT,

                                   ArrearLps = c.ARREAR_LPS,
                                   ArrearPrincipal = c.ARREAR_PRICIPAL,
                                   ArrearVat = c.ARREAR_VAT,

                                   //TotalArrearAmount = c.TOTAL_AMOUNT,
                                   //TotalReceiptPrincipal = c.TOTAL_RECEIPT_PRINCIPAL,
                                   //TotalReceiptVat = c.TOTAL_RECEIPT_VAT,

                                   //UnionPorishadName = cust.UNIONPORISHAD_NAME,
                                   //UnionPorishadNameBn = cust.UNIONPORISHAD_NAMEBN,

                                   //UnionPorishadCode = cust.UNIONPARISHAD_CODE,


                                   //CurrReceiptAmt = c.CURR_RECEIPT_AMT,
                                   CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                   CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                   CurrTotalBill = c.CURR_TOTAL_BILL,
                                   TotalArrear = c.TOTAL_ARREAR,
                                   TotalReceiptArrear = c.Total_Receipt_Arrear,
                                   ZoneCode = cust.ZONE_CODE,
                                   //ZoneName = cust.ZONE_NAME,
                                   LocationCode = c.LOCATION_CODE,
                                   //LocationNameBn = cust.LOCATION_NAMEBN,
                                   //ReceiptBillMonth = c.RECEIPT_BILLMONTH,
                                   //TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR

                               }));

            return mergeList;
        }

        #endregion Online Agriculture

        #region Online Agriculture and Poultry Ledger
        public async Task<List<OnlineAgricultureLedgerMergeDTO>> GetOnlineAgriAndPoultryLedger(string startMonth, string endMonth, string zoneCode, string locationCode)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<OnlineAgricultureLedgerModel> customers = new List<OnlineAgricultureLedgerModel>();
            var agriCustId = await GetKrishiPoultryWiseLedgerCustId(zoneCode, locationCode);
            var mergeList = new List<OnlineAgricultureLedgerMergeDTO>();
            foreach (var database in allDatabases)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

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

                            //if (database.CODE == "3" || database.CODE == "5" || database.CODE == "6" || database.CODE == "7")
                            if (database.CODE == "3")
                            {
                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_START_MONTH START_MONTH,
                                           :P_END_MONTH END_MONTH,
                                           X.ADDRESS,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           NVL(D.Total_Receipt_Arrear,0) Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_START_MONTH START_MONTH, :P_END_MONTH END_MONTH,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
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
                                                    WHERE  IH.bill_cycle_code BETWEEN :P_START_MONTH AND :P_END_MONTH  AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) +SUM(NVL(R.VAT_AMT, 0)) Total_Receipt_Arrear,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') BETWEEN :P_START_MONTH AND :P_END_MONTH AND R.SEQ_NUM IS NOT NULL  AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC";
                            }
                            else
                            {


                                sQuery3 = @"
                                     Select * from ( SELECT X.CUST_ID AS CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_START_MONTH START_MONTH,
                                           :P_END_MONTH END_MONTH,
                                           X.ADDRESS,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,
                                           nvl(A.LPS,0) ARREAR_LPS,
                                           nvl(A.VAT,0) ARREAR_VAT,
                                           NVL(D.Total_Receipt_Arrear,0) Total_Receipt_Arrear,
                                           D.RECEIPT_AMT ARREAR_RECEIPT_PRINCIPAL,
                                           D.VAT_AMT ARREAR_RECEIPT_VAT
                                        FROM 
                                            (
                                               SELECT IH.CUST_ID,IH.CUSTOMER_NUM , IH.CHECK_DIGIT,IH.CUSTOMER_NAME,BCLM.LOCATION_CODE ,BCLM.DESCR LOCATION_DESC,
                                                      :P_START_MONTH START_MONTH, :P_END_MONTH END_MONTH,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS 
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
                                                    WHERE  IH.bill_cycle_code BETWEEN :P_START_MONTH AND :P_END_MONTH AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) +SUM(NVL(R.VAT_AMT, 0)) Total_Receipt_Arrear,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') BETWEEN :P_START_MONTH AND :P_END_MONTH AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC";
                            }
                            var result = await conn.QueryAsync<OnlineAgricultureLedgerModel>(sQuery3, new { P_START_MONTH = startMonth, P_END_MONTH = endMonth});
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

                                from cust in agriCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineAgricultureLedgerMergeDTO
                                {
                                   ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    //Address = c.ADDRESS,
                                    CustId = cust.CUST_ID,
                                    KrishiDepartmentCode = cust.KRISHI_DEPARTMRNT_CODE,
                                    MinistryCode = cust.MINISTRY_CODE,
                                    KrishiDeptNameBn = cust.KRISHI_DEPT_NAME_BN,
                                    LocationNameBn = cust.LOCATION_NAME_BN,
                                    LocationName = cust.LOCATION_NAME,
                                    IsKrishi = cust.IS_KRISHI,
                                    IsPoultry = cust.IS_POULTRY,
                                    ConExtgNum = cust.CON_EXTG_NUM,

                                    CurrLps = c.CURR_LPS,
                                    CurrPrincipal = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,

                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat = c.ARREAR_VAT,
                                    CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    TotalReceiptArrear = c.Total_Receipt_Arrear,
                                   
                                    ZoneCode = cust.ZONE_CODE,
                                    LocationCode = c.LOCATION_CODE,
                                    ArrearReceiptPrincipal = c.ARREAR_RECEIPT_PRINCIPAL,
                                    ArrearReceiptVat = c.ARREAR_RECEIPT_VAT
                                }));

            return mergeList;
        }

        #endregion Online Agriculture and Poultry Ledger

        private async Task<int[]> GetNonCustLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<AgricultureCustIdModel> result = new List<AgricultureCustIdModel>();
            var sQuery = @"
                         select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO and c.ministry_code='1'
                        where c.KRISHI_DEPARTMRNT_CODE is not null and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;

        }

        private async Task<int[]> GetNonMinistryCustLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<AgricultureCustIdModel> result = new List<AgricultureCustIdModel>();
            var sQuery = @"
                         select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.MINISTRY_CODE = '1' and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;

        }

        private async Task<List<AgricultureCustIdModel>> GetKrishiWiseAgriCustId(string zoneCode, string locationCode) 
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"
                          SELECT 
                                CID.CUST_ID CUST_ID,
                                C.KRISHI_DEPARTMRNT_CODE KRISHI_DEPARTMRNT_CODE,
                                KD.NAMEBN KRISHI_DEPT_NAME_BN,
                                (SELECT Z.CODE FROM MISCBILLAPP_MS_ZONE Z WHERE C.ZONE_CODE = Z.CODE) ZONE_CODE,
                                (SELECT L.CODE FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_CODE,
                                (SELECT L.NAME FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME,
                                (SELECT L.NAMEBN FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME_BN,
                                C.IS_KRISHI IS_KRISHI,
                                C.CON_EXTG_NUM CON_EXTG_NUM
                          FROM MISCBILLAPP_GC_KRISHI_DEPT KD
                          INNER JOIN MISCBILLAPP_GC_CUSTOMER C ON C.KRISHI_DEPARTMRNT_CODE = KD.CODE AND c.IS_KRISHI = 1
                          INNER JOIN MISCBILLAPP_GC_CUSTID CID ON CID.CONSUMER_NO = C.CUSTOMER_NO
                          WHERE C.KRISHI_DEPARTMRNT_CODE IS NOT NULL  AND ('{zoneCode}'='0' OR C.zone_code='{zoneCode}') AND ('{locationCode}'='0' OR C.LOCATION_CODE='{locationCode}')";
            var result = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode});
            return result.ToList();
        }

        private async Task<List<AgricultureCustIdModel>> GetPoultryWisePoultryCustId(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"
                          SELECT 
                                CID.CUST_ID CUST_ID,
                                C.KRISHI_DEPARTMRNT_CODE KRISHI_DEPARTMRNT_CODE,
                                KD.NAMEBN KRISHI_DEPT_NAME_BN,
                                (SELECT Z.CODE FROM MISCBILLAPP_MS_ZONE Z WHERE C.ZONE_CODE = Z.CODE) ZONE_CODE,
                                (SELECT L.CODE FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_CODE,
                                (SELECT L.NAME FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME,
                                (SELECT L.NAMEBN FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME_BN,
                                C.IS_POULTRY IS_POULTRY,
                                C.CON_EXTG_NUM CON_EXTG_NUM
                          FROM MISCBILLAPP_GC_KRISHI_DEPT KD
                          INNER JOIN MISCBILLAPP_GC_CUSTOMER C ON C.KRISHI_DEPARTMRNT_CODE = KD.CODE AND c.IS_POULTRY = 1
                          INNER JOIN MISCBILLAPP_GC_CUSTID CID ON CID.CONSUMER_NO = C.CUSTOMER_NO
                          WHERE C.KRISHI_DEPARTMRNT_CODE IS NOT NULL  AND ('{zoneCode}'='0' OR C.zone_code='{zoneCode}') AND ('{locationCode}'='0' OR C.LOCATION_CODE='{locationCode}')";
            var result = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();
        }

        private async Task<List<AgricultureCustIdModel>> GetKrishiPoultryWiseKrishiPoultryCustId(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"
                          SELECT 
                                CID.CUST_ID CUST_ID,
                                C.KRISHI_DEPARTMRNT_CODE KRISHI_DEPARTMRNT_CODE,
                                KD.NAMEBN KRISHI_DEPT_NAME_BN,
                                (SELECT Z.CODE FROM MISCBILLAPP_MS_ZONE Z WHERE C.ZONE_CODE = Z.CODE) ZONE_CODE,
                                (SELECT L.CODE FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_CODE,
                                (SELECT L.NAME FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME,
                                (SELECT L.NAMEBN FROM MISCBILLAPP_MS_LOCATION L WHERE C.LOCATION_CODE = L.CODE) LOCATION_NAME_BN,
                                C.IS_KRISHI IS_KRISHI,
                                C.IS_POULTRY IS_POULTRY,
                                C.CON_EXTG_NUM CON_EXTG_NUM
                          FROM MISCBILLAPP_GC_KRISHI_DEPT KD
                          INNER JOIN MISCBILLAPP_GC_CUSTOMER C ON C.KRISHI_DEPARTMRNT_CODE = KD.CODE
                          INNER JOIN MISCBILLAPP_GC_CUSTID CID ON CID.CONSUMER_NO = C.CUSTOMER_NO
                          WHERE C.KRISHI_DEPARTMRNT_CODE IS NOT NULL AND ('{zoneCode}'='0' OR C.zone_code='{zoneCode}') AND ('{locationCode}'='0' OR C.LOCATION_CODE='{locationCode}')";
            var result = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();
        }


        private async Task<List<AgricultureCustIdModel>> GetMinistryWiseAgriCustId(string zoneCode, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"SELECT 
                              CID.CUST_ID,C.CUSTOMER_NO,C.MINISTRY_CODE,Z.CODE ZONE_CODE, L.CODE LOCATION_CODE, L.NAME LOCATION_NAME, L.NAMEBN LOCATION_NAME_BN
                              FROM MISCBILLAPP_GC_CUSTID CID
                              INNER JOIN MISCBILLAPP_GC_CUSTOMER C ON C.CUSTOMER_NO = CID.CONSUMER_NO 
                              INNER JOIN MISCBILLAPP_MS_ZONE Z ON Z.CODE=C.ZONE_CODE
                              INNER JOIN MISCBILLAPP_MS_LOCATION L ON L.CODE =C.LOCATION_CODE
                              AND C.MINISTRY_CODE = '1'
                              WHERE c.is_krishi IS NULL
                              AND c.is_poultry IS NULL AND ('{zoneCode}'='0' OR C.zone_code='{zoneCode}') AND ('{locationCode}'='0' OR C.LOCATION_CODE='{locationCode}')";
            var result = await con.QueryAsync<AgricultureCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();
        }

        private async Task<List<OnlineAgricultureLedgerCustIdModel>> GetKrishiPoultryWiseLedgerCustId(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"
                          SELECT 
                                CID.CUST_ID CUST_ID,
                                C.KRISHI_DEPARTMRNT_CODE KRISHI_DEPARTMRNT_CODE,
                                KD.NAMEBN KRISHI_DEPT_NAME_BN,
                                C.ZONE_CODE ZONE_CODE,
                                C.LOCATION_CODE LOCATION_CODE,
                                L.NAME LOCATION_NAME,
                                L.NAMEBN LOCATION_NAME_BN,
                                C.IS_KRISHI IS_KRISHI,
                                C.IS_POULTRY IS_POULTRY,
                                C.CON_EXTG_NUM CON_EXTG_NUM
                          FROM MISCBILLAPP_GC_KRISHI_DEPT KD
                          INNER JOIN MISCBILLAPP_GC_CUSTOMER C ON C.KRISHI_DEPARTMRNT_CODE = KD.CODE
                          INNER JOIN MISCBILLAPP_GC_CUSTID CID ON CID.CONSUMER_NO = C.CUSTOMER_NO
                          INNER JOIN MISCBILLAPP_MS_ZONE Z ON Z.CODE = C.ZONE_CODE
                          INNER JOIN MISCBILLAPP_MS_LOCATION L ON L.CODE = C.LOCATION_CODE
                          WHERE C.KRISHI_DEPARTMRNT_CODE IS NOT NULL 
                          AND ('{zoneCode}'='0' OR C.zone_code='{zoneCode}') AND ('{locationCode}'='0' OR C.LOCATION_CODE='{locationCode}')";
            var result = await con.QueryAsync<OnlineAgricultureLedgerCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();
        }


        public string GetChunkCustomers(int[] customers)
        {
            string _customers = string.Join(", ", customers);

            if (_customers.Length > 0)
                _customers = "" + _customers + "";

            return _customers;
        }


    }
}
