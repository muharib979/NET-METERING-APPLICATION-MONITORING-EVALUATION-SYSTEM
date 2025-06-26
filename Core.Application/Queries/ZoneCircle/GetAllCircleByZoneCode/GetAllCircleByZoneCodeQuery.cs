using Core.Application.Interfaces.ZoneCircle;
using Core.Domain.ZoneCircle;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetAllCircleByZoneCode
{
    public class GetAllCircleByZoneCodeQuery : IRequest<Response<List<CircleDto>>>
    {
        public string zoneCode { get; set; }
        public class Handler : IRequestHandler<GetAllCircleByZoneCodeQuery, Response<List<CircleDto>>>
        {
            private readonly IZoneCircleRepository _zoneCircleRepository;
            private readonly IMapper _mapper;
            public Handler(IZoneCircleRepository zoneCircleRepository, IMapper mapper) 
            {
                _zoneCircleRepository= zoneCircleRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<CircleDto>>> Handle(GetAllCircleByZoneCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _zoneCircleRepository.GetAllCircleByZoneCode(request.zoneCode);
                var test = _mapper.Map<List<CircleDto>>(result);
                return Response<List<CircleDto>>.Success(_mapper.Map<List<CircleDto>>(result), "Success");
            }
        }
    }
}
