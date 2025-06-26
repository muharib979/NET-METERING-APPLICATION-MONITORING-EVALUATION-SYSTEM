using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Domain.CustomeEntity;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Customer
{
    internal class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly IMapper _mapper;

        public CustomerTypeRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<CustomerTypeDTO>> GetCustomerTypeForBillGeneration()
        {
            try
            {
                List<CustomerTypeDTO> customerTypes= new List<CustomerTypeDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                var param = new
                {
                    OUT_PARAM = (OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output)
                };
                 var result = con.Query<CustomerType>("dpg_miscbill_cust.GET_CUST_TYPE", commandType: CommandType.StoredProcedure).ToList();
                customerTypes = _mapper.Map(result, customerTypes);
                return customerTypes;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
