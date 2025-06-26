using CFEMS.API.Controllers.Common;
using Core.Application.Commands.Dbo.ChangePassword;
using Core.Application.Commands.Dbo.DeleteExpireRefreshToken;
using Core.Application.Commands.Dbo.GetVerificationCode;
using Core.Application.Commands.Dbo.UserLogin;
using Core.Application.Commands.Dbo.VerifyOTP;
using Microsoft.AspNetCore.SignalR;

namespace CFEMS.API.Controllers.Dbo;

public class AuthController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator _mediatr => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    [AllowAnonymous]
    [HttpPost("/api/auth/signin")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand command) => Ok(await _mediatr.Send(command));
      


    //[AllowAnonymous]
    //[HttpPost("get-verification-code")]
    //public async Task<IActionResult> GetVerificationCode([FromBody] GetVerificationCodeCommand command) => Ok(await _mediatr.Send(command));
    //[AllowAnonymous]
    //[HttpPost("verify-otp")]
    //public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPCommand command) => Ok(await _mediatr.Send(command));
    //[AllowAnonymous]
    //[HttpPost("change-password")]
    //public async Task<IActionResult> ChangePassword(ChangePasswordCommand command) => Ok(await _mediatr.Send(command));
    //[AllowAnonymous]
    //[HttpGet("delete-expire-refresh-token")]
    //public async Task<IActionResult> DeleteExpireRefreshToken(string apiKey)
    //{
    //    if (apiKey == "1c852fac-4271-42b7-b0x91-48d7579ecdee")
    //    {
    //        return Ok(await _mediatr.Send(new DeleteExpireRefreshTokenCommand()));
    //    }
    //    else
    //    {
    //        return Unauthorized();
    //    }

    //}
}

