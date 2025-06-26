using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Domain.CustomeEntity;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Customer
{
    public class CustomerCategoryRepository : ICustomerCategoryRepository {
        private readonly IMapper _mapper;

        public CustomerCategoryRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<CustomerCategoryDTO>> GetCustomerCategoryForBillGeneration()
        {
            try
            {
                List<CustomerCategoryDTO> customerCategories = new List<CustomerCategoryDTO> { }; 
                using var con = new OracleConnection(Connection.ConnectionString());
                var result = con.Query<CustomerCategory>("dpg_miscbill_cust.GET_CUST_CATE", commandType: CommandType.StoredProcedure).ToList();
                customerCategories=_mapper.Map(result, customerCategories);
                return customerCategories;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
