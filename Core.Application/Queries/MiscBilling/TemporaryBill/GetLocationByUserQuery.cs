using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetLocationByUserQuery : IRequest<List<LocationsDTO>>
    {
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByUserQuery, List<LocationsDTO>>
        {
            
            private readonly IMscLocationRepository _repository;
            public Handler(IMscLocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDTO>> Handle(GetLocationByUserQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetLocationByUser(request.locationCode);
                return result;

            }
        }
    }
}