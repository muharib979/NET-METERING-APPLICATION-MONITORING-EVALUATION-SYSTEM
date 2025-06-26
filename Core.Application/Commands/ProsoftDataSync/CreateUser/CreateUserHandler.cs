using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ProsoftDataSync.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<RegistrationResponseDTO>>
    {
        private readonly IProsoftUserService userService;
        private readonly IMapper mapper;

        public CreateUserHandler(IProsoftUserService userService,IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        public async Task<Response<RegistrationResponseDTO>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            int isSave = await userService.AddProsoftUser(command);
            return isSave > 0 ? Response<RegistrationResponseDTO>.Success("User Saved Successfully") : Response<RegistrationResponseDTO>.Fail("User Not save");
            
        }
    }
}
