using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Castle.Core.Resource;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.NonBengali;
using Core.Domain.APA;
using Core.Domain.NonBengali;
using Core.Domain.UnionPorishad;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.NonBengali
{
    public class NonBengaliRepository : INonBengaliRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        public NonBengaliRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
        }

        public async Task<List<NonBengaliDTOs>> GetNonBengaliConsumerByDate(string validDate,string reportType)
        {
            List<NonBengaliDTOs> nonBengaliDTOs = new List<NonBengaliDTOs>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
             oracleDynamicParameters.Add("report_date", value: validDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<NonBengalis>("MISC_REPORT_NON_BANGALI_PKG.NON_BANGALI_COUNSUMER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            nonBengaliDTOs = _mapper.Map(result.ToList(), nonBengaliDTOs);
            return nonBengaliDTOs;
        }

        public async Task<List<NonBengaliSummaryDTO>> GetNonBengaliConsumerSummaryByDate(string validDate, string reportType)
        {
            List<NonBengaliSummaryDTO> nonBengaliSummaryDTOs = new List<NonBengaliSummaryDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("report_date", value: validDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<NonBengaliSummary>("MISC_REPORT_NON_BANGALI_PKG.NON_BANGALI_COUNSUMER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            nonBengaliSummaryDTOs = _mapper.Map(result.ToList(), nonBengaliSummaryDTOs);
            return nonBengaliSummaryDTOs;
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

        public async Task<List<NonBengaliCustId>> GetNonBengaliCustId(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"
                            select 
                         CID.CUST_ID, 
                         c.NON_BENGALI_CAMP_CODE,
                         c.ZONE_CODE, 
                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                         (select NAMEBN from MISCBILLAPP_GC_NON_BENGALI where CODE = c.NON_BENGALI_CAMP_CODE) NONBENGALI_NAMEBN,
                         (select NAME from MISCBILLAPP_GC_NON_BENGALI where CODE = c.NON_BENGALI_CAMP_CODE) NONBENGALI_NAME,
                         (select NAMEBN from MISCBILLAPP_MS_LOCATION  where CODE = c.location_code)  LOCATION_NAMEBN
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.NON_BENGALI_CAMP_CODE is not null " + (isZone ? @" and c.ZONE_CODE =:zone_code" : @" ") + (isLocationCode ? @" and c.Location_code =:loction_code " : " ");

            var result = await con.QueryAsync<NonBengaliCustId>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();
        }


        public async Task<List<NonBengaliSummaryDTO>> GetNonBengaliStaticValue(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            bool isLocation = locationCode != "0" ? isZone ? true : false : false;
            List<NonBengaliSummaryDTO> nonBengaliSummaryDTOs = new List<NonBengaliSummaryDTO>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            var result = await con.QueryAsync<NonBengaliSummary>(@"  
                  SELECT  
                   B.NON_BANGLI_NAME_CODE, 
                   B.NAMEBN,B.LOCATION_CODE,
                   B.METER_COUNT, 
                   NB.PRINCIPAL PRN,
                   NB.LPS LPS,
                   NB.TOTAL_ARREAR,
                   B.LOCATION_NAMEBN ,
                   1 IS_SATATIC ,
                  B.ZONE_CODE,
                    (select MAX(BILL_CYCLE_CODE)   FROM MISCBILLAPP_GC_NONB_ARREAR) BILL_CYCLE_CODE
                   FROM(   SELECT NON_BANGLI_NAME_CODE, NAMEBN,LOCATION_CODE,METER_COUNT
                        , (SELECT NAMEBN  FROM MISCBILLAPP_MS_LOCATION WHERE CODE =NON_BANGALI.LOCATION_CODE ) LOCATION_NAMEBN ,
                        (SELECT ZONE_CODE FROM MISCBILLAPP_MS_LOCATION WHERE CODE =NON_BANGALI.LOCATION_CODE ) ZONE_CODE
                       FROM  (Select
                        NB.CODE NON_BANGLI_NAME_CODE
                        ,NB.NAMEBN 
                        ,C.LOCATION_CODE
                        ,COUNT(C.CUSTOMER_NO) METER_COUNT
                        ,SUM(NVL(CA.PRINCIPAL, 0) + NVL(CA.ARREAR_PRINCIPAL, 0)) PRN
                        ,SUM(NVL(CA.CURR_LPS, 0) + NVL(CA.ARREAR_LPS, 0)) LPS,
                        SUM((NVL(CA.PRINCIPAL, 0 ) + NVL(CA.CURR_LPS, 0) + NVL(CA.ARREAR_LPS,0) + NVL(CA.ARREAR_PRINCIPAL, 0)))  TOTAL_ARREAR
                        from
                        MISCBILLAPP_GC_NON_BENGALI NB
                        LEFT  JOIN MISCBILLAPP_GC_CUSTOMER C ON  C.NON_BENGALI_CAMP_CODE = NB.CODE  
                        LEFT JOIN MISCBILLAPP_GC_NONB_ARREAR CA ON CA.CUSTOMER_NO = C.CUSTOMER_NO 
                        AND TO_CHAR(CA.CURR_DATE, 'DD-MON-RR') = '01-MAY-24' 
                        GROUP BY
                        NB.CODE
                        , NB.NAMEBN
                        , C.LOCATION_CODE
                      
                        ORDER BY NB.CODE) NON_BANGALI ) B
                        INNER JOIN  MISCBILLAPP_GC_NON_BENGALI NB ON NB.CODE = B.NON_BANGLI_NAME_CODE " + (isZone ? @" where  B.ZONE_CODE =:zoneCode " : " ") + (isLocation ? @" AND B.LOCATION_CODE =:locationCode " : " ")
                         , new { zoneCode = zoneCode, locationCode = locationCode });
            nonBengaliSummaryDTOs = _mapper.Map(result.ToList(), nonBengaliSummaryDTOs);
            return nonBengaliSummaryDTOs;
        }
        public async Task<List<OnlineNonBengaliSummaryMergeDTO>> GetOnlineNonBengaliConsumerDataByDate(string zoneCode, string locationCode, string billMonth, string reportType)
        {
            using var conb = new OracleConnection(Connection.ConnectionString());
            var allDatabase = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<OnlineNonBengaliSummary> nonBengali = new List<OnlineNonBengaliSummary>();
            var bengaliCustId = await GetNonBengaliCustId(zoneCode, locationCode);
            var margeList = new List<OnlineNonBengaliSummaryMergeDTO>();
            string maxBillMonth = billMonth;
            string createDateString = maxBillMonth.Substring(maxBillMonth.Length - 2) + "/" + "01/" + maxBillMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycle = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            string privious_bill_cycle = convertBillMonth.AddMonths(-2).ToString("yyyyMM");
            string fqyery = $"SELECT START_BILLCYCLE,END_BILLCYCLE FROM GC_FINANCIAL_YEAR1 WHERE TO_DATE({maxBillMonth}, 'YYYYMM') BETWEEN TO_DATE(START_BILLCYCLE, 'YYYYMM') AND TO_DATE(END_BILLCYCLE, 'YYYYMM')";
            var financialYear = await conb.QueryFirstOrDefaultAsync<FinancialYearModel>(fqyery);
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
                                          -- COUNT(X.CUSTOMER_NUM) METER_COUNT,
                                           X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                           C.RECEIPT_AMT CURR_RECEIPT_PRINCIPAL,
                                           C.VAT_AMT CURR_RECEIPT_VAT,
                                           nvl(A.PRINCIPAL,0) ARREAR_PRICIPAL,nvl(A.LPS,0) ARREAR_LPS,nvl(A.VAT,0) ARREAR_VAT,
                                           
                                           nvl(B.PRINCIPAL,0) CURR_PRINCIPAL,nvl(B.LPS,0) CURR_LPS,nvl(B.VAT,0) CURR_VAT,
                                           
                                           nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0) AS TOTAL_PRINCIPAL_ARREAR,
                                           nvl(a.LPS,0) + nvl(B.LPS,0) AS TOTAL_LPS_ARREAR,
                                           nvl(a.VAT,0) +nvl( B.VAT,0)  AS TOTAL_VAT_ARREAR,
                                           
                                           ( nvl(a.PRINCIPAL,0) +nvl( B.PRINCIPAL,0)+nvl(a.LPS,0) +nvl( B.LPS,0)+nvl(a.VAT,0) +nvl( B.VAT,0)) TOTAL_AMOUNT,
    
                                            NVL(C.RECEIPT_AMT,0) TOTAL_RECEIPT_PRINCIPAL,NVL(C.VAT_AMT,0) TOTAL_RECEIPT_VAT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.PRINCIPAL+B.VAT+B.LPS) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.PRINCIPAL+B.VAT+B.LPS)
                                       ELSE 0 
                                             END AS ARREAR_RECEIPT_AMOUNT,

                                       B.CURR_RECEIPT_AMT,
                                            F.FINANCIAL_AMOUNT
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
                                                    WHERE  (BILL_CYCLE_CODE BETWEEN '201604' AND :p_privious_bill_cycle) AND    ((IH.PRINCIPAL_AMT+IH.PRINCIPAL_ADJ-IH.PRINCIPAL_APPL) + (IH.LPS_AMT+IH.LPS_ADJ-IH.LPS_APPL)+
                                                    (IH.VAT_AMT+IH.VAT_ADJ-VAT_APPL)) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                       SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                       SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT,
                                                       SUM(NVL(IH.PRINCIPAL_APPL,0) + NVL(IH.LPS_APPL,0)+NVL(VAT_APPL,0)) CURR_RECEIPT_AMT

                                                FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE = :P_BILL_CYCLE_CODE AND  " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT 

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE IH.BILL_CYCLE_CODE =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID
                                      LEFT JOIN 

                                                 (SELECT R.CUST_ID,(SUM(NVL(R.RECEIPT_AMT, 0))+ SUM(NVL(R.VAT_AMT, 0))) FINANCIAL_AMOUNT 
            
                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                WHERE (IH.BILL_CYCLE_CODE between :P_START_BILLCYCLE AND :P_END_BILLCYCLE) AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"
                                                 GROUP BY R.CUST_ID

                                                ) F ON X.CUST_ID = F.CUST_ID"
                            ;
                            var result = await conn.QueryAsync<OnlineNonBengaliSummary>(sQuery3, new { P_BILL_CYCLE_CODE = bill_cycle, p_bill_cycleRECEIPT = billMonth, P_START_BILLCYCLE = financialYear.START_BILLCYCLE, P_END_BILLCYCLE = financialYear.END_BILLCYCLE, p_privious_bill_cycle = privious_bill_cycle });
                            nonBengali.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }

            margeList.AddRange(from c in nonBengali
                               from cust in bengaliCustId
                               where c.CUST_ID == cust.CUST_ID
                               select new OnlineNonBengaliSummaryMergeDTO
                               {
                                   ConsumerNo = c.CONSUMER_NO,
                                   CustomerName = c.CUSTOMER_NAME,
                                   Address = c.ADDRESS,
                                   LocationCode = c.LOCATION_CODE,
                                   LocationDesc = c.LOCATION_DESC,
                                   LocationNameBn = cust.LOCATION_NAMEBN,

                                   MeterCount = c.METER_COUNT,
                                   BillCycleCode = c.BILL_CYCLE_CODE,
                                   ZoneName = cust.ZONE_NAME,

                                   CurrLps = c.TOTAL_LPS_ARREAR,
                                   CurrPrin = c.TOTAL_PRINCIPAL_ARREAR,
                                   CurrVat = c.TOTAL_VAT_ARREAR,

                                   ArrearLps = c.ARREAR_LPS,
                                   ArrearPrincipal = c.ARREAR_PRICIPAL,
                                   ArrearVat = c.ARREAR_VAT,

                                   TotalArrearAmount = c.TOTAL_AMOUNT,
                                   TotalReceiptPrincipal = c.TOTAL_RECEIPT_PRINCIPAL,
                                   TotalReceiptVat = c.TOTAL_RECEIPT_VAT,
                                   TotalAmount = c.TOTAL_AMOUNT,

                                   //NonbengaliName = cust.NONBENGALI_NAME,
                                   NonBengaliNameBn = cust.NONBENGALI_NAMEBN,

                                   NonBengaliCampCode = cust.NON_BENGALI_CAMP_CODE,


                                   CurrReceiptAmt = c.CURR_RECEIPT_AMT,
                                   CurrReceiptPrincipal = c.CURR_RECEIPT_PRINCIPAL,
                                   CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                   ZoneCode = cust.ZONE_CODE
                               });
            return margeList;
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
