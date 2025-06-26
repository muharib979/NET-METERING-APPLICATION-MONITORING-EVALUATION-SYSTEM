using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class ImposedByRepository:IImposedByRepository
    {
        private readonly IMapper _mapper;

        public ImposedByRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<ImposedByDTO>> GetImposedBy()
        {
            var someParameter = "Infonet1";
            List<ImposedByDTO> imposedBies = new List<ImposedByDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER", value:someParameter, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<ImposedBy>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_IMPOSED_BY_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            imposedBies = _mapper.Map(result, imposedBies);
            return imposedBies;
        }
    }
}
