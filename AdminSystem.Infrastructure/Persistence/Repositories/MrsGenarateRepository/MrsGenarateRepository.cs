using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.Temporary;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MrsGenarateRepository
{
    public class MrsGenarateRepository : IMrsGenarateRepository
    {
        private readonly IMapper _mapper;

        public MrsGenarateRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<MRSGenarateDTO>> GetMRSGenarate(string locationCode, string customerNumber, string billCycleCode)
        {
            List<MRSGenarateDTO> mrsgenarate = new List<MRSGenarateDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUSTOMER_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_CYCLE_CODE", value: billCycleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "MISCBILL", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MRSGenarate>("DPG_MISCBILL_MRS_BILL_PRINT.DPD_MRS_GENERATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            mrsgenarate = _mapper.Map(result, mrsgenarate);
            return mrsgenarate;
        }

        public async Task<bool> SaveMrsGenerate(List<MRSGenarateDTO> model)
        {
            bool success = false;
            string[] readingId = model.Select(p => p.ReadingId.ToString()).ToArray();
            string[] meterReadingId = model.Select(p => p.MeterReadingId.ToString()).ToArray();
            string[] custIdArray = model.Select(p => p.CustId.ToString()).ToArray();
            string[] preasentReading = model.Select(p => p.PresentReading).ToArray();
            string[] advanceReading = model.Select(p => p.Advance).ToArray();
            string[] defectiveCode = model.Select(p => p.MeterCondition).ToArray();
            string[] powerFactor = model.Select(p => p.PowerFactor).ToArray();
            string[] userName = model.Select(p => p.UserName).ToArray();

            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            param.Add("P_CUSTOMER_NUM", value: model[0].CustomerNum, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_BILL_CYCLE_CODE", value: model[0].BillCycleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_Meter_Reading_Id", value: meterReadingId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Meter_Reading_Id", value: meterReadingId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Reading_Id", value: readingId, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Cust_Id", value: custIdArray, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Cls_Reading", value: preasentReading, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Advance", value: advanceReading, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Defective_Code", value: defectiveCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_Power_Factor", value: powerFactor, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            param.Add("P_USER", value: userName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_MRS_BILL_PRINT.DPD_MRS_SAVE", param: param, commandType: CommandType.StoredProcedure);
            success = param.Get<int>("O_Status") > 0;
            return success;

        }

        public async Task<List<MRSBillCalculationDTO>> GetMrsBillCalculation(string userLocation, string customerNumber, string billCycleCode)
        {
            List<MRSBillCalculationDTO> mrsCalculation = new List<MRSBillCalculationDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: userLocation, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_CUSTOMER_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_BILL_CYCLE_CODE", value: billCycleCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<MRSBillCalculation>("DPG_MISCBILL_MRS_BILL_PRINT.DPD_BILL_GENERATE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            mrsCalculation = _mapper.Map(result, mrsCalculation);
            return mrsCalculation;
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
    }
}
