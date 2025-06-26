using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Cutomers.ServiceInterfaces;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.Customers
{
    public class CustomerTypeService : ICustomerTypeServices
    {
        private readonly IMapper _mapper;
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public CustomerTypeService(IMapper mapper, ICustomerTypeRepository customerTypeRepository)
        {
            _mapper = mapper;
            _customerTypeRepository = customerTypeRepository;
        }

        public async Task<List<CustomerTypeDTO>> GetCustomerTypeForBillGeneration()
        {
            try
            {
                var result =_mapper.Map<List<CustomerTypeDTO>>( await _customerTypeRepository.GetCustomerTypeForBillGeneration());

                
                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
