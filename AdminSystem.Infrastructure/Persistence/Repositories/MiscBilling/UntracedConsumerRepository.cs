using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.MiscBilling;
using Dapper.Oracle;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class UntracedConsumerRepository : IUntracedConsumerRepository
    {
        private readonly IDatabaseConfigRepository _dbConfigRepository;

        public UntracedConsumerRepository(IDatabaseConfigRepository databaseConfigRepository)
        {
            _dbConfigRepository = databaseConfigRepository;
        }

        public async Task<bool> ChangeConsumerStatus(int id,int custStatus,string updatedBy)
        {
            bool success = false;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            param.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            param.Add("P_CUST_Status", value: custStatus, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            param.Add("P_UPDATED_BY", value: updatedBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_UPDATED_DATE", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = await con.ExecuteAsync("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_UPDATE_CONSUMER_STATUS", param: param, commandType: CommandType.StoredProcedure);
            success = param.Get<int>("O_Status") > 0;
            return success;
        }

        public async Task<List<UntracedConsumerDTO>> GetAllConsumer()
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var result = await con.QueryAsync<UntracedConsumerDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_GET_ALL_CONSUMER", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                result = result.ToList();
                return (List<UntracedConsumerDTO>)result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UntracedConsumerDTO> GetConsumerByCustNumber(string custNumber)
        {
            try
            {

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUST_NUMBER", value: custNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<UntracedConsumerDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_GET_CONSUMER_BY_CUSTNUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        public async  Task<List<UntracedCustomerViewDTO>> GetConsumerByDate(string startDate, string endDate, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_FROM_START_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TO_END_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<UntracedCustomerViewDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_UNTRACEDCUST_BY_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;

        }

        public async Task<List<UntracedConsumerDTO>> GetConsumerSearchByDate(string startDate, string endDate, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_FROM_START_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TO_END_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<UntracedConsumerDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_UNTRACEDCUSTSEARCH_BY_DATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;

        }

        public async Task<bool> SaveUntracedConsumer(UntracedConsumerDTO model)
        {
            try
            {
                bool success = false;
                UntracedConsumerDTO data = new UntracedConsumerDTO();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUST_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                data = con.Query<UntracedConsumerDTO>("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_GET_CONSUMER_BY_CUSTNUM", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                DropdownResultForStringKey dbCode = await _dbConfigRepository.GetDbByLocationCode(model.LocationCode);

                if (data == null)
                {
                    OracleDynamicParameters param = new OracleDynamicParameters();
                    string query = $"SELECT NAME FROM MISCBILLAPP_MS_LOCATION WHERE CODE='{model.LocationCode}'";
                    model.LocationName=con.QueryFirstOrDefault<string>(query); 
                    param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                    param.Add("P_CUST_ID", value: model.CustId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_CUSTOMER_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_CUSTOMER_NAME", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_LOCATION_NAME", value: model.LocationName, dbType: (OracleMappingType?)OracleDbType.NVarchar2, direction: ParameterDirection.Input);
                    param.Add("P_CUSTOMER_ADDRESS", value: model.CustomerAddr, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_BUSINESS_TYPE", value: model.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_TARIFF_ID", value: model.TariffId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_TARIFF_DESCRIPTION", value: model.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_AREA_CODE", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_PREVIOUS_ACC_NO", value: model.PrvAcNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_METER_NUMBER", value: model.MeterNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_METER_TYPE_DESC", value: model.MeterTypeDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_METER_CONDITION", value: model.MeterConditionDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_STATUS", value: 1, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
                    param.Add("P_CREATED_BY", value: model.CreatedBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_CREATED_DATE", value: model.CreatedDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_UPDATED_BY", value: model.UpdatedBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_UPDATED_DATE", value: model.UpdatedDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_UC_TYPE", value: model.UcType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    param.Add("P_DB_CODE", value: dbCode.Key, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    var result = await con.ExecuteAsync("DPG_MISCBILL_UNTRACED_CONSUMER.DPD_SAVE_UNTRACED_CONSUMER", param: param, commandType: CommandType.StoredProcedure);
                    success = param.Get<int>("O_Status") > 0;
                }

                return success;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
