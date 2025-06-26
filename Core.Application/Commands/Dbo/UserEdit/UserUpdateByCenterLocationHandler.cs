using Core.Application.Commands.Dbo.UserAdd;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Dbo.UserAddByCenterLocation
{
    public class UserUpdateByCenterLocationHandler : IRequestHandler<UserUpdateByCenterLocationCommand, Response<IActionResult>>
    {
        private readonly IUserService _service;

        public UserUpdateByCenterLocationHandler(IUserService service)
        {
            _service = service;
        }
        public async Task<Response<IActionResult>> Handle(UserUpdateByCenterLocationCommand request, CancellationToken cancellationToken)
        {
           var user = await _service.GetByIdAsync(request.Id);
            // var result = await _service.AddUserByCenterLocationAsync(request);
            var result = await _service.UpdateUserByCenterLocationByAsync(user.ID, request);
            return result > 0 ? Response<IActionResult>.Success("User Successfully Updated") : Response<IActionResult>.Fail("Problem saving changes");
        }
    }
}
