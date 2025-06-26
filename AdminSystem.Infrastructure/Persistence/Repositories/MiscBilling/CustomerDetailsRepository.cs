using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.APA;
using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class CustomerDetailsRepository : ICustomerDetailsRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepositry;

        public CustomerDetailsRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepositry)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepositry = commRepositry;
        }

        public async Task<CustomerDetailsDTO> GetCustomerByBillNumber(string billNumber, string custNumber)
        {
            CustomerDetailsDTO customerDetails = new CustomerDetailsDTO();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: custNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CustomerDetails>("DPG_MISCBILL_PENALTY_BILL_GEN.GetPenaltyBillById", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            var customerObject = result.FirstOrDefault();
            customerDetails = _mapper.Map(customerObject, customerDetails);

            OracleDynamicParameters paramList = new OracleDynamicParameters();
            paramList.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramList.Add("BILL_NO", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            customerDetails.FileList = con.Query<FileDto>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_GetFilesData", param: paramList, commandType: CommandType.StoredProcedure).ToList();
            return customerDetails;
        }

        public async Task<CustomerDetailsDTO> GetCustomerByCustomerNymber(string custNumber, string locationCode)
        {
            try
            {
                CustomerDetailsDTO customerDetails = new CustomerDetailsDTO();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("CUST_NUMBER", value: custNumber.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode.Trim(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.QueryFirstOrDefault<CustomerDetails>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_DETAIL_BY_CUST_NUMBER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                customerDetails = _mapper.Map(result, customerDetails);
                return customerDetails;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<CustomerDetailsDTO> GetNonCustomerByBillNumber(string billNumber, string custNumber)
        {
            CustomerDetailsDTO customerDetails = new CustomerDetailsDTO();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: custNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CustomerDetails>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_NON_CUST_BILL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            var customerObject = result.FirstOrDefault();
            customerDetails = _mapper.Map(customerObject, customerDetails);

            OracleDynamicParameters paramList = new OracleDynamicParameters();
            paramList.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramList.Add("BILL_NO", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            customerDetails.FileList = con.Query<FileDto>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_GetFilesData", param: paramList, commandType: CommandType.StoredProcedure).ToList();
            return customerDetails;
        }

        public async Task<CustomerDetailsDTO> GetUntracedCustomer(string custNumber, string dbCode, string locCode)
        {
            CustomerDetailsDTO customerDetails = new CustomerDetailsDTO();
            using var conn = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locCode}'";
            dbCode = conn.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var connection = new OracleConnection(connectionString);
            string query = $@"select a.cust_id CUST_ID,
                                a.CUSTOMER_NUM||a.CONS_CHK_DIGIT CustomerNumber,
                                b.CUSTOMER_NAME Customer_Name,
                                a.location_code LOCATION_CODE,
                                a.SANC_LOAD SanctionedLoad, 
                                lm.DESCR LOCATION_NAME,
                                a.area_code AREA_CODE, 
                                a.TARIFF TARIFF, 
                                a.METER_NUM_KWH METER_NUM,
                                b.walking_sequence AS wlk_ord,
                                substr(b.area_code,4) BILL_GROUP,
                                substr(b.area_code,1,3) BOOK_NO,
                                b.cons_extg_num PRV_AC_NO,
                                d.ADDR_DESCR1||', '||d.ADDR_DESCR2 || ', ' || d.CITY CUSTOMER_ADDR,
                                e.DEFECTIVE_CODE METER_CODINTION_CODE,
                                e.DEFECTIVE_DESC METER_CODINTION_DESC,
                                f.total_arrear ArrearAmount,
                                a.METER_TYPE_KWH METER_TYPE_Code,
                                z.METER_TYPE_DESC  METER_TYPE_DESC,
                                a.bill_cycle_code,
                                y.BUS_TYPE_CODE  BUSINESS_TYPE_CODE,
                                    y.BUS_TYPE_DESC  BUSINESS_TYPE,
                                CLS_KWH_SR_RDNG LastBillReadingSr,
                                CLS_KWH_PK_RDNG LastBillReadingPk,
                                CLS_KWH_OFPK_RDNG LastBillReadingOfPk,
                                a.METER_COND_KWH
                                from bc_bill_image a,BC_METER_TYPECODE_MAP  z,BC_BUS_TYPE_CODE y,bc_customers b,BC_LOCATION_MASTER lm,(SELECT t1.cust_id,MAX(BILL_CYCLE_CODE) bill_cycle_code from  bc_bill_image  t,bc_customers t1
                                                       WHERE t.cust_id = t1.cust_id
                                                        AND t1.LOCATION_CODE =:locCode 
                                                        and (t1.customer_num || t1.check_digit) = '{custNumber}'
                                                        group by t1.cust_id) c,BC_CUSTOMER_ADDR d,BC_DEFECTIVE_CODE e,(select ih.cust_id,sum((NVL(IH.PRINCIPAL_AMT,0)+NVL(IH.PRINCIPAL_ADJ,0)-NVL(IH.PRINCIPAL_APPL,0)) + ( NVL(IH.LPS_AMT,0)+NVL(IH.LPS_ADJ,0)-NVL(IH.LPS_APPL,0))+
                                                    (NVL(IH.VAT_AMT,0)+NVL(IH.VAT_ADJ,0)-NVL(VAT_APPL,0)))  total_arrear from bc_invoice_hdr ih,bc_customers t1
                                                       WHERE ih.cust_id = t1.cust_id
                                                       and (t1.customer_num || t1.check_digit) = '{custNumber}'
                                                        AND t1.LOCATION_CODE =:locCode 
                                                        group by ih.cust_id) f
                                        where A.bill_cycle_code=c.bill_cycle_code
                                        and (b.customer_num || b.check_digit) = '{custNumber}'
                                        and A.CUST_ID=b.cust_id
                                        AND z.old_meter_typecode=a.METER_TYPE_KWH AND y.BUS_TYPE_CODE=a.BUS_TYPE_CODE
                                        and a.cust_id=c.cust_id
                                        AND B.LOCATION_CODE =:locCode 
                                        AND B.LOCATION_CODE=lm.location_code
                                        AND d.ADDR_TYPE='B'
                                        AND d.ADDR_EXP_DATE IS NULL
                                        and a.cust_id=d.cust_id
                                        and A.METER_COND_KWH=e.DEFECTIVE_CODE(+)
                                        and a.cust_id=f.cust_id(+)
                                        and A.INVOICE_NUM is not null";
            var result = connection.QueryFirstOrDefault<CustomerDetails>(query, new { locCode });
            using var con = new OracleConnection(Connection.ConnectionString());
            string deptquery = $"SELECT DEPTCODE FROM MISCBILLAPP_MS_LOCATION WHERE CODE='{locCode}'";
            var deptCode = con.QueryFirstOrDefault<string>(deptquery);
            result.DEPTCODE = deptCode;
            customerDetails = _mapper.Map(result, customerDetails);
            //customerDetails.ArrearAmount =await GetCustomerArrear(customerDetails.CustId, connectionString);
            return customerDetails;
        }



        private async Task<decimal> GetCustomerArrear(int custId, string connectionString)
        {
            //var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            //string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var connection = new OracleConnection(connectionString);
            var sQuery = $@"SELECT SUM(nvl(IH.PRINCIPAL_AMT,0)+nvl(IH.PRINCIPAL_ADJ,0)-nvl(IH.PRINCIPAL_APPL,0))+
                                                      SUM(nvl( IH.LPS_AMT,0)+ nvl(IH.LPS_ADJ,0)-nvl(IH.LPS_APPL,0)) +
                                                      SUM( nvl(IH.VAT_AMT,0)+ nvl(IH.VAT_ADJ,0)-nvl(IH.VAT_APPL,0)) Total_Arrear  
                                               FROM BC_INVOICE_HDR IH 
                                                    WHERE IH.CUST_ID={custId}";
            var result = connection.QueryFirstOrDefault<decimal>(sQuery);
            return result;
        }




        public async Task<CustomerDetailsPenaltyBillPrepaidDTO> GetCustomerDetailsPrepaidBillPenalty(string custNumber, string dbCode, string locCode)
        {
            CustomerDetailsPenaltyBillPrepaidDTO customerDetails = new CustomerDetailsPenaltyBillPrepaidDTO();
            using var conn = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locCode}'";
            dbCode = conn.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var connection = new OracleConnection(connectionString);
            string query = $@"select a.cust_id CUST_ID,
                                            a.CUSTOMER_NUM||a.CONS_CHK_DIGIT CustomerNumber,
                                            b.CUSTOMER_NAME Customer_Name,
                                            b.F_H_NAME Father_Name,
                                            a.location_code LOCATION_CODE,
                                             a.SANC_LOAD SanctionedLoad, 
                                             lm.DESCR LOCATION_NAME,
                                             a.area_code AREA_CODE, 
                                             a.TARIFF TARIFF_DESC, 
                                             a.METER_NUM_KWH METER_NUM,
                                             b.walking_sequence wlk_ord,
                                             substr(b.area_code,4) BILL_GROUP,
                                              substr(b.area_code,1,3) BOOK_NO,
                                              b.cons_extg_num PRV_AC_NO,
                                              d.ADDR_DESCR1||', '||d.ADDR_DESCR2 || ', ' || d.CITY CUSTOMER_ADDR,
                                              e.DEFECTIVE_CODE METER_CODINTION_CODE,
                                             e.DEFECTIVE_DESC METER_CODINTION_DESC,   
                                            a.METER_TYPE_KWH METER_TYPE_Code,
                                            y.BUS_TYPE_DESC BUSINESS_TYPE,
                                            y.BUS_TYPE_CODE BUSINESS_TYPE_CODE,
                                            z.METER_TYPE_DESC  METER_TYPE_DESC,
                                            a.bill_cycle_code
                                from bc_bill_image a,BC_METER_TYPECODE_MAP  z,BC_BUS_TYPE_CODE y,bc_customers b,BC_LOCATION_MASTER lm,(SELECT t1.cust_id,MAX(BILL_CYCLE_CODE) bill_cycle_code from  bc_bill_image  t,bc_customers t1
                                                       WHERE t.cust_id = t1.cust_id
                                                        AND t1.LOCATION_CODE =:locCode 
                                                        and (t1.customer_num || t1.check_digit) = '{custNumber}'
                                                        group by t1.cust_id) c,BC_CUSTOMER_ADDR d,BC_DEFECTIVE_CODE e
                                           where A.bill_cycle_code=c.bill_cycle_code
                                          and (b.customer_num || b.check_digit) = '{custNumber}'
                                        and A.CUST_ID=b.cust_id
                                        AND z.old_meter_typecode=a.METER_TYPE_KWH AND y.BUS_TYPE_CODE=a.BUS_TYPE_CODE
                                        and a.cust_id=c.cust_id
                                        AND B.LOCATION_CODE =:locCode 
                                        AND B.LOCATION_CODE=lm.location_code
                                        AND d.ADDR_TYPE='B'
                                        AND d.ADDR_EXP_DATE IS NULL
                                        and a.cust_id=d.cust_id
                                        and A.METER_COND_KWH=e.DEFECTIVE_CODE(+)
                                        and A.INVOICE_NUM is not null";
            var result = connection.QueryFirstOrDefault<CustomerDetails>(query, new { locCode });
            using var con = new OracleConnection(Connection.ConnectionString());
            string deptquery = $"SELECT DEPTCODE FROM MISCBILLAPP_MS_LOCATION WHERE CODE='{locCode}'";
            var deptCode = con.QueryFirstOrDefault<string>(deptquery);
            result.DEPTCODE = deptCode;
            customerDetails = _mapper.Map(result, customerDetails);
            //customerDetails.ArrearAmount =await GetCustomerArrear(customerDetails.CustId, connectionString);
            return customerDetails;
        }

        public async Task<UserInfoDto> GetCustomersInformation(string customerNumber, string dbCode, string locCode)
        {
            UserInfoDto customerDetails = new UserInfoDto();
            using var conn = new OracleConnection(Connection.ConnectionString());
            string querydb = $"SELECT DB_CODE FROM MISCBILLAPP_MS_LOCATION where CODE='{locCode}'";
            dbCode = conn.QueryFirstOrDefault<string>(querydb);
            var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by id.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var connection = new OracleConnection(connectionString);
                                            string query = $@"SELECT *
                                                        FROM (
                                                            SELECT 
                                                                B.CUSTOMER_NUM || B.CHECK_DIGIT AS CUST_NUM,
                                                                B.LOCATION_CODE,
                                                                TO_CHAR(TO_DATE(TO_DATE(SUBSTR(A.BILL_CYCLE_CODE, 5, 2) || SUBSTR(A.BILL_CYCLE_CODE, 1, 4), 'MMYYYY'), 'DD/MM/RRRR'), 'MON-YY') AS MONTH,
                                                                NVL(PRINCIPAL_AMT, 0) + NVL(PRINCIPAL_ADJ, 0) - NVL(PRINCIPAL_APPL, 0) + NVL(LPS_AMT, 0) + NVL(LPS_ADJ, 0) - NVL(LPS_APPL, 0) + NVL(VAT_AMT, 0) + NVL(VAT_ADJ, 0) - NVL(VAT_APPL, 0) AS MONTH_ARR
                                                            FROM 
                                                                EBC.BC_INVOICE_HDR A
                                                            JOIN 
                                                                BC_CUSTOMERS B ON A.CUST_ID = B.CUST_ID
                                                        ) S
                                                        WHERE 
                                                    S.LOCATION_CODE = :locCode 
                                                    AND (S.CUST_NUM = :customerNumber OR :customerNumber IS NULL)
                                                    AND S.MONTH_ARR > 1;";
            var result = connection.QueryFirstOrDefault<UserDetails>(query, new { locCode });
            using var con = new OracleConnection(Connection.ConnectionString());
            customerDetails = _mapper.Map(result, customerDetails);
            return customerDetails;
        }
    }
}
