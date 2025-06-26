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
    public class ProsoftUserLoginHandler : IRequestHandler<ProsoftUserLoginCommand, Response<ProsoftTokenResponseDto>>
    {
        private readonly IProsoftTokenService _tokenService;

        public ProsoftUserLoginHandler(IProsoftTokenService tokenService) => this._tokenService = tokenService;

        public async Task<Response<ProsoftTokenResponseDto>> Handle(ProsoftUserLoginCommand request, CancellationToken cancellationToken)
        {
            var authToken = new ProsoftTokenResponseDto();
            //Create New JWT and Refresh Token

            var aT = await _tokenService.UserValidate(request).Result
                                    .CreateRefreshtoken().Result
                                    .CreateAccessToken();

            if (aT == null) return Response<ProsoftTokenResponseDto>.Fail("Generating a new token is failed");

            authToken = aT;
            return Response<ProsoftTokenResponseDto>.Success(authToken, "Successfully Generated auth token");

            //if (request.RefreshToken == string.Empty)
            //{
            //    //Create New JWT and Refresh Token

            //    var aT = await _tokenService.UserValidate(request).Result
            //                            .CreateRefreshtoken().Result
            //                            .CreateAccessToken();

            //    if (aT == null) return Response<ProsoftTokenResponseDto>.Fail("Generating a new token is failed");

            //    authToken = aT;
            //    return Response<ProsoftTokenResponseDto>.Success(authToken, "Successfully Generated auth token");
            //}
            //else
            //{
            //    //Refresh JWT and Refresh Refresh Token

            //    var aT = await _tokenService.RefreshTokenValidate(request).Result
            //                            .UserValidateById().Result //.IsCurrentSession(request.SessionId).Result
            //                            .CreateRefreshtoken().Result
            //                            .CreateAccessToken();

            //    if (aT == null) return Response<ProsoftTokenResponseDto>.Fail("Refreshing token is failed");
            //    authToken = aT;
            //    return Response<ProsoftTokenResponseDto>.Success(authToken, "Successfully Generated auth token");
            //}
        }
    }
}
