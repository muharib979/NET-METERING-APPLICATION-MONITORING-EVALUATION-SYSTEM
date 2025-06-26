using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ProsoftDataSync.ProsoftUserLogin
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Response<ProsoftTokenResponseDto>>
    {
        private readonly IProsoftUserService userService;
        private readonly IConfiguration configuration;
        private readonly IProsoftTokenService _tokenService;

        public RefreshTokenHandler(IProsoftUserService userService, IConfiguration configuration, IProsoftTokenService tokenService)
        {
            this.userService = userService;
            this.configuration = configuration;
            this._tokenService = tokenService;
        }
        public async Task<Response<ProsoftTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var authToken = new ProsoftTokenResponseDto();
            //Refresh JWT and Refresh Refresh Token

            var aT = await _tokenService.RefreshTokenValidate(request).Result
                                    .UserValidateById().Result //.IsCurrentSession(request.SessionId).Result
                                    .CreateRefreshtoken().Result
                                    .CreateAccessToken();

            if (aT == null) return Response<ProsoftTokenResponseDto>.Fail("Refreshing token is failed");
            authToken = aT;
            return Response<ProsoftTokenResponseDto>.Success(authToken, "Successfully Generated auth token");
        }
    }
}
