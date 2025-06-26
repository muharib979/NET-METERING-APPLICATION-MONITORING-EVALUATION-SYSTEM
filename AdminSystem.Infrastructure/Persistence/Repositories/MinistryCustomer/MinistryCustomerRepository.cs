using AutoMapper;
using Core.Application.Interfaces.Common.Repository;

using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.MinistryCustomer;
using Core.Domain.MinistryCustomer;
using Core.Domain.NonBengali;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Ministry;
using Shared.DTOs.MinistryCustomer;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MinistryCustomer
{
    public class MinistryCustomerRepository : IMinistryCustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;

        public MinistryCustomerRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository _commRepo)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            this._commRepo = _commRepo;
        }
        public async Task<List<DropdownResultForStringKey>> GetAllPouroshova()
        {
            List<DropdownResultForStringKey> pouroshova = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Pouroshova>("DPG_MINISTRY_CUSTOMER_PKG.GET_POUROSHAVA_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            pouroshova = _mapper.Map(result, pouroshova); 
            return pouroshova;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllUnionParishod()
        {
            List<DropdownResultForStringKey> unionParishod = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<UnionParishad>("DPG_MINISTRY_CUSTOMER_PKG.GET_UNION_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            unionParishod = _mapper.Map(result, unionParishod);
            return unionParishod;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllMinistryDepartment(string ministryCode)
        {
            List<DropdownResultForStringKey> ministryDept = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistryDepartment>("DPG_MINISTRY_CUSTOMER_PKG.GET_MINISTRY_BY_DEPT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            ministryDept = _mapper.Map(result, ministryDept);
            return ministryDept;
        }

        public async Task<List<MinistryCustomerGetDTOs>> GetAllMinistryCustomer(string? customerNo, string? centerCode, string? locationCode, string? ministryCode, string? cityCode, bool? isRebate)
        {
            string rebateCode = "";
            string cityCorpoCode = ministryCode=="2"? cityCode:"";
            if (ministryCode == "29")
            {
                 rebateCode = isRebate == true ? "1" : "0";
                //locationcode = null;

            }
            else
            {
                 rebateCode =  null;
            }

            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value:customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CITY_CODE", value: cityCorpoCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_RELIGIOUS_CODE", value: rebateCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistryCustomerGetDTOs>("DPG_GC_CUSTOMER.GET_GC_CUSTOMER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<MinistryDataViewDTO> SaveMinistryCustomer(MinistryCustomerDTOs model)
        {
            var custNo = Convert.ToString(model.CustomerNo);
            var ministryCustomer = GetMinistryCustomerById(custNo, model.LocationCode);
            bool isSuccess = false;
            var db =await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(model.DbCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var billConn = new OracleConnection(connectionString);
            string query = $"select  c.CUST_ID,C.CUSTOMER_NAME,(BCA.ADDR_DESCR1 ||  BCA.ADDR_DESCR2 || BCA.ADDR_DESCR3) ADDRESS  FROM ebc.bc_customers c INNER JOIN EBC.BC_CUSTOMER_ADDR BCA ON BCA.CUST_ID=c.CUST_ID where  (c.customer_num || c.check_digit) ={model.CustomerNo}";
            var result = await billConn.QueryFirstOrDefaultAsync<CustomerNameDTO>(query);

            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUSTOMER_NO", value: model.CustomerNo, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUSTOMER_NAMEBN", value: model.CustomerNameBN, dbType: (OracleMappingType?)OracleDbType.NVarchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUSTOMER_NAMEEN", value: result.CUSTOMER_NAME, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ADDRESS", value: result.ADDRESS, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DIVISION_CODE", value: model.DivisionCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DISTRICT_CODE", value: model.DistrictCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_UPAZILA_CODE", value: model.UpazilaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_MINISTRY_CODE", value: model.MinistryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DEPARTMENT_CODE", value: model.DepartmentCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CITYCORPORATION_CODE", value: model.CitycorporationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_POUROSHOVA_CODE", value: model.PouroshovaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_UNIONPARISHAD_CODE", value: model.UnionParishadCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_RELIGIOUS_CODE", value: model.ReligiousCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NON_BENGALI_CAMP_CODE", value: model.NonBengaliCampCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_IS_POLICE", value: model.IsPolice, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_ZONE_CODE", value: model.ZoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CIRCLE_CODE", value: model.CircleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DB_CODE", value: model.DbCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            
            if (result.CUST_ID > 0 && ministryCustomer.Result != null)
            {
                oracleDynamicParameters.Add("P_UPDATED_BY", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var results = con.Execute("TEST_GC_CUSTOMER.DPD_UPDATE_CUSTOMER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("cur_data") > 0;
            }
            else if (result.CUST_ID > 0)
            {
                oracleDynamicParameters.Add("P_CREATED_BY", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_ID", value: result.CUST_ID, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                var results = con.Execute("TEST_GC_CUSTOMER.DPD_SAVE_CUSTOMER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("cur_data") > 0;
            }

            if(isSuccess)
            {
                var viewData = await GetCustomerData(custNo, model.LocationCode);
                return viewData;
            }
            else
            {
                throw new Exception("Invalid Customer No");
            }
        }

        public async Task<int> DeleteMinistryCustomer(int customerNo)
        {
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            var querydelete = $"DELETE FROM MISCBILLAPP_GC_CUSTID WHERE CONSUMER_NO={customerNo}";
            var query = $"DELETE FROM MISCBILLAPP_GC_CUSTOMER WHERE CUSTOMER_NO={customerNo}";
            var result = await con.ExecuteAsync(query);
            var resultdelete = await con.ExecuteAsync(querydelete);
            return result;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllDivision()
        {
            List<DropdownResultForStringKey> division = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<Division>("DPG_MINISTRY_CUSTOMER_PKG.GET_DIVISION_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            division = _mapper.Map(result, division);
            return division;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllDistrict(string divisionCode)
        {
            List<DropdownResultForStringKey> district = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_DIVISION_CODE", value: divisionCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<District>("DPG_MINISTRY_CUSTOMER_PKG.GET_DISTRICT_BY_DIVISION", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            district = _mapper.Map(result, district);
            return district;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllUpozila(string districtCode)
        {
            List<DropdownResultForStringKey> upozila = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_DISTRICT_CODE", value: districtCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Upozila>("DPG_MINISTRY_CUSTOMER_PKG.GET_UPAZILA_BY_DISTRICT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            upozila = _mapper.Map(result, upozila);
            return upozila;
        }

        public async Task<List<DropdownResultForStringKey>> GetAllNonBengaliCamp()
        {
            List<DropdownResultForStringKey> nonBengaliList = new List<DropdownResultForStringKey>();
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<NonBengalis>("DPG_MINISTRY_CUSTOMER_PKG.GET_NON_BENGALI_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            nonBengaliList = _mapper.Map(result, nonBengaliList);
            return nonBengaliList;
        }

        public async Task<MinistryCustomerDTOs> GetMinistryCustomerById(string customerNo, string locationCode)
        {
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value: customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            
            var result = con.Query<MinistryCustomerDTOs>("DPG_GC_CUSTOMER.GET_GC_CUSTOMER_BY_ID", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public async Task<MinistryDataViewDTO> GetCustomerData(string customerNo, string locationCode)
        {
            try
            {
                using var con = new OracleConnection(Context.Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value: customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var result = con.Query<MinistryDataViewDTO>("DPG_GC_CUSTOMER.VIEW_CUSTOMER_DATA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MinistryCustomerDTOs> GetMinistryCustomerByCustomerNo(string customerNo)
        {
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUSTOMER_NUMBER", value: customerNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var result = con.Query<MinistryCustomerDTOs>("DPG_GC_CUSTOMER.MINISTRY_CUSTOMER_BY_CUSTNO", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }


        public async Task<List<MinistryWiseCustomerCountDTO>> GetMinistryWiseCustomerCount(string ministryCode, string locationCode)
        {
            using var con = new OracleConnection(Context.Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            oracleDynamicParameters.Add("P_MINISTRY_CODE", value: ministryCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MinistryWiseCustomerCountDTO>("DPG_MINISTRY_CUSTOMER_PKG.GET_MINISTRY_CUST_COUNT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();

            return result;
        }
    }
}
