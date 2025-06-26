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

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Meter
{
    public class MeterRepository : IMeterRepository
    {
        private readonly IMapper _mapper;
        public MeterRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<MeterCondionDTO>> GetAllMeterCondition()
        {
            try
            {
                List<MeterCondionDTO> meterCondion = new List<MeterCondionDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<MeterCondition>("DPG_MISCBILL_NEW_CENSUS.DPD_METER_COND_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                meterCondion = _mapper.Map(result, meterCondion);
                return meterCondion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MeterOwnerDTO>> GetAllMeterOwner()
        {
            try
            {
                List<MeterOwnerDTO> meterOwner = new List<MeterOwnerDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<MeterOwner>("DPG_MISCBILL_NEW_CENSUS.DPD_METER_OWNER_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                meterOwner = _mapper.Map(result, meterOwner);
                return meterOwner;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MeterTypeDTO>> GetAllMeterType()
        {
            try
            {
                List<MeterTypeDTO> meterType = new List<MeterTypeDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<MeterType>("DPG_MISCBILL_NEW_CENSUS.DPD_METER_TYPE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                meterType = _mapper.Map(result, meterType);
                return meterType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
