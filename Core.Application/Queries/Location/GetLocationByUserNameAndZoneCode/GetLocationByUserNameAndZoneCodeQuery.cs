using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetAllLocation.GetLocationByUserIdAndCircleCode;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationByUserNameAndZoneCode
{
    public class GetLocationByUserNameAndZoneCodeQuery : IRequest<List<LocationsDto>>
    {
        public string UserName { get; set; }
        public string ZoneCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByUserNameAndZoneCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationByUserNameAndZoneCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationByUserNameZoneCode(request.UserName, request.ZoneCode);
                return result;
            }
        }
    }
}
