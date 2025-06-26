using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Microsoft.AspNetCore.Components.Forms;
using Shared.DTOs.Building;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Building.GetBuildingList;

public class GetBuildingListQuery : PaginationParams, IRequest<Response<PaginatedList<BuildingDto>>>
{
    public class Handler : IRequestHandler<GetBuildingListQuery, Response<PaginatedList<BuildingDto>>>
    {
        private readonly IBuildingRepository _repository;
        private readonly List<string> _validationMessage;
        public Handler(IBuildingRepository repository) 
        {
            _repository= repository;
            _validationMessage = new List<string>();
        }
        public async Task<Response<PaginatedList<BuildingDto>>> Handle(GetBuildingListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var buildings = await _repository.GetAllAsync(request);
                if (buildings == null)
                {
                    _validationMessage.Add("No Building Information Found");
                    return Response<PaginatedList<BuildingDto>>.Fail("No Building Information Found", _validationMessage);
                }
                var totalBuildingsCount = buildings.Select(x => x.TotalRowCount).FirstOrDefault();
                var paginatedList = PaginatedList<BuildingDto>.Create(buildings, request.PageNumber, request.pageSize, totalBuildingsCount);

                return Response<PaginatedList<BuildingDto>>.Success(paginatedList, "Successfully Retrieved All Building Infos");
            }
            catch (Exception e)
            {
                _validationMessage.Add(e.Message);
                return Response<PaginatedList<BuildingDto>>.Fail("Failed to Retrieve Building Infos", _validationMessage);
            }
        }
    }
}
