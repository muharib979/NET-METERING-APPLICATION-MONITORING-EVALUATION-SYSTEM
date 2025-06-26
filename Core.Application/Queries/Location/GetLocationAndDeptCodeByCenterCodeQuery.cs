using Core.Application.Interfaces.Location;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location
{
    public class GetLocationAndDeptCodeByCenterCodeQuery:IRequest <List<LocationsDto>>
    {
        public string CenterCode {  get; set; }
        public class Handler:IRequestHandler<GetLocationAndDeptCodeByCenterCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationAndDeptCodeByCenterCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationAndDeptCodeByCenterCode(request.CenterCode);
                return result;
            }
        }
    }
}
