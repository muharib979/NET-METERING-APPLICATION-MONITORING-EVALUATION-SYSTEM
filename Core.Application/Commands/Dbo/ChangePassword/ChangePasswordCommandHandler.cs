using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Response<IActionResult>>
{
    private readonly IForgotPasswordService _forgotPasswordService;
    public ChangePasswordCommandHandler(IForgotPasswordService forgotPasswordService)
    {
        _forgotPasswordService = forgotPasswordService;
    }

    public async Task<Response<IActionResult>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _forgotPasswordService.ChangePassword(request);

        return result> 0 ? Response<IActionResult>.Success("Password have changed successfully") : Response<IActionResult>.Fail("Password have changed successfully");
    }
}
