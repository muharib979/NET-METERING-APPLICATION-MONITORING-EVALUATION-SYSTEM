using Core.Application.Commands.ProsoftDataSync.AddProsoftData;
using Shared.DTOs.Building;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.ServiceInterface
{
    public interface IProsoftDataSyncService : IBaseService<EmployeeDTO>
    {
        //Task<int> AddListAsync(AddProsoftDataCommand command);
    }
}
