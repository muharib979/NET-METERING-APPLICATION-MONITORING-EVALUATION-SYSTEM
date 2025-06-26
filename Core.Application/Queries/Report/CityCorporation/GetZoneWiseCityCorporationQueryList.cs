using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetZoneWiseCityCorporationQueryList: IRequest<List<ZoneWiseCitycorporationDataDTO>>
    {
        public string ZoneCode { get; set; }

        public class Handler : IRequestHandler<GetZoneWiseCityCorporationQueryList, List<ZoneWiseCitycorporationDataDTO>>
        {
            private readonly ICityCorporationRepository _citycorporationRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository citycorporationRepository, IMapper mapper)
            {
                _citycorporationRepository = citycorporationRepository;
                _mapper = mapper;
            }

            public async Task<List<ZoneWiseCitycorporationDataDTO>> Handle(GetZoneWiseCityCorporationQueryList request, CancellationToken cancellationToken)
            {
                var result = await _citycorporationRepository.GetZoneWiseCitycorporationData(request.ZoneCode);
                return result;
            }
        }
    }
}
