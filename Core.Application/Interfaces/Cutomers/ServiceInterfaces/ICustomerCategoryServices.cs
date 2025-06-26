using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Cutomers.ServiceInterfaces
{
    public interface ICustomerCategoryServices
    {
        Task<List<CustomerCategoryDTO>> GetCustomerCategoryForBillGeneration();
    }
}
