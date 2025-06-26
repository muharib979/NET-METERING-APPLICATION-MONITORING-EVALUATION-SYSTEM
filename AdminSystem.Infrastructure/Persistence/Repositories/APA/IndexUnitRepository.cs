using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.APA;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.APA
{
    public class IndexUnitRepository : IIndexUnitRepository
    {
        private readonly IMapper _mapper;
        public IndexUnitRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> DeleteIndexUnitBill(int id)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.DELETE_INDEX_UNIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            int res = oracleDynamicParameters.Get<int>("O_Status");
            return res;
        }

        public async Task<List<IndexUnitDto>> GetIndexUnitDataList()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT * FROM APA_INDEX_UNIT";
            var result = await con.QueryAsync<IndexUnitDto>(sQuery);
            return result.ToList();
        }
        
        public async Task<bool> SaveIndexUnitBill(IndexUnitDto model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_ID", value: model.Id, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAME", value: model.Name, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_NAMEBN", value: model.NameBn, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            //if (model.Id != null)
            //{
            //    var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_INDEX_UNIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            //    isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            //}
            //else
            //{
            //    var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_INDEX_UNIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            //    isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            //}
            var result = con.Query<int>("DPG_MISCBILL_APA_PKG.SAVE_INDEX_UNIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);
        }
    }
}
