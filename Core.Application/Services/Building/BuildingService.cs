//using Core.Application.Interfaces.Building.RepositoryInterfaces;
//using Core.Application.Interfaces.Building.ServiceInterfaces;
//using Shared.DTOs.Building;

//namespace Core.Application.Services.Building;

//public class BuildingService : IBuildingService
//{
//    private readonly IBuildingRepository _repository;
//    private readonly IMapper _mapper;
//    public BuildingService(IBuildingRepository repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//    }

//    public async Task<int> AddAsync(BuildingDto entity) => await _repository.AddAsync(_mapper.Map<Core.Domain.Building.Building>(entity));

//    public Task<int> AddListAsync(List<BuildingDto> entity)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<int> DeleteAsync(int id)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<List<BuildingDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<BuildingDto>>(await _repository.GetAllAsync(pParams));

//    public async Task<List<DropdownResult>> GetAllBuildingDDAsync() => await _repository.GetAllBuildingDDAsync();

//    public async Task<BuildingDto> GetByIdAsync(int id) => _mapper.Map<BuildingDto>(await _repository.GetByIdAsync(id));

//    public async Task<int> GetTotalCountAsync(string searchBy) => await _repository.GetTotalCountAsync(searchBy);

//    public async Task<bool> IsUniqueSiteNbrAsync(string sitenbr) => await _repository.IsUniqueSiteNbrAsync(sitenbr);

//    public async Task<int> UpdateAsync(BuildingDto entity) => await _repository.UpdateAsync(_mapper.Map<Core.Domain.Building.Building>(entity));
//}