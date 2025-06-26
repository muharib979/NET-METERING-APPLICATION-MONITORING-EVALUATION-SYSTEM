using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class DcRcBillGenerateRepository : IDcRcBillGenerateRepository
    {
        private readonly IMapper _mapper;


        public DcRcBillGenerateRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<DcTypeDTO>> GetAllDcType()
        {
            try
            {
                List<DcTypeDTO> dcType = new List<DcTypeDTO>();
                var pUser = "MISCBILL";
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: pUser, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<DcType>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_DC_TYPE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                dcType = _mapper.Map(result, dcType);
                return dcType;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<DcRcBillViewDTO>> GetDcRcView(DcRcBillViewDTO model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("NID_NUMBR", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<DcRcBillViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.GET_CUSTOMER_DCRC_BILL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<ReturnDTO> SaveDcRcBill(DcRcBillDTO model)
        {
            try
            {
                ReturnDTO result = new ReturnDTO();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUST_NUM", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TARIFF", value: model.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_UNIT", value: null, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PEAK_UNIT", value: model.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: model.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DC_TYPE_CODE", value: model.DcTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PRN_AMT", value: model.DcAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_VAT_AMT", value: model.VatAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_AREA_CODE", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BUS_TYPE_CODE", value: model.BusinessTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_NAME", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_ADDR", value: model.CustomerAddr, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_COND", value: model.MeterConditionCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_NUM", value: model.MeterNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_TYPE", value: model.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PREV_ACC", value: model.PrvAcNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: model.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_IMPOSED_BY_CODE", value: model.ImposedByCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MOBILE", value: model.MobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_NID", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_PRSNT_READING", value: model.LastReading, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);                
                oracleDynamicParameters.Add("P_PEAK_PRSNT_READING", value:null, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_PRSNT_READING", value: model.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_REMARK", value: model.p_REMARK, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_Amount", value: model.SrAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_Peak_Amount", value: model.PeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OffPeak_Amount", value: model.OffPeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DC_DATE", value: model.DcDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_LAST_READING_DATE", value: model.LastReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REQUEST_CHANNEL_TYPE", value: "WEB", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                if (model.BillNumber != null)
                {
                    oracleDynamicParameters.Add("P_BILL_NUM", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    result = con.QuerySingle<ReturnDTO>("DPG_MISCBILL_DC_RC_BIL_CST_RGN.DPD_DCRC_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                    string quesry = $"DELETE FROM MISCBILL_BILL_MST_FILE WHERE BILL_NUMBER ={model.BillNumber}";
                    con.Execute(quesry);

                }
                else
                {
                    result = con.QuerySingle<ReturnDTO>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_DCRC_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);

                }


                OracleDynamicParameters param = new OracleDynamicParameters();
                if (model.FileList != null)
                {
                    foreach (var item in model.FileList)
                    {
                        param.Add("BILL_NO", value: result.BILL_NUM, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("FILE_NAME", value: item.FileNames[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("FILE_CONTENT", value: item.FileList[0], dbType: (OracleMappingType?)OracleDbType.Blob, direction: ParameterDirection.Input);
                        param.Add("FILE_TYPE", value: item.FileTypes[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        con.Execute("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_FILE_SAVE", param: param, commandType: CommandType.StoredProcedure);
                    }
                }

                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> UpdateRcDate(string billNumber, string customerNumber, string rcDate,string userName)
        {
            bool isSuccess=false;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NO", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_RC_DATE", value: rcDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: userName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_DCRC_BILL_STATUS_UPDATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);
        }
    }
}
