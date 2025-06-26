namespace Core.Application.Commands.Dbo.RoleAdd;
public class RoleAddCommandValidator : AbstractValidator<RoleAddCommand>
{
    public RoleAddCommandValidator()
    {
        RuleFor(x => x.RoleName).NotEmpty().NotNull();
        //RuleFor(x => x.MenuFkId).NotEmpty().NotNull();
    }
}
