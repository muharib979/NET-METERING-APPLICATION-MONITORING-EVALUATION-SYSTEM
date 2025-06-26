using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.ZoneCircle;
using Core.Application.Queries.ZoneCircle.GetZoneByUserIdZoneCode;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation.GetLocationByUserIdAndCircleCode
{
    public class GetLocationByUserIdAndCircleCodeQuery : IRequest<List<LocationsDto>>
    {
        public string UserName { get; set; }
        public string CircleCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByUserIdAndCircleCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationByUserIdAndCircleCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationByUserNameCircleCode(request.UserName, request.CircleCode);
                return result;
            }
        }
    }
}
