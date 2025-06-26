using Core.Application.Interfaces.VisitorDetails;
using Core.Domain.Dbo;
using Microsoft.AspNetCore.Http;
using Nancy;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;

namespace Core.Application.Commands.Dbo.UserLogin;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ResponseNem<TokenResponseDto>>
{
    private readonly ITokenService _tokenService;
    private readonly IVisitorDetails _visitorDetails;

    //public UserLoginCommandHandler(ITokenService tokenService) => _tokenService = tokenService;
    public UserLoginCommandHandler(ITokenService tokenService, IVisitorDetails visitorDetails) 
    {
        _tokenService = tokenService;
        _visitorDetails = visitorDetails;
    }

    public async Task<ResponseNem<TokenResponseDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var authToken = new TokenResponseDto();
        if (request.UserName != "")
        {
            //Create New JWT and Refresh Token
            ////UserValidate(request).Result

            var aT = await _tokenService.UserValidate(request).Result
                                    .CreateRefreshtoken("").Result
                                    .CreateAccessToken();
            // Create visitor details information
            var visitor = _visitorDetails.SaveVisitorDetails(request.UserName).Result;

            if (aT == null) return ResponseNem<TokenResponseDto>.Fail("Generating a new token is failed");

            authToken = aT;
            return ResponseNem<TokenResponseDto>.Success(authToken, "Successfully Generated auth token");
        }
        else
        {

            if (string.IsNullOrEmpty(request.UserName))
            {
                //var visitor = _visitorDetails.NemCheckValidate(request).Result;

                                    var errors = new List<ErrorDetail>
                            {
                                new ErrorDetail { Code = "400.1", Message = "Username is required." },
                                new ErrorDetail { Code = "400.1", Message = "Password is required." }
                            };

                return ResponseNem<TokenResponseDto>.Fails("Generating a new token failed", errors);

            }
            return ResponseNem<TokenResponseDto>.Fail("df");
            //if (string.IsNullOrEmpty(request.Password))
            //{
            //    var errorResponse = new TokenResponseDto
            //    {
            //        Status = 400,
            //        Data = null,
            //        Errors = new List<ErrorDetail>
            //    {
            //        new ErrorDetail
            //        {
            //            Code = "400.1",
            //            Message = "Password  is required."
            //        }
            //    }
            //    };

            //    return Response<TokenResponseDto>.Fail("Password  is required");
            //}

            //if (request.UserName != "username" || request.Password != "password")
            //{
            //    var errorResponse = new TokenResponseDtos
            //    {
            //        Status = 401,
            //        Data = null,
            //        Errors = new List<ErrorDetail>
            //    {
            //        new ErrorDetail
            //        {
            //            Code = "401.1",
            //            Message = "Username or password is invalid."
            //        }
            //    }
            //    };

            //    return Response<TokenResponseDto>.Fail("Username or password is invalid.");
            //}

            //return Response<TokenResponseDto>.Fail();



            //if (string.IsNullOrEmpty(request.UserName))
            //{
            //    errors.Add(new ErrorDetail { Code = "400.1", Message = "Username is required." });
            //}

            //if (string.IsNullOrEmpty(request.Password))
            //{
            //    errors.Add(new ErrorDetail { Code = "400.1", Message = "Password is required." });
            //}




            //}




        }
   

 


    }
}
