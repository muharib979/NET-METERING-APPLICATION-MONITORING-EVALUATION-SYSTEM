using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.APA;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection = AdminSystem.Infrastructure.Persistence.Context.Connection;

namespace AdminSystem.Infrastructure.Persistence.Repositories.APA
{
    public class UnitIndexTargetRepository : IUnitIndexTargetRepository
    {
        private readonly IMapper _mapper;
        public UnitIndexTargetRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<TargetDto>> GetAllTargetDataByPerformanceCode(string performCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"select T.FISCALYEAR_CODE FISCALYEARCODE, T.PERFORMANCE_INDEX_CODE PERFORMCODE, 
                            T.VALUE TARGETVALUE from MISCBILLAPP_APA_TARGET T 
                            where PERFORMANCE_INDEX_CODE=:performanceCode";
            var result = await con.QueryAsync<TargetDto>(sQuery, new { performanceCode = performCode});
            return result.ToList();
        }

        public async Task<List<UnitIndexAndPerformValueDto>> GetAllUnitIndexAndPerformValueByIndexCode(string indexCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = @"SELECT P.NAME, P.NAMEBN, P.CODE, U.NAMEBN UNITNAMEBN, U.CODE UNITCODE, 
                            P.VALUE FROM APA_INDEX_UNIT U INNER JOIN 
                            APA_PERFORMANCE_INDEX P ON P.INDEX_UNIT_CODE = U.CODE 
                            WHERE P.INDEX_UNIT_CODE = :indexCode";
            var result = await con.QueryAsync<UnitIndexAndPerformValueDto>(sQuery, new { indexCode = indexCode });
            return result.ToList();
        }
    }
}
