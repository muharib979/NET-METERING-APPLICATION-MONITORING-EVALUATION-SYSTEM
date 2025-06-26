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

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class TarrifRepository : ITarrifRepository
    {
        private readonly IMapper _mapper;

        public TarrifRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

       

        public async Task<List<TariffDTO>> GetAllNonCustTariff(string locationCode)
        {
            List<TariffDTO> tarrfis = new List<TariffDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Tariff>(" DPG_MISCBILL_PNL_BILL_NONCUST. DPD_TARIFF_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            tarrfis = _mapper.Map(result, tarrfis);
            return tarrfis;
        }

        public async Task<List<TariffDTO>> GetAllTariff()
        {
            try
            {
                List<TariffDTO> tariff = new List<TariffDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<Tariff>("DPG_MISCBILL_NEW_CENSUS.DPD_TARIFF_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                tariff = _mapper.Map(result, tariff);
                return tariff;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
