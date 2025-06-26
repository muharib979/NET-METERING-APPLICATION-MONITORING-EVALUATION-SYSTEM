using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Building.GetBuildingDDList;

public class GetBuildingDDListQuery : IRequest<Response<List<DropdownResult>>> 
{
    public class Handler : IRequestHandler<GetBuildingDDListQuery, Response<List<DropdownResult>>>
    {
        private readonly IBuildingRepository _repository;

        public Handler (IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<List<DropdownResult>>> Handle(GetBuildingDDListQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllBuildingDDAsync();
            return Response<List<DropdownResult>>.Success(result, "Successfully Retrieved All Building DropDown List");
        }
    }
}
