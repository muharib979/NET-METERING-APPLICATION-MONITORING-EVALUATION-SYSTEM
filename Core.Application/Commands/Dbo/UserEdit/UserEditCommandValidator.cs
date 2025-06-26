namespace Core.Application.Commands.Dbo.UserEdit;

public class UserEditCommandValidator : AbstractValidator<UserEditCommand>
{
    public UserEditCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull();
        RuleFor(x => x.RoleId).NotEmpty().NotNull();
        RuleFor(x => x.UserName).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
    }
}
