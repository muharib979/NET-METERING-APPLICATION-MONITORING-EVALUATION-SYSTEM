using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.UnionPorishod;
using Core.Domain.CityCorporation;
using Core.Domain.NonBengali;
using Core.Domain.UnionPorishad;
using Core.Domain.ZoneCircle;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.MinistryCustomer;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.UnionPorishodRepository
{
    public class UnionPorishodRepository : IUnionPorishodRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;

        public UnionPorishodRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _mapper = mapper;
            _commRepo= commRepo;
            _dbConfigRepo = dbConfigRepo;
        }
        public async Task<List<ZoneWiseUnionPorishodDto>> GetUnionPorishodbyDate(string zoneCode, string circleCode, string locationCode, string billMonth, string reportType)
        {
            List<ZoneWiseUnionPorishodDto> unionParishodDTO = new List<ZoneWiseUnionPorishodDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_MONTH", value: billMonth.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //oracleDynamicParameters.Add("P_IS_ZONE_CODE", value: isZoneCode, dbType: (OracleMappingType?)OracleDbType.Int16, direction: ParameterDirection.Input);
            //oracleDynamicParameters.Add("P_IS_LOCATION_CODE", value: isLocationCode, dbType: (OracleMappingType?)OracleDbType.Int16, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ZoneWiseUnionPorishodDto>("MISC_REPORT_UNION_PORISHOD_PKG.UNION_PORISHOD_DATA_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            unionParishodDTO = _mapper.Map(result.ToList(), unionParishodDTO);
            return unionParishodDTO;
        }

     

        #region online UnionPorishad
        public async Task<List<OnlineUnionPorisadMergeDataDto>> GetOnlineUnionPorishod(string zoneCode, string locationCode, string billMonth, string reportType)
    {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorpoartionAndPouroCustId(zoneCode, locationCode);
            List<OnlineUnionPorisadMergeDataDto>? mergeList = new List<OnlineUnionPorisadMergeDataDto>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
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

                            var result = await conn.QueryAsync<CityCorporationArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth });
                            customers.AddRange(result.ToList());

                        }

                    }

                }
                catch (Exception)
                {
                    continue;
                }
            }

            mergeList.AddRange(from c in customers

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineUnionPorisadMergeDataDto
                                {
                                    ConsumerNo = c.CONSUMER_NO,
                                    CustomerName = c.CUSTOMER_NAME,
                                    Address = c.ADDRESS,

                                    CurrLps = c.CURR_LPS,
                                    CurrPrin = c.CURR_PRINCIPAL,
                                    CurrVat = c.CURR_VAT,

                                    ArrearLps = c.ARREAR_LPS,
                                    ArrearPrincipal = c.ARREAR_PRICIPAL,
                                    ArrearVat= c.ARREAR_VAT,
                                    
                                    TotalArrearAmount = c.TOTAL_AMOUNT,
                                    TotalReceiptPrincipal = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TotalReceiptVat = c.TOTAL_RECEIPT_VAT,

                                    UnionPorishadName = cust.UNIONPORISHAD_NAME,
                                    UnionPorishadNameBn = cust.UNIONPORISHAD_NAMEBN,
                                    
                                    UnionPorishadCode= cust.UNIONPARISHAD_CODE,


                                    CurrReceiptAmt = c.CURR_RECEIPT_AMT,
                                    CurrReceiptPrincipal= c.CURR_RECEIPT_PRINCIPAL,
                                    CurrReceiptVat = c.CURR_RECEIPT_VAT,
                                    ZoneCode = cust.ZONE_CODE,
                                    ZoneName = cust.ZONE_NAME,
                                    LocationCode = c.LOCATION_CODE,
                                    LocationNameBn= cust.LOCATION_NAMEBN,
                                    ReceiptBillMonth = c.RECEIPT_BILLMONTH,
                                    TotalReceiptArrear = c.TOTAL_RECEIPT_ARREAR

                                });

            return mergeList;
        }

        private async Task<int[]> GetNonCustLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<UnionPorishadCustIdModel> result = new List<UnionPorishadCustIdModel>();
            var sQuery = @" select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.unionparishad_code is not null and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<UnionPorishadCustIdModel>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;

        }
        private async Task<List<UnionPorishadCustIdModel>> GetCityCorpoartionAndPouroCustId(string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"
                            select 
                         CID.CUST_ID, 
                         c.unionparishad_code,
                         c.ZONE_CODE, 
                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                         (select NAMEBN from MISCBILLAPP_GC_UNIONPORISHOD where CODE = c.unionparishad_code) UNIONPORISHAD_NAMEBN,
                         (select NAME from MISCBILLAPP_GC_UNIONPORISHOD where CODE = c.unionparishad_code) UNIONPORISHAD_NAME,
                        (select NAMEBN from MISCBILLAPP_MS_LOCATION  where CODE = c.location_code)  LOCATION_NAMEBN
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.unionparishad_code is not null " + (isZone ? @" and c.ZONE_CODE =:zone_code" : @" ") + (isLocationCode ? @" and c.Location_code =:loction_code " : " ");

            var result = await con.QueryAsync<UnionPorishadCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
            return result.ToList();


        }
        //private async Task<int[]> GetPouroshovaCustId()
        //{
        //    using var con = new OracleConnection(Connection.ConnectionString());
        //    List<CityCorporationCustIdModel> result = new List<CityCorporationCustIdModel>();
        //    var sQuery = @" select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
        //                INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
        //                where c.POUROSHOVA_CODE is not null";
        //    var resultt = await con.QueryAsync<CityCorporationCustIdModel>(sQuery);
        //    result = resultt.ToList();
        //    var custIdArray = result.Select(p => p.CUST_ID).ToArray();
        //    int[] custs = custIdArray;
        //    return custs;

        //}
        #region Utitly 

        public string GetChunkCustomers(int[] customers)
        {
            string _customers = string.Join(", ", customers);

            if (_customers.Length > 0)
                _customers = "" + _customers + "";

            return _customers;
        }


        #endregion


        #endregion online citycorporation and pouroshova

    }
}
