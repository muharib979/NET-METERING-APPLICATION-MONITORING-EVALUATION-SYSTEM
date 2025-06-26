using Core.Application.Interfaces.Location;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation
{
    public class GetAllLocationQuery : PaginationParams, IRequest<Response<PaginatedList<LocationsDto>>>
    {
        public class Handler : IRequestHandler<GetAllLocationQuery, Response<PaginatedList<LocationsDto>>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository) 
            {
                _repository= repository;
            }
            public async Task<Response<PaginatedList<LocationsDto>>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var location = await _repository.GetAllAsync(request);
                    if (location == null)
                    {

                        return Response<PaginatedList<LocationsDto>>.Fail("No Database Information Found");
                    }

                    var paginatedList = PaginatedList<LocationsDto>.Create(location, request.PageNumber, request.pageSize, 10);

                    return Response<PaginatedList<LocationsDto>>.Success(paginatedList, "Successfully Retrieved All Database Infos");
                }
                catch (Exception e)
                {

                    return Response<PaginatedList<LocationsDto>>.Fail("Failed to Retrieve Database Infos");
                }
            }
        }
    }
}
