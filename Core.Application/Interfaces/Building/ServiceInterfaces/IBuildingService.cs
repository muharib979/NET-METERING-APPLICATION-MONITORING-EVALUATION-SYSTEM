using Shared.DTOs.Building;
namespace Core.Application.Interfaces.Building.ServiceInterfaces;

public interface IBuildingService: IBaseService<BuildingDto>
{
    Task<List<DropdownResult>> GetAllBuildingDDAsync();
    Task<bool> IsUniqueSiteNbrAsync(string sitenbr);
}
