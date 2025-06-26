namespace Core.Application.Commands.Dbo.MenuEdit;

public class MenuEditCommandValidator : AbstractValidator<MenuEditCommand>
{
    public MenuEditCommandValidator()
    {
        RuleFor(x => x.MenuId).NotEmpty().NotNull();
        RuleFor(x => x.MenuName).NotEmpty().NotNull();
    }
}
