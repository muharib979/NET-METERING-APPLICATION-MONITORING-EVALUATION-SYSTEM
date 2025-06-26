using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetLocationDDQuery : IRequest<Response<List<LocationDDDTO>>>
    {
        public class Handler : IRequestHandler<GetLocationDDQuery, Response<List<LocationDDDTO>>>
        {
            
            private readonly IMscLocationRepository _repository;
            public Handler(IMscLocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<Response<List<LocationDDDTO>>> Handle(GetLocationDDQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationAsync();

                return Response<List<LocationDDDTO>>.Success(result, "Successfully Retrived Locations!");
            }
        }
    }
}
