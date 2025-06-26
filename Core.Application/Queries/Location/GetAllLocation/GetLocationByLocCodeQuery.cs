using Core.Application.Interfaces.Location;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation
{
    public class GetLocationByLocCodeQuery : IRequest<List<LocationsDto>>
    {
        public string[] LocationCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByLocCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<LocationsDto>> Handle(GetLocationByLocCodeQuery request, CancellationToken cancellationToken)
            {
                string locCode =  "'" + string.Join("', '", request.LocationCode) + "'";
                return await _repository.GetLocationByLocCode(locCode.TrimEnd(','));
            }
        }
    }
}
