using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetAllLocation.GetLocationByUserIdAndCircleCode;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationByUserNameAndCenterCode
{
    public class GetLocationByUserNameAndCenterCodeQuery : IRequest<List<LocationsDto>>
    {
        public string UserName { get; set; }
        public string[] db { get; set; }
        public class Handler : IRequestHandler<GetLocationByUserNameAndCenterCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationByUserNameAndCenterCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationByUserNameCenterCode(request.UserName, request.db);
                return result;
            }
        }
    }
}
