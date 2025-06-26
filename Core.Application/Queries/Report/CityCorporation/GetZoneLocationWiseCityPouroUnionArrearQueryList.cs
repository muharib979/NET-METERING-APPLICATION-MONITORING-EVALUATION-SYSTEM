using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetZoneLocationWiseCityPouroUnionArrearQueryList: IRequest<Response<List<ZoneLocationWiseCityPouroUnionDto>>>
    {
        public string ZoneCode { get; set; }
        public string billMonth { get; set; }
        public class Handler : IRequestHandler<GetZoneLocationWiseCityPouroUnionArrearQueryList, Response<List<ZoneLocationWiseCityPouroUnionDto>>>
        {
            private readonly ICityCorporationRepository _cityRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository cityRepository, IMapper mapper)
            {
                _cityRepository= cityRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<ZoneLocationWiseCityPouroUnionDto>>> Handle(GetZoneLocationWiseCityPouroUnionArrearQueryList request, CancellationToken cancellationToken)
            {
                
                var result = await _cityRepository.GetZoneLocationWiseCityPouroUnionArrear(request.ZoneCode, request.billMonth);
                return Response<List<ZoneLocationWiseCityPouroUnionDto>>.Success(_mapper.Map<List<ZoneLocationWiseCityPouroUnionDto>>(result), "Success");
            }
        }
    }
}
