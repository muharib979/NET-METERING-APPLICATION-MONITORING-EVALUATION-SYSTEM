namespace Core.Application.Commands.Building.EditBuilding;

public class EditBuildingCommandValidator : AbstractValidator<EditBuildingCommand>
{
    public EditBuildingCommandValidator()
    {
        RuleFor(x => x.BuildingId).NotEmpty();
        RuleFor(x => x.BuildingTitle).NotEmpty();
    }
}
