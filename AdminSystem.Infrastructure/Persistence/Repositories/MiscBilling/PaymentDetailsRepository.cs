using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Dapper.Oracle;
using MediatR;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class PaymentDetailsRepository : IPaymentDetailsRepository
    {
       

        public async Task<List<PaymentDetailsDTO>> GetPenaltyPaymentDetails()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var result = con.Query<PaymentDetailsDTO>("", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }
}
