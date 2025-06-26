using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.Dbo;
using Core.Domain.MISCBILL;
using Dapper;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class PenaltyBillGenerateRepository : IPenaltyBillGenerateRepository
    {
        private readonly IMapper _mapper;
        private readonly string _billNumber;

        public PenaltyBillGenerateRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<NonConsumerPenaltyBillViewDTO>> GetNonConsumerView(NonConsumerPenaltyBillViewDTO model)
        {
            throw new NotImplementedException();
        }

    

        public async Task<List<PenaltyBillViewDTO>> GetPenaltyView(PenaltyBillViewDTO model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("NID_NUMBR", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("DUE_DATE", value: model.DuaDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.GET_CUSTOMER_PENALTY_BILL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<Returndto> SaveNonConsumerBill(NonConsumerPenaltyBillDTO model)
        {
            try
            {
                Returndto result = new Returndto();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TARIFF", value: model.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_UNIT", value: model.PenaltyUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PEAK_UNIT", value: model.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: model.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_RATE_MULTIPLY", value: model.MultypliBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PRN_AMT", value: model.PrincipleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
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
                oracleDynamicParameters.Add("P_PEAK_PRSNT_READING", value: model.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_PRSNT_READING", value: model.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_REMARK", value: model.p_REMARK, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_Amount", value: model.SrAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_Peak_Amount", value: model.PeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OffPeak_Amount", value: model.OffPeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_LAST_READING_DATE", value:null, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REQUEST_CHANNEL_TYPE", value:"WEB", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                if (model.BillNumber != null)
                {
                    oracleDynamicParameters.Add("P_BILL_NUM", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    result = con.QuerySingle<Returndto>("DPG_MISCBILL_PNL_BIL_NNCST_RGN.DPD_PENALTY_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                    string quesry = $"DELETE FROM MISCBILL_BILL_MST_FILE WHERE BILL_NUMBER ={model.BillNumber}";
                    con.Execute(quesry);
                }

                else
                {
                    result = con.QuerySingle<Returndto>(" DPG_MISCBILL_PNL_BILL_NONCUST.DPD_PENALTY_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
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

                return  result ;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ReturnDTO> SavePenaltyBill(PenaltyBillDTO model)
        {
            try
            {
                ReturnDTO result=new ReturnDTO();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_CUST_NUM", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TARIFF", value: model.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_UNIT", value: model.PenaltyUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PEAK_UNIT", value: model.PeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_UNIT", value: model.OffPeakUnit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_RATE_MULTIPLY", value: model.MultypliBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PRN_AMT", value: model.PrincipleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
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
                oracleDynamicParameters.Add("P_PEAK_PRSNT_READING", value: model.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_PRSNT_READING", value: model.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_REMARK", value: model.p_REMARK, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SR_Amount", value: model.SrAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_Peak_Amount", value: model.PeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OffPeak_Amount", value: model.OffPeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("p_LAST_READING_DATE", value: model.LastReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_REQUEST_CHANNEL_TYPE", value:"WEB", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                if(model.BillNumber!= null)
                {
                    oracleDynamicParameters.Add("P_BILL_NUM", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    result = con.QuerySingle<ReturnDTO>("DPG_MISCBILL_PNL_BILL_CUST_RGN.DPD_PENALTY_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);

                    string quesry = $"DELETE FROM MISCBILL_BILL_MST_FILE WHERE BILL_NUMBER ={model.BillNumber}";
                    con.Execute(quesry);
                }
                else
                {
                     result = con.QuerySingle<ReturnDTO>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_PENALTY_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                }


                OracleDynamicParameters param = new OracleDynamicParameters(); 
                if(model.FileList!= null)
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

        public async Task<ReturnDTO> SavePenaltyBillPrepaid(PenaltyBillPrepaidDTO modal)
        {
            try
            {
                ReturnDTO result = new ReturnDTO();
                //bool isSuccess = true;
                using var conn = new OracleConnection(Connection.ConnectionString());

                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                oracleDynamicParameters.Add("P_CUSTOMER_NUM", value: modal.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUSTOMER_NAME", value: modal.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_FATHER_NAME", value: modal.FatherName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_AREA_CODE", value: modal.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_ID", value: modal.CustId, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_IMPOSED_BY", value: modal.ImposedByCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TARIFF", value: modal.TariffDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BUSINESS_TYPE", value: modal.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BUSINESS_TYPE_CODE", value: modal.BusinessTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_CYCLE_CODE", value: modal.BillCycleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_WALKING_SEQUENCE", value: modal.WlkOrd, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LOCATION_CODE", value: modal.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PHONE", value: modal.MobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_NID", value: modal.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_ADDRESS", value: modal.CustomerAddr, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_READING_DATE", value: modal.ReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_Last_Reading", value: modal.LastReading, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_LAST_BILL_READING_DATE", value: modal.LastBillReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_TYPE_CODE", value: modal.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_TYPE_DESC", value: modal.MeterTypeDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_NUM", value: modal.MeterNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PRV_AC_NO", value: modal.PrvAcNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BOOK_NO", value: modal.BookNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_GROUP", value: modal.BillGroup, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_CODINTION_CODE", value: modal.MeterConditionCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_CODINTION_DESC", value: modal.MeterConditionDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_TYPE", value: modal.MeterType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: modal.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SANC_LOAD", value: modal.SanctionedLoad, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CONN_LOAD", value: modal.ConnectionLoad, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_Peak_Amount", value: modal.PeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OffPeak_Amount", value: modal.OffPeakAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PEAK_PRSNT_READING", value: modal.LastReadingPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_OFF_PEAK_PRSNT_READING", value: modal.LastReadingOffPeak, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TOTAL_BILL_AMOUNT", value: modal.TotalAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PRINCPAL_AMOUNT", value: modal.PrincipleAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_VAT_AMOUNT", value: modal.VatAmount, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: modal.p_USER, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);


                result = conn.QuerySingle<ReturnDTO>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_PENALTY_BILL_PREPAID_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);



                OracleDynamicParameters param = new OracleDynamicParameters();
                if (modal.FileList != null)
                {
                    foreach (var item in modal.FileList)
                    {

                        param.Add("BILL_NO", value: result.BILL_NUM, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("FILE_NAME", value: item.FileNames[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        param.Add("FILE_CONTENT", value: item.FileList[0], dbType: (OracleMappingType?)OracleDbType.Blob, direction: ParameterDirection.Input);
                        param.Add("FILE_TYPE", value: item.FileTypes[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                        conn.Execute("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_BILL_FILE_SAVE", param: param, commandType: CommandType.StoredProcedure);
                    }
                }
                return result;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SavePenaltyExistingConsumer(PenaltyExistingBillDto model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BILL_CHECK_DIGIT", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUSTOMER_NUM", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NUM_CHEK_DIGIT", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_WALKING_SEQUENCE", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CONS_EXTG_NUM", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_NAME", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_ADDRESS", value: model.CustomerAddr, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_PHONE", value: model.MobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NID", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BUS_CODE", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_AREA_CODE", value: model.AreaCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUST_ID", value: model.CustId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_TYPE_CODE", value: "1", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_REASON_CODE", value: model.BillReasonCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USAGE_CAT_CODE", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_READING_DATE", value: model.LastReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_CODE", value: model.MeterTypeCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_NUM", value: model.MeterNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_DIGIT", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_COND", value: model.MeterConditionDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_TYPE", value: model.MeterTypeDesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_METER_STATUS", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_SANC_LOAD", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CONN_LOAD", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TOTAL_BILL_AMOUNT", value: model.TotalAmount, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TOT_PRINCPAL_AMOUNT", value: model.PrincipleAmount, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TOT_VAT_AMOUNT", value: model.VatAmount, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TOT_LPS_AMOUNT", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REMARKS", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_STATUS", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CREATE_BY", value: model.CreateBy, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_FORM_TYPE", value: 0, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input);
            var result = await con.ExecuteAsync("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_PENALTY_EXISTING_BILL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);

        }

  

        public async Task<List<PenaltyBillPrtepaidViewDTO>> GetPenaltyBillPrepaidView(PenaltyBillPrtepaidViewDTO modal)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: modal.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: modal.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("NID_NUMBR", value: modal.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: modal.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<PenaltyBillPrtepaidViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.GET_PENALTY_BILL_PREPAID", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }
}
