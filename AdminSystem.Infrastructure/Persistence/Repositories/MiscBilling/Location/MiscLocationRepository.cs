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

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Location
{
    public class MiscLocationRepository : IMscLocationRepository
    {
        private readonly IMapper _mapper;
        public MiscLocationRepository(IMapper mapper) 
        {
            _mapper= mapper;
        }
        public async Task<List<BlockNumDTO>> GetBlockNumByLocation(string locationCode)
        {
            List<BlockNumDTO> blockNums = new List<BlockNumDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<BlockNum>("DPG_MISCBILL_NEW_CENSUS.DPD_BOOK_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            blockNums = _mapper.Map(result, blockNums);
            return blockNums;
        }

        public async Task<List<LocationDDDTO>> GetLocationAsync()
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var result = con.Query<LocationDDDTO>("DPG_MISCBILL_TEMPORARY_BILL_GEN.GET_LOCATION_DD", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LocationsDTO>> GetLocationByUser(string locationCode)
        {
            try
            {
                List<LocationsDTO> locations = new List<LocationsDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<Locations>("DPG_MISCBILL_NEW_CENSUS.DPD_LOCATION_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                locations = _mapper.Map(result, locations);
                return locations;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
