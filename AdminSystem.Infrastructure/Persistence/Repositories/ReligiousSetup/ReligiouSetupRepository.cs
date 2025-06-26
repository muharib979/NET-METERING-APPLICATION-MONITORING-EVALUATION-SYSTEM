using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.ReligiousSetup;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.ReligiousSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ReligiousSetup
{
    public class ReligiouSetupRepository : IReligiousSetupRepository
    {
        private readonly IMapper _mapper;
        public ReligiouSetupRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> SetupReligious(ReligiousSetupDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_STATUS", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_FROM_MONTH", value: model.FromMonth, (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_TO_MONTH", value: model.ToMonth, (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: model.User, (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);

            var result = con.Query<int>("DPD_RELIGIOUS_REBAIT.REBAIT_SETUP", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_STATUS") > 0;
            return (isSuccess);
        }
    }
}
