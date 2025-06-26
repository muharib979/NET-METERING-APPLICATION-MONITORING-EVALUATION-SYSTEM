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
    public class CustomerCategoryServices : ICustomerCategoryServices
    {
        private readonly IMapper _mapper;
        private readonly ICustomerCategoryRepository _customerCategoryRepository;

        public CustomerCategoryServices(IMapper mapper, ICustomerCategoryRepository customerCategoryRepository)
        {
            _mapper = mapper;
            _customerCategoryRepository = customerCategoryRepository;
        }

        public async Task<List<CustomerCategoryDTO>> GetCustomerCategoryForBillGeneration()
        {
            try
            {
                var result = _mapper.Map<List<CustomerCategoryDTO>>(await _customerCategoryRepository.GetCustomerCategoryForBillGeneration());


                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
