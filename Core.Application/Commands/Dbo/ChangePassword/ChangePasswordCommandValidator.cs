namespace Core.Application.Commands.Dbo.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().NotNull();
        RuleFor(x => x.NewPassword).NotEmpty().NotNull();
    }
}
