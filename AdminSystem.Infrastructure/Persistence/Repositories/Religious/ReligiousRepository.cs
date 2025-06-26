using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.CityCorporation;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.MinistryCustomer;
using Core.Application.Interfaces.Religious;
using Core.Domain.MinistryCustomer;
using Core.Domain.NonBengali;
using Core.Domain.Religious;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Location;
using Shared.DTOs.MinistryCustomer;
using Shared.DTOs.NonBengali;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Religious
{
    public class ReligiousRepository : IReligiousRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        private readonly IMinistryCustomerRepository _ministryCustomerRepository;
        private readonly ICityCorporationRepository _cityCorporation;



        public ReligiousRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo, IMinistryCustomerRepository ministryCustomerRepository, ICityCorporationRepository cityCorporation)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
            _ministryCustomerRepository = ministryCustomerRepository;
            _cityCorporation = cityCorporation;

        }

        public async Task<List<ReligiousDTOs>> GetAllReligiousSummaryData(string reportDate, string zoneCode, string locationCode, string? reportType, string circleCode)
        {
            var isZoneCode = zoneCode != "0" ? 1 : 0;
            var isCircle = circleCode != "0" ? zoneCode != "0" ? 1 : 0 : 0;
            List<ReligiousDTOs> religiousDTOs = new List<ReligiousDTOs>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_REPORT_DATE", value: reportDate.ToString().ToUpper(), dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_IS_ZONE_CODE", value: isZoneCode, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_IS_CIRCLE_CODE", value: isCircle, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REPORT_TYPE", value: reportType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Core.Domain.Religious.Religious>("MISC_REPORT_RELIGIOUS_PKG.RELIGIUS_DATA_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            religiousDTOs = _mapper.Map(result.ToList(), religiousDTOs);
            return religiousDTOs;
        }

        public async Task<List<LocationsDto>> GetLocationByCircle(string circleCode)
        {
            List<LocationsDto> locDTO = new List<LocationsDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: circleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationsDto>("MISC_REPORT_RELIGIOUS_PKG.ALL_LOCATION_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            locDTO = _mapper.Map(result.ToList(), locDTO);
            return locDTO;
        }

        public async Task<List<LocationWiseReligiousRptDTO>> GetLocationWiseRpt(string? zoneCode, string? locationCode, string startMonth, string endMonth)
        {
            List<LocationWiseReligiousRptDTO> religiousDTO = new List<LocationWiseReligiousRptDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_START_MONTH", value: startMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_END_MONTH", value: endMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationWiseReligiousRptDTO>("DPD_RELIGIOUS_REBAIT.LOCATION_WISE_RPT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            religiousDTO = _mapper.Map(result.ToList(), religiousDTO);
            return religiousDTO;
        }

        public async Task<List<NoOfMosqueAndOtherPlaceOfWorshipDTO>> GetNoOfMosqueAndWorshipData(string startMonth, string endMonth)
        {
            List<NoOfMosqueAndOtherPlaceOfWorshipDTO> worshipDTO = new List<NoOfMosqueAndOtherPlaceOfWorshipDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_START_MONTH", value: startMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_END_MONTH", value: endMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<NoOfMosqueAndOtherPlaceOfWorshipDTO>("DPD_RELIGIOUS_REBAIT.CIRCLE_WISE_RPT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            worshipDTO = _mapper.Map(result.ToList(), worshipDTO);
            return worshipDTO;
        }

        public async Task<List<ReligiousArrearSummaryDTO>> GetReligiousArrearSummary()
        {
            List<ReligiousArrearSummaryDTO> religiousArrearDTO = new List<ReligiousArrearSummaryDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<ReligiousArrearSummaryDTO>("DPD_RELIGIOUS_REBAIT.RELIGIOUS_SUMMERY", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            religiousArrearDTO = _mapper.Map(result.ToList(), religiousArrearDTO);
            return religiousArrearDTO;
        }


        public async Task<List<ReligiousReceiptDTO>> GetReligiousReceiptList(string locationCode)
        {

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ReligiousReceiptDTO>("MISC_REPORT_RELIGIOUS_PKG.RELIGIOUS_RECEIPT_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<bool> SaveReligiousReceiptBill(ReligiousReceiptViewModel model)
        {

            //foreach (var item in model)
            //{


            //}
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            OracleDynamicParameters paramnew = new OracleDynamicParameters();

            bool success = false;
            foreach (var item in model.CustomerList)
            {
                param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                param.Add("P_CUSTOMER_NUMBER", value: item.CustomerNo, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                param.Add("P_CUSTOMER_NAME", value: item.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_LOCATION_CODE", value: item.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_RECEIPT_AMOUNT", value: item.ReceiptAmt, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                param.Add("P_BILL_CYCLE_CODE", value: item.CurrentDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                param.Add("P_USER", value: item.CreateBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Execute("MISC_REPORT_RELIGIOUS_PKG.DPD_RELIGIOUS_RECEIPT_SAVE", param: param, commandType: CommandType.StoredProcedure);
                success = param.Get<int>("O_Status") > 0;
            }
            foreach (var item in model.NewCustomerList)
            {

                var db = await _dbConfigRepo.GetDatabaseDataBylocCodeAsync(item.LocationCode); // Getting database info by id.
                string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
                using var billConn = new OracleConnection(connectionString);

                string query = $"select  c.CUST_ID,C.CUSTOMER_NAME,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS  FROM ebc.bc_customers c INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=c.CUST_ID where  (c.customer_num || c.check_digit) ={item.NewCustomerNumber}";
                var resultt = await billConn.QueryFirstOrDefaultAsync<CustomerNameDTO>(query);
                string queryzone = $"SELECT ZONE_CODE ,CIRCLE_CODE ,LOCATION_CODE,DB_CODE  FROM MISCBILLAPP_GC_CUSTOMER WHERE CUSTOMER_NO='{item.CustomerNo}' and LOCATION_CODE='{item.LocationCode}'";
                var customerZone = await con.QueryFirstOrDefaultAsync<CustomerZoneCircleDTO>(queryzone);
                paramnew.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                paramnew.Add("P_CUST_ID", value: resultt.CUST_ID, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                paramnew.Add("P_CUSTOMER_NUMBER", value: item.NewCustomerNumber, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                paramnew.Add("P_CUSTOMER_NAME", value: resultt.CUSTOMER_NAME, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_MINISTRY_CODE", value: 29, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_DEPARTMENT_CODE", value: 31, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_RELIGIOUS_CODE", value: 1, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_ZONE_CODE", value: customerZone.ZONE_CODE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_CIRCLE_CODE", value: customerZone.CIRCLE_CODE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_LOCATION_CODE", value: item.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_DB_CODE", value: customerZone.DB_CODE, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_ADDRESS", value: resultt.ADDRESS, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_OLD_CUSTOMER_NUMBER", value: item.CustomerNo, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                paramnew.Add("P_BILL_CYCLE_CODE", value: item.CurrentDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                paramnew.Add("P_RECEIPT_AMOUNT", value: item.ReceiptAmt, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                paramnew.Add("P_CREATED_BY", value: item.CreateBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Execute("MISC_REPORT_RELIGIOUS_PKG. DPD_RELIGIOUS_RECEIPT_NEW_CUST_NUM", param: paramnew, commandType: CommandType.StoredProcedure);
                success = paramnew.Get<int>("O_Status") > 0;
                if (success)
                {
                    param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                    param.Add("P_CUSTOMER_NUMBER", value: item.NewCustomerNumber, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_CUSTOMER_NAME", value: resultt.CUSTOMER_NAME, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_LOCATION_CODE", value: item.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_RECEIPT_AMOUNT", value: item.ReceiptAmt, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
                    param.Add("P_BILL_CYCLE_CODE", value: item.CurrentDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_USER", value: item.CreateBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    var results = con.Execute("MISC_REPORT_RELIGIOUS_PKG.DPD_RELIGIOUS_RECEIPT_SAVE", param: param, commandType: CommandType.StoredProcedure);
                    success = param.Get<int>("O_Status") > 0;
                }
            }
            return success;



        }

        public string GetChunkCustomers(int[] customers)
        {
            string _customers = string.Join(", ", customers);

            if (_customers.Length > 0)
                _customers = "" + _customers + "";

            return _customers;
        }

        public async Task<List<LocationWiseReligiousRptDTO>> GetReligiousByMonth(string billMonth, string zoneCode, string locationCode)
        {
            List<LocationWiseReligiousRptDTO> religiousDTO = new List<LocationWiseReligiousRptDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BILL_MONTH", value: billMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<LocationWiseReligiousRptDTO>("DPD_RELIGIOUS_REBAIT.MONTH_WISE_RELIGIOUS_RPT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            religiousDTO = _mapper.Map(result.ToList(), religiousDTO);
            return religiousDTO;
        }
    }

}
