namespace Core.Application.Commands.Dbo.RoleEdit;

public class RoleEditCommandValidator : AbstractValidator<RoleEditCommand>
{
    public RoleEditCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.RoleName).NotEmpty().NotNull();
        //RuleFor(x => x.MenuFkId).NotEmpty().NotNull();
    }
}
