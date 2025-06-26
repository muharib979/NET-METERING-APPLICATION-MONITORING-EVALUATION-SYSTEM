namespace Core.Application.Commands.Dbo.UserAdd;

public class UserAddCommandValidator : AbstractValidator<UserAddCommand>
{
    public UserAddCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
    }
}
