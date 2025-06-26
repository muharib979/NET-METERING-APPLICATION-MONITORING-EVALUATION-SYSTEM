using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetZoneWisePouroshovaQueryList : IRequest<List<ZoneWisePouroshovaDataDTO>>
    {
        public string ZoneCode { get; set; }

        public class Handler : IRequestHandler<GetZoneWisePouroshovaQueryList, List<ZoneWisePouroshovaDataDTO>>
        {
            private readonly ICityCorporationRepository _cityCorporationRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository cityCorporationRepository, IMapper mapper)
            {
                _cityCorporationRepository = cityCorporationRepository;
                _mapper = mapper;
            }

            public async Task<List<ZoneWisePouroshovaDataDTO>> Handle(GetZoneWisePouroshovaQueryList request, CancellationToken cancellationToken)
            {
                var result = await _cityCorporationRepository.GetZoneWisePouroshovaData(request.ZoneCode);
                return result;
            }
        }
    }
}
