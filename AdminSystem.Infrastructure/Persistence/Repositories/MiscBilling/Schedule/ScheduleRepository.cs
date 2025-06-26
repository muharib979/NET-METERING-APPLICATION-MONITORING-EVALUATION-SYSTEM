using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.Temporary;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Schedule
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IMapper _mapper;
        public ScheduleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<CenBillGroupDTO>> GetScheduleBillGroup()
        {
            List<CenBillGroupDTO> billGroup = new List<CenBillGroupDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CenBillGroup>("DPG_MISCBILL_BILL_CYCLE_SCH.DPD_BILL_GROUP_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            billGroup = _mapper.Map(result, billGroup);
            return billGroup;
        }

        public async Task<List<ScheduleMonthDTO>> GetScheduleMonth(string year)
        {
            List<ScheduleMonthDTO> month = new List<ScheduleMonthDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_YEAR", value: year, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ScheduleMonth>("DPG_MISCBILL_BILL_CYCLE_SCH.DPD_MONTH_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            month = _mapper.Map(result, month);
            return month;
        }

        public async Task<List<ScheduleYearDTO>> GetScheduleYear()
        {
            List<ScheduleYearDTO> year = new List<ScheduleYearDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ScheduleYear>("DPG_MISCBILL_BILL_CYCLE_SCH.DPD_YEAR_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();

            year = _mapper.Map(result, year);
            return year;
        }

        public async Task<int> SaveBillSchedule(BillScheduleDTO model)
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_LOCATION", value: model.LocationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_GROUP", value: model.BillGroup, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_YEAR", value: model.BillYear, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_MONTH", value: model.BillMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_READING_START_DATE", value: model.StartDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_READING_END_DATE", value: model.EndDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_BILL_DATE", value: model.ReadingDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_DUE_DATE", value: model.DueDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: model.UserName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

                var result = con.Query<int>("DPG_MISCBILL_BILL_CYCLE_SCH.DPD_BILL_CYCLE_SCHEDULE_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                return oracleDynamicParameters.Get<int>("O_Status");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
