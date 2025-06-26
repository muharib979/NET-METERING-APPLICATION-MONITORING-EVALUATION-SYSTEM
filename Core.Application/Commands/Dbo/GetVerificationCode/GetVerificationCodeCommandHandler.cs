using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.GetVerificationCode;

public class GetVerificationCodeCommandHandler : IRequestHandler<GetVerificationCodeCommand, Response<bool>>
{
    private readonly IForgotPasswordService _forgotPasswordService;
    public GetVerificationCodeCommandHandler(IForgotPasswordService forgotPasswordService) => _forgotPasswordService = forgotPasswordService;

    public async Task<Response<bool>> Handle(GetVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Email))
        {
            var otp = await _forgotPasswordService
                .CheckUserWithEmail(request.Email).Result
                .GenerateOtp().Result
                .SendEmail();

            if (!otp) Response<bool>.Fail("Email sending failed");
            return Response<bool>.Success(otp,"Invalid Email Error");
        }
        return Response<bool>.Fail("Invalid Email Error");
    }
}
