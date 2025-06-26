using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Domain.CustomeEntity;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Customer
{
    public class CustomerTariffRepository : ICustomerTariffRepository
    {
        private IMapper _mapper;
        public CustomerTariffRepository(IMapper mapper)
        {
            _mapper= mapper;
        }

        public async Task<List<CustomerTariffDto>> GetCustomerTariff()
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var result = con.Query<CustomerTariff>("DPG_GC_CUSTOMER.ALL_TARRIF_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                // var result = con.Query<CustomerTariff>("Sp_GetAllCustCategoryForBillGeneration", commandType: CommandType.StoredProcedure);
                // var result = con.Query<CustomerTariff>("select * from MISCBILL_TARIFF");
                 var customerTariff = _mapper.Map<List<CustomerTariffDto>>(result);
                 return customerTariff.ToList();
             

                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public Task<int> AddAsync(CustomerTariff entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<CustomerTariff> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomerTariff>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerTariff> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(CustomerTariff entity)
        {
            throw new NotImplementedException();
        }
    }
}
