using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class TemporaryBillRepository : ITemporaryBillRepository
    {
        private readonly IMapper _mapper;
        public TemporaryBillRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
 
        public async Task<List<FeederDTO>> GetAllFeeder(string locationCode)
        {
            try
            {
                List<FeederDTO> feeder = new List<FeederDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<Feeder>("DPG_MISCBILL_NEW_CENSUS.DPD_FEEDER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                feeder = _mapper.Map(result, feeder);
                return feeder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public async Task<List<InitialReadingDTO>> GetInitialReading(string meterTypeCode)
        {
            List<InitialReadingDTO> initialReading = new List<InitialReadingDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_MTR_TYPE", value: meterTypeCode, dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<InitialReading>("DPG_MISCBILL_NEW_CENSUS.DPD_INITIAL_READING_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            initialReading = _mapper.Map(result, initialReading);
            return initialReading;
        }

        public async Task<bool> SaveCensusBill(CensusBillDTO model)

        {
            try
            {
                string[]? todCodeArray = model.InitialReadingList.Select(p => p.TodCode).ToArray();
                string[]? timeCycleCodeArray = model.InitialReadingList.Select(p => p.TimeCycleCode).ToArray();
                string[]? readingTypeCodeArray = model.InitialReadingList.Select(p => p.ReadingTypeCode).ToArray();
                decimal[]? readingArray = model.InitialReadingList.Select(p => p.Reading).ToArray();
                string[]? readingDateArray = model.InitialReadingList.Select(p => p.ReadingDate.ToString("dd/MM/yyyy")).ToArray();

                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BOOK", value: model.BlcokNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_GRP", value: model.BillGroup, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_PREV_ACC", value: model.OldAcNo, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_WALK_ORDER", value: model.WalkOrder, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_NAME", value: model.CustomerName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_FATHER_HUSBAND_NAME", value: model.FatherName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CUST_ADDR", value: model.Address, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_NID", value: model.NidNUmber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MOBILE", value: model.MobileNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_OWNER", value: model.MeterOwner, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_TYPE", value: model.MeterType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_NUM", value: model.MeterNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_DIGIT", value: model.MeterDigit, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_COND", value: model.MeterCondition, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_METER_OMF", value: model.Omf, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TARIFF", value: model.Tarrif, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_SANCTIONED_LOAD", value: model.SanctionLoad, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_CONNECTED_LOAD", value: model.ConnectedLoad, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MONTHLY_LIKELY", value: model.MonthlyConsumption, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BUS_TYPE_CODE", value: model.BusinessType, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_XFORMER_NUM", value: model.FormerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_XFORMER_CAPACITY", value: model.FormerCapacity, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_FEEDER_NO", value: model.Feederdesc, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TOD_CODE", value: todCodeArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
                oracleDynamicParameters.Add("P_TIME_CYCLE_CODE", value: timeCycleCodeArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
                oracleDynamicParameters.Add("P_READING_TYPE_CODE", value: readingTypeCodeArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
                oracleDynamicParameters.Add("P_READING", value: readingArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
                oracleDynamicParameters.Add("P_READING_DATE", value: readingDateArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
                oracleDynamicParameters.Add("P_USER", value:model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.QuerySingle<ReturnDTO>("DPG_MISCBILL_NEW_CENSUS.DPD_NEW_CENSUS_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                if (result.STATUS == 1)
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<CustomerCensusList>> GetCensusCustomerList(string locationCode)
        {
            List<CustomerCensusList> censusList = new List<CustomerCensusList>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CustomerCensusList>("DPG_MISCBILL_CENSUS_CUSTOMS.DPD_CENSUS_CUSTOMER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<MRSBillPrintDTO> GetMrsBillReport(string locationCode, string customerNumber, string billNumber)
        {
            MRSBillPrintDTO mrsReport = new MRSBillPrintDTO();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_CUST_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_NUM", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MRSBillPrint>("DPG_MISCBILL_MRS_BILL_PRINT.DPD_BILL_REPORT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            mrsReport = _mapper.Map(result.FirstOrDefault(), mrsReport);
            return mrsReport;
        }

        public async Task<List<TemporaryBillViewDTO>> GetTemporaryBillView(TemporaryBillViewDTO model)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("CUST_NUMBER", value: model.CustomerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("BILL_NUMBER", value: model.BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("NID_NUMBR", value: model.NidNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("DUE_DATE", value: model.DuaDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_LOCATION_CODE", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<TemporaryBillViewDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.GET_CUSTOMER_TEMPORARY_BILL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public Task<List<ScheduleMonthDTO>> GetScheduleMonth(string year)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveBillSchedule(BillScheduleDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
