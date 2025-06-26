using Core.Application.Commands.Dbo.UserAdd;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Dbo.UserAddByCenterLocation
{
    public class UserAddByCenterLocationHandler : IRequestHandler<UserAddByCenterLocationCommand, Response<IActionResult>>
    {
        private readonly IUserService _service;

        public UserAddByCenterLocationHandler(IUserService service)
        {
            _service = service;
        }
        public async Task<Response<IActionResult>> Handle(UserAddByCenterLocationCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.AddUserByCenterLocationAsync(request);

            return result > 0 ? Response<IActionResult>.Success("User Successfully Created") : Response<IActionResult>.Fail("Problem saving changes");
        }
    }
}
