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
    public class GetZoneWiseCityPouroUnionArrearQueryList: IRequest<Response<List<ZoneWiseCityPouroUnionArrearDto>>>
    {
        public string billMonth { get; set; }
       

        public class Handler : IRequestHandler<GetZoneWiseCityPouroUnionArrearQueryList, Response<List<ZoneWiseCityPouroUnionArrearDto>>>
        {
            private readonly ICityCorporationRepository _cityCorporRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository cityCorporRepository, IMapper mapper)
            {
                _cityCorporRepository = cityCorporRepository;
                _mapper = mapper;   
            }
            public async Task<Response<List<ZoneWiseCityPouroUnionArrearDto>>> Handle(GetZoneWiseCityPouroUnionArrearQueryList request, CancellationToken cancellationToken)
            {
                var result = await _cityCorporRepository.GetZoneWiseCityPouroUnionArrear(request.billMonth);
                return Response<List<ZoneWiseCityPouroUnionArrearDto>>.Success(_mapper.Map<List<ZoneWiseCityPouroUnionArrearDto>>(result),"Success");
            }
        }
    }
}
