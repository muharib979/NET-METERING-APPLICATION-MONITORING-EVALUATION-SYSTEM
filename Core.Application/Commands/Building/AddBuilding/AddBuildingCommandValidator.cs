namespace Core.Application.Commands.Building.AddBuilding;

public class AddBuildingCommandValidator: AbstractValidator<AddBuildingCommand>
{
    public AddBuildingCommandValidator()
    {
        RuleFor(x => x.BuildingTitle).NotEmpty();
    }
}
