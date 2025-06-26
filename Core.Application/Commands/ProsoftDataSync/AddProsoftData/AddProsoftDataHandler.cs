using Core.Application.Commands.Building.AddBuilding;
using Core.Application.Interfaces.Building.ServiceInterfaces;
using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ProsoftDataSync.AddProsoftData
{
    public class AddProsoftDataHandler : IRequestHandler<AddProsoftDataCommand, Response<IActionResult>>
    {
        private readonly IProsoftDataSyncService _service;
        public AddProsoftDataHandler(IProsoftDataSyncService service)
        {
            _service = service;
        }

        public async Task<Response<IActionResult>> Handle(AddProsoftDataCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                int result = await _service.AddListAsync(request);
                return result > 0 ? Response<IActionResult>.Success("Data Added Successfully") : Response<IActionResult>.Fail("Problem Saving Changes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Response<IActionResult>.Fail("Problem Saving Changes");
            }
        }
    }
}
