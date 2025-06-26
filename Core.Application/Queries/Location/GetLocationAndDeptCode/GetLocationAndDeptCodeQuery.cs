using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.Religious;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationAndDeptCode
{
    public class GetLocationAndDeptCodeQuery:IRequest<List<LocationsDto>>
    {
        public class Handler : IRequestHandler<GetLocationAndDeptCodeQuery, List<LocationsDto>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationAndDeptCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationAndDeptCode();
                return result;
            }
        }
    }
}
