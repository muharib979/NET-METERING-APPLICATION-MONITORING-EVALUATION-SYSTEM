using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Building;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Building.GetBuildingById;

public class GetBuildingByIdQuery : IRequest<Response<BuildingDto>>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<GetBuildingByIdQuery, Response<BuildingDto>>
    {
        private readonly IBuildingRepository _repository;
        

        public Handler (IBuildingRepository repository, IMapper mapper)
        {
            _repository = repository;
            
        }
        public async Task<Response<BuildingDto>> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _repository.GetByIdAsync(request.Id);

            if (building == null) return Response<BuildingDto>.Fail("Failed to Retrieve Building Info");

            return Response<BuildingDto>.Success(building, "Successfully Retrieved Building Info");
        }
    }
}
