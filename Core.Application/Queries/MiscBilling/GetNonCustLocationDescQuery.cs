using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Location;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetNonCustLocationDescQuery:IRequest<List<LocationDTO>>
    {
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetNonCustLocationDescQuery, List<LocationDTO>>
{
            private readonly ILocationRepository _repository;

            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationDTO>> Handle(GetNonCustLocationDescQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetNonCustLocationDesc(request.locationCode);
                return result;
            }
        }
    }
}
