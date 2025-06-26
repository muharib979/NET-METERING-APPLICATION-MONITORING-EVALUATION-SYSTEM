using Core.Domain.CustomeEntity;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Cutomers.RepositoryInterfaces
{
    public interface ICustomerCategoryRepository
    {
        Task<List<CustomerCategoryDTO>> GetCustomerCategoryForBillGeneration();
    }
}
