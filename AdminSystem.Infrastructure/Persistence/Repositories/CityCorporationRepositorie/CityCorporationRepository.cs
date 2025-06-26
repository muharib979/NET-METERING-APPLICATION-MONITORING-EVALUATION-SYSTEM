using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Castle.Core.Resource;
using Core.Application.Interfaces.CityCorporation;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Domain.APA;
using Core.Domain.CityCorporation;
using Core.Domain.Ministry;
using Core.Domain.Temporary;
using Dapper;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Agriculture;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.CityCorporationRepositorie
{
    public class CityCorporationRepository:ICityCorporationRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;

        public CityCorporationRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _mapper = mapper;
            _commRepo = commRepo;
            _dbConfigRepo = dbConfigRepo;
        }

        public async Task<List<CityCorporationDetailsDTO>> GetAllCityCorporAndPouroshovaArrearDetails(string billMonth, string reportType, string zoneCode, string locationCode, string circleCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("billMonth", value: billMonth.ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var result = await con.QueryAsync<CityCorporationDetailsDTO>("MISC_REPORT_CITY_POUR_UNI_PKG.POUR_CITY_ARREAR_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        #region  citycorporation 
        /// <summary>
        /// return all city corporation list
        /// </summary>
        public async Task<List<CityCorporationDataDto>> GetAllCityCorporation()
        {
            List<CityCorporationDataDto> cityCorporationDataDtos = new List<CityCorporationDataDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<CityCorporationDataDto>("MISC_REPORT_CITY_POUR_UNI_PKG.ALL_CITYCORPORATION_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            cityCorporationDataDtos = _mapper.Map(result.ToList(), cityCorporationDataDtos);
            return cityCorporationDataDtos;
        }

        #region online citycorporation and pouroshova
        public async Task<List<OnlineCityPourMergeData>> GetAllOnlinePouroAndCityCorporBn(string billMonth, string zoneCode, string locationCode)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorpoartionAndPouroCustId(zoneCode, locationCode);
            var mergeList = new List<OnlineCityPourMergeData>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);    
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            //string bill_cycl = convertBillMonth.AddMonths(+1).ToString("yyyyMM");
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

                            if (database.CODE=="3" || database.CODE=="5" || database.CODE=="6" || database.CODE=="7")
                           // if (database.CODE=="3")
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
                            var result = await conn.QueryAsync<CityCorporationArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth, P_BILL_CYCLE_CODES = bill_cycleRECEIPT, DB_CODE = database.CODE });
                            customers.AddRange(result.ToList());
                        }

                    }
                    //LEFT JOIN
                    //                         (SELECT R.CUST_ID, SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT, SUM(NVL(R.VAT_AMT, 0)) VAT_AMT,  Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                    //                            FROM BC_RECEIPT_HDR R
                    //                            INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                    //                                    WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycle AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                    //                           ) D ON X.CUST_ID = D.CUST_ID
                }
                catch (Exception)
                {
                    continue;
                }
            }

            mergeList.AddRange((from c in customers

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID && c.DB_CODE== cust.DB_CODE 
                                select new OnlineCityPourMergeData
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    ADDRESS = c.ADDRESS,
                                    ARREAR_AMT = c.TOTAL_AMOUNT,
                                    CITYCORPORATION_CODE = cust.CITYCORPORATION_CODE,
                                    POUROSHOVA_CODE = cust.POUROSHOVA_CODE,
                                    City_Cor_NAME = cust.City_Cor_NAME,
                                    POUR_NAME = cust.POUR_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    ZONE_CODE = cust.ZONE_CODE,
                                    ZONE_NAME = cust.ZONE_NAME,
                                    RECEIPT_AMT = c.RECEIPT_AMT,
                                    ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                    CURR_LPS = c.CURR_LPS,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS= c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT= c.ARREAR_VAT,
                                    POUR_NAMEBN = cust.POUR_NAMEBN,
                                    City_Cor_NAMEBN = cust.City_Cor_NAMEBN,
                                    TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                    RECEIPT_BILLMONTH = c.RECEIPT_BILLMONTH,
                                    PRV_RECEIPT_AMT = c.PRV_RECEIPT_AMT,
                                    PRV_RECEIPT_PRINCIPAL = c.PRV_RECEIPT_PRINCIPAL,
                                    PRV_RECEIPT_VAT = c.PRV_RECEIPT_VAT,
                                    Total_Receipt_Arrear = c.TOTAL_RECEIPT_ARREAR,

                                    ORDER_NO = cust.ORDER_NO
                                }));

            return mergeList;
        }

        private async Task<int[]> GetNonCustLocationDesc(string dbCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<CityCorporationCustIdModel> result = new List<CityCorporationCustIdModel>();
            var sQuery = @"
                         select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.CITYCORPORATION_CODE is not null and DB_CODE =:dbCode

                        UNION ALL

                        select CID.CUST_ID   from MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.POUROSHOVA_CODE is not null and DB_CODE =:dbCode";
            var resultt = await con.QueryAsync<CityCorporationCustIdModel>(sQuery, new { dbCode });
            result = resultt.ToList();
            var custIdArray = result.Select(p => p.CUST_ID).ToArray();
            int[] custs = custIdArray;
            return custs;

        }

        private async Task<List<CityCorporationCustIdModel>> GetCityCorporAndPouroCodeWiseCustId(string zoneCode, string locationCode, string cityCorporationCode, string pouroshovaCode) 
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode == "0" ? false : isZone ? true : false;
            var isCityCorporationCode = cityCorporationCode == "0" ? false : isZone ? true : false;
            var isPouroshovaCode = pouroshovaCode == "0" ? false : isZone ? true : false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"
                        select 
                         CID.CUST_ID, 
                         ' ' POUROSHOVA_CODE,
                         c.CITYCORPORATION_CODE,
                         c.ZONE_CODE, 
                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                          N' ' POUR_NAMEBN,
                         ' ' POUR_NAME,
                         (select NAMEBn from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) City_Cor_NAMEBN,
                          (select NAME from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) City_Cor_NAME,
                          (select ORDER_NUMBER from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) ORDER_NO
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                         where c.CITYCORPORATION_CODE is not null" + (isZone ? @" and c.ZONE_CODE =:zone_code" : @" ") + (isLocationCode ? @" and Location_code =:loction_code " : " ") +
                         (isCityCorporationCode ? @" and c.CITYCORPORATION_CODE =:citycorporation_code " : " ")+

                       @" UNION ALL
                        
                        
                              select 
                         CID.CUST_ID, 
                         c.POUROSHOVA_CODE,
                        ' ' CITYCORPORATION_CODE,
                         c.ZONE_CODE, 
                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                         (select NAMEBN from MISCBILLAPP_GC_POUROSHOVA where CODE = c.POUROSHOVA_CODE) POUR_NAMEBN,
                         (select NAME from MISCBILLAPP_GC_POUROSHOVA where CODE = c.POUROSHOVA_CODE) POUR_NAME,
                             N' ' City_Cor_NAMEBN,
                             ' ' City_Cor_NAME,
                          0 OREDER_NO
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.POUROSHOVA_CODE is not null " + (isZone ? @" and c.ZONE_CODE =:zone_code" : @" ") + (isLocationCode ? @" and Location_code =:loction_code " : " ") +
                        (isPouroshovaCode ? @" and c.POUROSHOVA_CODE =:pouroshova_code " : " ");

            var result = await con.QueryAsync<CityCorporationCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode, citycorporation_code = cityCorporationCode, pouroshova_code = pouroshovaCode });
            return result.ToList();
        }

        private async Task<List<CityCorporationCustIdModel>> GetCityCorpoartionAndPouroCustId( string zoneCode, string locationCode)
        {
            var isZone = zoneCode == "0" ? false : true;
            var isLocationCode = locationCode== "0" ? false : isZone ? true: false;
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"
                        select 
                         CID.CUST_ID, 
                         ' ' POUROSHOVA_CODE,
                         c.CITYCORPORATION_CODE,
                         c.ZONE_CODE, 
                                C.DB_CODE ,

                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                          N' ' POUR_NAMEBN,
                         ' ' POUR_NAME,
                         (select NAMEBn from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) City_Cor_NAMEBN,
                          (select NAME from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) City_Cor_NAME,
                          (select ORDER_NUMBER from GC_CITYCORPORATION where code = c.CITYCORPORATION_CODE) ORDER_NO
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                         where c.CITYCORPORATION_CODE is not null" + (isZone  ? @" and c.ZONE_CODE =:zone_code": @" ") + (isLocationCode ? @" and Location_code =:loction_code " : " " )+

                       @" UNION ALL
                        
                        
                              select 
                         CID.CUST_ID, 
                         c.POUROSHOVA_CODE,
                        ' ' CITYCORPORATION_CODE,
                         c.ZONE_CODE, 
                                C.DB_CODE ,

                         (Select NAMEBN From  MISCBILLAPP_MS_ZONE where code = c.ZONE_CODE) ZONE_NAME,
                         (select NAMEBN from MISCBILLAPP_GC_POUROSHOVA where CODE = c.POUROSHOVA_CODE) POUR_NAMEBN,
                         (select NAME from MISCBILLAPP_GC_POUROSHOVA where CODE = c.POUROSHOVA_CODE) POUR_NAME,
                             N' ' City_Cor_NAMEBN,
                             ' ' City_Cor_NAME,
                          0 OREDER_NO
                         FROM MISCBILLAPP_GC_CUSTOMER  c
                        INNER JOIN MISCBILLAPP_GC_CUSTID CID on CID.CONSUMER_NO = c.CUSTOMER_NO
                        where c.POUROSHOVA_CODE is not null " + (isZone ? @" and c.ZONE_CODE =:zone_code" : @" ") + (isLocationCode ? @" and Location_code =:loction_code " : " ");

            var result = await con.QueryAsync<CityCorporationCustIdModel>(sQuery, new { zone_code = zoneCode, loction_code = locationCode });
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


        /// <summary>
        /// return all city corporation list
        /// <paramref name="billMonth"/>
        /// <para>
        /// data type string 
        /// param take data 
        /// <example>
        /// validDate = '01-JAN-23'
        /// </example>
        /// </para>
        /// </summary>
        public async Task<List<CityCorporationDto>> GetPourAndCityCorporbyDate(string billMonth, string reportType, string zoneCode, string locationCode, string circleCode)
        {
            List<CityCorporationDto> cityCorporationDtos = new List<CityCorporationDto>();

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("billMonth", value: billMonth.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CityCorporations>("MISC_REPORT_CITY_POUR_UNI_PKG.POUR_CITY_ARREAR_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            cityCorporationDtos = _mapper.Map(result, cityCorporationDtos);
            return cityCorporationDtos;
        }
        /// <summary>
        /// return zone wise and location wise City poroshova and union arrear list
        /// <paramref name="billMonth"/>
        /// <para>
        /// zoneCode: data type string 
        /// reportDate: data type string 
        /// <example>
        /// zoneCode: "1",
        /// validDate = '01-JAN-23'
        /// </example>
        /// </para>
        /// </summary>
        public async Task<List<ZoneLocationWiseCityPouroUnion>> GetZoneLocationWiseCityPouroUnionArrear(string zoneCode, string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("zone_code_value", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("billMonth", value: billMonth.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ZoneLocationWiseCityPouroUnion>("MISC_REPORT_CITY_POUR_UNI_PKG.ZoneLocWiseCityPourUni_Arr", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result.ToList();
        }
        /// <summary>
        /// return all city corporation list
        /// <paramref name="validDate"/>
        /// <para>
        /// data type string 
        /// param take data 
        /// <example>
        /// validDate = '01-JAN-23'
        /// </example>
        /// </para>
        /// </summary>
        public async Task<List<ZoneWiseCityPouroUnionArrear>> GetZoneWiseCityPouroUnionArrear(string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("billMonth", value: billMonth.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ZoneWiseCityPouroUnionArrear>("MISC_REPORT_CITY_POUR_UNI_PKG.ZON_WISE_CITY_UNI_POUR", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result.ToList();
        }


        #endregion get all citycorporation

        #region GetAllOnlinePouroAndCityCorpoBnDetails
        public async Task<List<OnlineCityPourMergeData>> GetAllOnlinePouroAndCityCorpoBnDetails(string billMonth, string zoneCode, string locationCode)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorpoartionAndPouroCustId(zoneCode, locationCode);
            var mergeList = new List<OnlineCityPourMergeData>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            foreach (var database in allDatabases)
            {
                var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

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
                                     Select * from ( SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
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
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.CURR_RECEIPT_AMT)
                                       ELSE 0 
                                             END AS ARREAR_RECEIPT_AMOUNT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (B.CURR_RECEIPT_AMT)
                                       ELSE (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) 
                                             END AS CURR_RECEIPT_AMT

                                          --  F.FINANCIAL_AMOUNT
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
                                                    WHERE BILL_CYCLE_CODE <:P_BILL_CYCLE_CODE AND    (
                                                    (NVL(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)-NVL(IH.PRINCIPAL_APPL,0)) + ( NVL(IH.LPS_AMT,0)+NVL(IH.LPS_ADJ,0)-NVL(IH.LPS_APPL,0))+
                                                    (NVL(IH.VAT_AMT,0)+NVL(IH.VAT_ADJ,0)-NVL(VAT_APPL,0))) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                       SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                       SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT,
                                                       sum(NVL(IH.INVOICE_AMT,0)) CURR_RECEIPT_AMT

                                                FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE = :P_BILL_CYCLE_CODE AND  " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT,  Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
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

            mergeList.AddRange((from c in customers

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineCityPourMergeData
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    ADDRESS = c.ADDRESS,
                                    ARREAR_AMT = c.TOTAL_AMOUNT,
                                    CITYCORPORATION_CODE = cust.CITYCORPORATION_CODE,
                                    POUROSHOVA_CODE = cust.POUROSHOVA_CODE,
                                    City_Cor_NAME = cust.City_Cor_NAME,
                                    POUR_NAME = cust.POUR_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    ZONE_CODE = cust.ZONE_CODE,
                                    ZONE_NAME = cust.ZONE_NAME,
                                    RECEIPT_AMT = c.RECEIPT_AMT,
                                    ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                    CURR_LPS = c.CURR_LPS,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS = c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT = c.ARREAR_VAT,
                                    POUR_NAMEBN = cust.POUR_NAMEBN,
                                    City_Cor_NAMEBN = cust.City_Cor_NAMEBN,
                                    TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                    RECEIPT_BILLMONTH = c.RECEIPT_BILLMONTH,
                                    PRV_RECEIPT_AMT = c.PRV_RECEIPT_AMT,
                                    PRV_RECEIPT_PRINCIPAL = c.PRV_RECEIPT_PRINCIPAL,
                                    PRV_RECEIPT_VAT = c.PRV_RECEIPT_VAT
                                }));

            return mergeList;
        }

        #endregion

        public async Task<List<ZoneWiseCitycorporationDataDTO>> GetZoneWiseCitycorporationData(string zoneCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = await con.QueryAsync<ZoneWiseCitycorporationDataDTO>("MISC_REPORT_CITY_POUR_UNI_PKG.ZONE_WISE_CITYCORPORATION", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<ZoneWisePouroshovaDataDTO>> GetZoneWisePouroshovaData(string zoneCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = await con.QueryAsync<ZoneWisePouroshovaDataDTO>("MISC_REPORT_CITY_POUR_UNI_PKG.ZONE_WISE_POUROSHOVA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityCodeWiseBn(string billMonth, string zoneCode, string locationCode, string cityCorporationCode, string pouroshovaCode)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorporAndPouroCodeWiseCustId(zoneCode, locationCode, cityCorporationCode, pouroshovaCode);
            var mergeList = new List<OnlineCityPourMergeData>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            //string bill_cycl = convertBillMonth.AddMonths(+1).ToString("yyyyMM");
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
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN')   AND " + query + @"GROUP BY R.CUST_ID
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
                            var result = await conn.QueryAsync<CityCorporationArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth, P_BILL_CYCLE_CODES = bill_cycleRECEIPT });
                            customers.AddRange(result.ToList());
                        }

                    }
                    //LEFT JOIN
                    //                         (SELECT R.CUST_ID, SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT, SUM(NVL(R.VAT_AMT, 0)) VAT_AMT,  Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                    //                            FROM BC_RECEIPT_HDR R
                    //                            INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                    //                                    WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycle AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                    //                           ) D ON X.CUST_ID = D.CUST_ID
                }
                catch (Exception)
                {
                    continue;
                }
            }

            mergeList.AddRange((from c in customers

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineCityPourMergeData
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    ADDRESS = c.ADDRESS,
                                    ARREAR_AMT = c.TOTAL_AMOUNT,
                                    CITYCORPORATION_CODE = cust.CITYCORPORATION_CODE,
                                    POUROSHOVA_CODE = cust.POUROSHOVA_CODE,
                                    City_Cor_NAME = cust.City_Cor_NAME,
                                    POUR_NAME = cust.POUR_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    ZONE_CODE = cust.ZONE_CODE,
                                    ZONE_NAME = cust.ZONE_NAME,
                                    RECEIPT_AMT = c.RECEIPT_AMT,
                                    ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                    CURR_LPS = c.CURR_LPS,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS = c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT = c.ARREAR_VAT,
                                    POUR_NAMEBN = cust.POUR_NAMEBN,
                                    City_Cor_NAMEBN = cust.City_Cor_NAMEBN,
                                    TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                    RECEIPT_BILLMONTH = c.RECEIPT_BILLMONTH,
                                    PRV_RECEIPT_AMT = c.PRV_RECEIPT_AMT,
                                    PRV_RECEIPT_PRINCIPAL = c.PRV_RECEIPT_PRINCIPAL,
                                    PRV_RECEIPT_VAT = c.PRV_RECEIPT_VAT,
                                    Total_Receipt_Arrear = c.TOTAL_RECEIPT_ARREAR,

                                    ORDER_NO = cust.ORDER_NO
                                }));

            return mergeList;
        }

        public async Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityCodeWiseBnDetails(string billMonth, string zoneCode, string locationCode, string cityCorporationCode, string pouroshovaCode)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorporAndPouroCodeWiseCustId(zoneCode, locationCode, cityCorporationCode, pouroshovaCode);
            var mergeList = new List<OnlineCityPourMergeData>();
            string createDateString = billMonth.Substring(billMonth.Length - 2) + "/" + "01/" + billMonth.Substring(0, 4);
            var convertBillMonth = DateTime.Parse(createDateString);
            string bill_cycleRECEIPT = convertBillMonth.AddMonths(-1).ToString("yyyyMM");
            foreach (var database in allDatabases)
            {
                var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(database.CODE); // Getting database info by id.

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
                                     Select * from ( SELECT X.CUST_ID,(X.CUSTOMER_NUM || X.CHECK_DIGIT) CONSUMER_NO, X.CUSTOMER_NAME,
                                           X.LOCATION_CODE , 
                                           X.LOCATION_DESC,
                                           :P_BILL_CYCLE_CODE BILL_CYCLE_CODE,
                                           X.ADDRESS,
                                            NVL(C.RECEIPT_BILLMONTH,0) RECEIPT_BILLMONTH,
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
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (C.RECEIPT_AMT+C.VAT_AMT) - (B.CURR_RECEIPT_AMT)
                                       ELSE 0 
                                             END AS ARREAR_RECEIPT_AMOUNT,
                                       CASE 
                                            WHEN (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) > (B.CURR_RECEIPT_AMT) THEN (B.CURR_RECEIPT_AMT)
                                       ELSE (NVL(C.RECEIPT_AMT,0)+NVL(C.VAT_AMT,0)) 
                                             END AS CURR_RECEIPT_AMT

                                          --  F.FINANCIAL_AMOUNT
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
                                                    WHERE BILL_CYCLE_CODE <:P_BILL_CYCLE_CODE AND    (
                                                    (NVL(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)-NVL(IH.PRINCIPAL_APPL,0)) + ( NVL(IH.LPS_AMT,0)+NVL(IH.LPS_ADJ,0)-NVL(IH.LPS_APPL,0))+
                                                    (NVL(IH.VAT_AMT,0)+NVL(IH.VAT_ADJ,0)-NVL(VAT_APPL,0))) > 0 AND " + query + @"
                                               GROUP BY IH.CUST_ID) 

                                               A ON A.CUST_ID=X.CUST_ID

                                       LEFT JOIN
                                              (
                                                SELECT IH.CUST_ID,SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0)) PRINCIPAL,
                                                       SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) LPS,
                                                       SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) VAT,
                                                       sum(NVL(IH.INVOICE_AMT,0)) CURR_RECEIPT_AMT

                                                FROM BC_INVOICE_HDR IH 
                                                    WHERE BILL_CYCLE_CODE = :P_BILL_CYCLE_CODE AND  " + query + @"
                                               GROUP BY IH.CUST_ID)
                                               
                                                B ON X.CUST_ID = B.CUST_ID

                                      LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(NVL(R.RECEIPT_AMT, 0)) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT,  Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:p_bill_cycleRECEIPT AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
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

            mergeList.AddRange((from c in customers

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineCityPourMergeData
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    ADDRESS = c.ADDRESS,
                                    ARREAR_AMT = c.TOTAL_AMOUNT,
                                    CITYCORPORATION_CODE = cust.CITYCORPORATION_CODE,
                                    POUROSHOVA_CODE = cust.POUROSHOVA_CODE,
                                    City_Cor_NAME = cust.City_Cor_NAME,
                                    POUR_NAME = cust.POUR_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    ZONE_CODE = cust.ZONE_CODE,
                                    ZONE_NAME = cust.ZONE_NAME,
                                    RECEIPT_AMT = c.RECEIPT_AMT,
                                    ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                    CURR_LPS = c.CURR_LPS,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS = c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT = c.ARREAR_VAT,
                                    POUR_NAMEBN = cust.POUR_NAMEBN,
                                    City_Cor_NAMEBN = cust.City_Cor_NAMEBN,
                                    TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                    RECEIPT_BILLMONTH = c.RECEIPT_BILLMONTH,
                                    PRV_RECEIPT_AMT = c.PRV_RECEIPT_AMT,
                                    PRV_RECEIPT_PRINCIPAL = c.PRV_RECEIPT_PRINCIPAL,
                                    PRV_RECEIPT_VAT = c.PRV_RECEIPT_VAT
                                }));

            return mergeList;
        }

        public async Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityDetailsWithCRV(string billMonth, string zoneCode, string locationCode, string reportType)
        {
            var allDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob();
            List<CityCorporationArrearModel> customers = new List<CityCorporationArrearModel>();
            var cityCustId = await GetCityCorpoartionAndPouroCustId(zoneCode, locationCode);
            var mergeList = new List<OnlineCityPourMergeData>();
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
                                             ( SELECT R.CUST_ID,SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                        END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT ,Max(IH.BILL_CYCLE_CODE) RECEIPT_BILLMONTH

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') =:P_BILL_CYCLE_CODE AND R.RECEIPT_TYPE_CODE <> 'CRV' AND " + query + @"GROUP BY R.CUST_ID
                                               ) C ON X.CUST_ID = C.CUST_ID

                                       LEFT JOIN 
                                             ( SELECT R.CUST_ID,SUM(CASE WHEN R.RECEIPT_TYPE_CODE = 'CAN' THEN NVL(R.RECEIPT_AMT, 0)
                                                                ELSE CASE WHEN NVL(R.RECEIPT_AMT, 0) > 0 THEN NVL(R.RECEIPT_AMT, 0) ELSE 0 END
                                                                    END) AS RECEIPT_AMT,SUM(NVL(R.VAT_AMT, 0)) VAT_AMT

                                                FROM BC_RECEIPT_HDR R
                                                INNER JOIN BC_INVOICE_HDR IH ON IH.INVOICE_NUM = R.INVOICE_NUM
                                                        WHERE TO_CHAR(TO_DATE(R.RECEIPT_DATE, 'DD-MON-RR'), 'YYYYMM') <:P_BILL_CYCLE_CODE AND R.SEQ_NUM IS NOT NULL AND (R.RECEIPT_TYPE_CODE <> 'CRV' OR R.CREATE_BY = 'SYSADMIN') AND " + query + @"GROUP BY R.CUST_ID
                                               ) D ON X.CUST_ID = D.CUST_ID

                                            ) P order by LOCATION_CODE ASC, RECEIPT_BILLMONTH DESC";
                            var result = await conn.QueryAsync<CityCorporationArrearModel>(sQuery3, new { P_BILL_CYCLE_CODE = billMonth, p_bill_cycleRECEIPT = billMonth, P_BILL_CYCLE_CODES = bill_cycleRECEIPT });
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

                                from cust in cityCustId
                                where c.CUST_ID == cust.CUST_ID
                                select new OnlineCityPourMergeData
                                {
                                    CUST_ID = c.CUST_ID,
                                    CONSUMER_NO = c.CONSUMER_NO,
                                    CUSTOMER_NAME = c.CUSTOMER_NAME,
                                    ADDRESS = c.ADDRESS,
                                    ARREAR_AMT = c.TOTAL_AMOUNT,
                                    CITYCORPORATION_CODE = cust.CITYCORPORATION_CODE,
                                    POUROSHOVA_CODE = cust.POUROSHOVA_CODE,
                                    City_Cor_NAME = cust.City_Cor_NAME,
                                    POUR_NAME = cust.POUR_NAME,
                                    LOCATION_CODE = c.LOCATION_CODE,
                                    ZONE_CODE = cust.ZONE_CODE,
                                    ZONE_NAME = cust.ZONE_NAME,
                                    RECEIPT_AMT = c.RECEIPT_AMT,
                                    ARREAR_RECEIPT_AMOUNT = c.ARREAR_RECEIPT_AMOUNT,
                                    CURR_RECEIPT_VAT = c.CURR_RECEIPT_VAT,
                                    CURR_RECEIPT_PRINCIPAL = c.CURR_RECEIPT_PRINCIPAL,
                                    CURR_RECEIPT_AMT = c.CURR_RECEIPT_AMT,
                                    CURR_LPS = c.CURR_LPS,
                                    CURR_PRINCIPAL = c.CURR_PRINCIPAL,
                                    CURR_VAT = c.CURR_VAT,
                                    ARREAR_LPS = c.ARREAR_LPS,
                                    ARREAR_PRICIPAL = c.ARREAR_PRICIPAL,
                                    ARREAR_VAT = c.ARREAR_VAT,
                                    POUR_NAMEBN = cust.POUR_NAMEBN,
                                    City_Cor_NAMEBN = cust.City_Cor_NAMEBN,
                                    TOTAL_RECEIPT_PRINCIPAL = c.TOTAL_RECEIPT_PRINCIPAL,
                                    TOTAL_RECEIPT_VAT = c.TOTAL_RECEIPT_VAT,
                                    RECEIPT_BILLMONTH = c.RECEIPT_BILLMONTH,
                                    PRV_RECEIPT_AMT = c.PRV_RECEIPT_AMT,
                                    PRV_RECEIPT_PRINCIPAL = c.PRV_RECEIPT_PRINCIPAL,
                                    PRV_RECEIPT_VAT = c.PRV_RECEIPT_VAT,
                                    Total_Receipt_Arrear = c.TOTAL_RECEIPT_ARREAR,

                                    ORDER_NO = cust.ORDER_NO
                                }));

            return mergeList;
        }
    }
}
