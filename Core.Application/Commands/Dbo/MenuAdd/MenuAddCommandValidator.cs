namespace Core.Application.Commands.Dbo.MenuAdd;

public class MenuAddCommandValidator : AbstractValidator<MenuAddCommand>
{
    public MenuAddCommandValidator()
    {
        RuleFor(x => x.MenuName).NotEmpty().NotNull();
    }
}
