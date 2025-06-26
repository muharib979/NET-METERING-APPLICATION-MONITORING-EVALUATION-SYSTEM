using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.VerifyOTP;

public class VerifyOTPCommandHandler : IRequestHandler<VerifyOTPCommand, Response<bool>>
{
    private readonly IForgotPasswordService _forgotPasswordService;
    public VerifyOTPCommandHandler(IForgotPasswordService forgotPasswordService)
    {
        _forgotPasswordService = forgotPasswordService;
    }
    public async Task<Response<bool>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
    {
        if (request != null)
        {
            var success = await _forgotPasswordService.VerifyOtp(request.Code, request.ForgotPasswordMail);

            if (!success) return Response<bool>.Fail("false");
            return Response<bool>.Success(success,"OTP verified");
        }
        return Response<bool>.Fail("Invalid OTP");
    }
}
