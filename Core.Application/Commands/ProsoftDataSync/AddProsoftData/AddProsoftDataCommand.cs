using Shared.DTOs.Building;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ProsoftDataSync.AddProsoftData
{
    public class AddProsoftDataCommand : List<EmployeeDTO>, IRequest<Response<IActionResult>>
    {
    }
}
