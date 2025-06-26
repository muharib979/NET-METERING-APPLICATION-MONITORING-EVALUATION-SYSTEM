using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.AppUserManagement;
using Core.Domain.AppUserManagement;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.AppuserManagement
{
    public class AppUserManagementRepository : IAppUserManagement
    {
        private readonly IMapper _mapper;
        public AppUserManagementRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> DeleteAppMangementBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_APP_MANAGEMENT_PKG.DELETE_APP_MANAGEMENT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

        public async Task<List<AppUserDesignationDTO>> GetAppUserDesignationList()
        {
            List<AppUserDesignationDTO> designation = new List<AppUserDesignationDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<AppUserDesignation>("DPG_APP_MANAGEMENT_PKG.APP_USER_DESIGNATION_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            designation = _mapper.Map(result, designation);
            return designation;
        }

        public async Task<List<AppUserManagementDTO>> GetAppUserManagementList()
        {
            List<AppUserManagementDTO> appmanagement = new List<AppUserManagementDTO>();
            List<AppUserManagementDTO> appmanagementss = new List<AppUserManagementDTO>();

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            string query = $"SELECT USER_CODE UserCode,USER_NAME UserName , Location_code Location from MISCBILL_APP_USER_MST";
            appmanagement = con.Query<AppUserManagementDTO>(query).ToList();
            
             
            param.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<AppUserManagement>("DPG_APP_MANAGEMENT_PKG.APP_MANAGEMENT_LIST", param: param, commandType: CommandType.StoredProcedure).ToList();
            appmanagement = _mapper.Map(result, appmanagement);
            return appmanagement;
        }

        public async Task<int> SaveAppUserManagementBill(AppUserManagementDTO model)
        {
            int isSuccess = 0;
            using var con = new OracleConnection(Connection.ConnectionString());
            string checkQuerey = $"Select COUNT(*) from MISCBILL_APP_USER_MST WHERE USER_CODE='{model.UserCode}' AND USER_ID <> {model.UserId}";
            var isExists = con.QueryFirstOrDefault<int>(checkQuerey);
            if (isExists > 0)
            { return isSuccess = 2; }

            else
            {
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();

                oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER_ID", value: model.UserId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER_CODE", value: model.UserCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER_NAME", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_EMAIL", value: model.Email, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MOBILE_NO", value: model.MobileNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER_PASSWORD", value: model.UserPassword, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DESIG_CODE", value: model.DesignationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DB_CODE", value: model.Db, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.Location, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var result = con.Query<int>("DPG_APP_MANAGEMENT_PKG.SAVE_APP_MANAGEMENT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                isSuccess = oracleDynamicParameters.Get<int>("O_Status");

                return (isSuccess);
            }
        }



    }

}
