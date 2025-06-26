namespace Core.Application.Interfaces.Building.RepositoryInterfaces;
using Core.Domain.Building;
using Shared.DTOs.Building;

public interface IBuildingRepository : IBaseRepository<BuildingDto>
{
    Task<List<DropdownResult>> GetAllBuildingDDAsync();
    Task<bool> IsUniqueSiteNbrAsync(string sitenbr);
}
