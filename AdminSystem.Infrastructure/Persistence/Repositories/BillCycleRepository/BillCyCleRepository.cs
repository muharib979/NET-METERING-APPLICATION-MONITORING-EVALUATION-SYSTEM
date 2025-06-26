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

namespace AdminSystem.Infrastructure.Persistence.Repositories.BillCycleRepository
{
    public class BillCyCleRepository : IBillCycleRepository
    {
        private readonly IMapper _mapper;

        public BillCyCleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<BillCycleDTO>> GetAllBillCycle()
        {
            List<BillCycleDTO> billCycle = new List<BillCycleDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<BillCycle>("DPG_MISCBILL_MRS_BILL_PRINT.DPD_BILL_CYCLE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();

            billCycle = _mapper.Map(result, billCycle);
            return billCycle;
        }

        public async Task<List<BillCycleListDTO>> GetBillCycleList()
        {
          
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<BillCycleListDTO>("DPG_MISCBILL_CENSUS_CUSTOMS.DPD_BILL_CYCLE_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);

            return (List<BillCycleListDTO>)result;
        }
    }
}
