using Core.Application.Interfaces.Location;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetZoneCircleByLocation
{
    public class GetZoneCircleByLocationQuery:IRequest<List<ZoneCircleLocationDTO>>
    {
        public string LocationCode { get; set; }
        class Handler : IRequestHandler<GetZoneCircleByLocationQuery, List<ZoneCircleLocationDTO>>
        {
            private readonly ILocationRepository _locationRepository;
            public Handler(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<List<ZoneCircleLocationDTO>> Handle(GetZoneCircleByLocationQuery request, CancellationToken cancellationToken)
            {
                var result = await _locationRepository.GetZoneCircleByLocation(request.LocationCode);
                return result;
            }
        }
    }
}
