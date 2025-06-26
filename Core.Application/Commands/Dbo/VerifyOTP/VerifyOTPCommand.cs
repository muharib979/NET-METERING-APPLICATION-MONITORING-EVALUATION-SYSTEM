using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.VerifyOTP;

public class VerifyOTPCommand : IRequest<Response<bool>>
{
    public int Code { get; set; }
    public string ForgotPasswordMail { get; set; }
}
