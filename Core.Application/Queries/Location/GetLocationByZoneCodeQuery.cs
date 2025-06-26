using Core.Application.Interfaces.Location;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;

namespace Core.Application.Queries.Location
{
    public class GetLocationByZoneCodeQuery : IRequest<Response<List<LocationsDto>>>
    {
        public string ZoneCode { get; set; }
        public class Handler : IRequestHandler<GetLocationByZoneCodeQuery, Response<List<LocationsDto>>>
        {
            private readonly ILocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILocationRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<LocationsDto>>> Handle(GetLocationByZoneCodeQuery request, CancellationToken cancellationToken)
            {
                try
                {

                    var result = await _repository.GetLocationByZoneCode(request.ZoneCode);
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
