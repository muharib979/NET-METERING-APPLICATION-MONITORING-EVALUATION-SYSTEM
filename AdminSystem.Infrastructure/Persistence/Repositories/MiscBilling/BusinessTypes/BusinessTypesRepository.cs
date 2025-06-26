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

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.BusinessTypes
{
    public class BusinessTypesRepository : IBusinessTypeRepository
    {
        private readonly IMapper _mapper;
        public BusinessTypesRepository(IMapper mapper)
        {
            _mapper= mapper;
        }
        public async Task<List<BusinessTypeDTO>> GetAllBusinessType()
        {
            try
            {
                List<BusinessTypeDTO> businessType = new List<BusinessTypeDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<BusinessType>("DPG_MISCBILL_NEW_CENSUS.DPD_BUS_TYPE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                businessType = _mapper.Map(result, businessType);
                return businessType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
