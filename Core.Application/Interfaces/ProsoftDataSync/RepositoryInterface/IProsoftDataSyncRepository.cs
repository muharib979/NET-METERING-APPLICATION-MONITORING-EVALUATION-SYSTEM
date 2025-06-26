using Core.Domain.ProsoftDataSync;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface
{
    public interface IProsoftDataSyncRepository : IBaseRepository<Employee>
    {
        //Task<int> AddListAsync(List<EmployeeDTO> employeeDTOs);
    }
}
