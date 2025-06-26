using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetLocationByDbCode;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location
{
    public class GetLocationByDbArrayQuery : IRequest<Response<List<LocationsDto>>>
    {
        public string[] DbCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByDbArrayQuery, Response<List<LocationsDto>>>
        {
            private readonly ILocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILocationRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<LocationsDto>>> Handle(GetLocationByDbArrayQuery request, CancellationToken cancellationToken)
            {
                try
                {

                    var result = await _repository.GetLocationByDbArray(request.DbCode);
                    return Response<List<LocationsDto>>.Success(_mapper.Map<List<LocationsDto>>(result), "Successfully Retrived");

                }
                catch (Exception ex)
                {
                    return Response<List<LocationsDto>>.Fail("Error");
                }
            }
        }

    }
}
