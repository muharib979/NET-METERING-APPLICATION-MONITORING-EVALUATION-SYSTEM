using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetAllLocation;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location
{
    public class GetDivisionAndDistrictByLocationQuery : IRequest<LocationsDto>
    {
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetDivisionAndDistrictByLocationQuery, LocationsDto>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<LocationsDto> Handle(GetDivisionAndDistrictByLocationQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetDivisionAndDistrictByLocation(request.locationCode);
            }
        }
    }
}

