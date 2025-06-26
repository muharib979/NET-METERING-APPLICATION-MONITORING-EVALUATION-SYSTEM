using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.ZoneCircle;
using Core.Domain.Ministry;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Fire_service
{
    public class FireServiceRepository : IFireServiceRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        private readonly IZoneCircleRepository _zoneRepo;

        public FireServiceRepository(DapperContext context, IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo, IZoneCircleRepository zoneRepo)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
            _zoneRepo = zoneRepo;
        }

        private async Task<List<PublicSecurityDivisionCustId>> GetSecurityserviceDivisionCustId(string zoneCode, string locationCode)
        {

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
                           WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
                           (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (@" AND C.MINISTRY_CODE = '17' ") + @" ORDER BY ORDERNO ASC";
            var result = await con.QueryAsync<PublicSecurityDivisionCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode });
            return result.ToList();

        }
        private async Task<int[]> GetSecurityserviceDivisionCustIdlocwise(string zoneCode, string locationCode)
        {

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
                           WHERE C.MINISTRY_CODE IS NOT NULL" + (isZone ? @" AND C.ZONE_CODE = :zone_code" : @" ") +
                           (isLocation ? @" AND C.LOCATION_CODE = :location_code" : @" ") + (@" AND C.MINISTRY_CODE = '17' ") + @" ORDER BY ORDERNO ASC";
            var result = await con.QueryAsync<PublicSecurityDivisionCustId>(sQuery, new { zone_code = zoneCode, location_code = locationCode });
            //return result.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
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
        public async Task<List<PublicSecurityDivisionMergeDTO>> OnlineSecurityServiceDivArrearDetails(string billMonth, string zoneCode, string locationCode, string reportType)
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
            List<PublicSecurityDivisionDetails> customers = new List<PublicSecurityDivisionDetails>();
            var ministryCustId = await GetSecurityserviceDivisionCustId(zoneCode, locationCode);
            var mergeList = new List<PublicSecurityDivisionMergeDTO>();

            foreach (var database in allDatabase)
            {
                var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by Id
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database
                try
                {
                    int[] custIds;
                    if (zoneCode != "0")
                    {
                        custIds = await GetSecurityserviceDivisionCustIdlocwise(zoneCode, locationCode);

                    }
                    else
                    {
                        custIds = await GetSecurityserviceDivisionCustIdlocwise(zoneCode, locationCode);

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
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND " + query + @"GROUP BY R.CUST_ID
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
                            var result = await conn.QueryAsync<PublicSecurityDivisionDetails>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                            customers.AddRange(result.ToList());

                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
                mergeList.AddRange((from c in customers

                                    from cust in ministryCustId
                                    where c.CUST_ID == cust.CUST_ID
                                    select new PublicSecurityDivisionMergeDTO
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


            }


            return mergeList;
        }
    }
}
