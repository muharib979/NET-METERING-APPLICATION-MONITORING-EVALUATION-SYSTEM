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
    public class BillDetailsRepository: IBillDetailsRepository<CustomerBillDetailsDTO>
    {
        private readonly IMapper _mapper;

        public BillDetailsRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<CustomerBillDetailsDTO>> BillDetailsByCustomer(string customerNumber, string locationCode)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.Add("P_CUST_NUMBER", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            parameters.Add("P_LOCATION_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<CustomerBillDetailsDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_CUST_BILL_DETAILS", param: parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }
}
