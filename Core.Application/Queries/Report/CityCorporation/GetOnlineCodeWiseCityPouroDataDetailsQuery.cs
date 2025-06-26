using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetOnlineCodeWiseCityPouroDataDetailsQuery : IRequest<OnlineCityPourDataListModel>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string CityCorporationCode { get; set; }
        public string PouroshovaCode { get; set; }

        public class Handler : IRequestHandler<GetOnlineCodeWiseCityPouroDataDetailsQuery, OnlineCityPourDataListModel>
        {
            private readonly ICityCorporationRepository _cityCorporationRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository cityCorporationRepository, IMapper mapper)
            {
                _cityCorporationRepository = cityCorporationRepository;
                _mapper = mapper;
            }

            public async Task<OnlineCityPourDataListModel> Handle(GetOnlineCodeWiseCityPouroDataDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = _mapper.Map<List<OnlineCityPourMergeDTO>>(await _cityCorporationRepository.GetOnlinePouroAndCityCodeWiseBn(request.BillMonth, request.ZoneCode, request.LocationCode, request.CityCorporationCode, request.PouroshovaCode));
                return new OnlineCityPourDataListModel { CityCoporationDataList = null, PouroshovaDataList = null, CityPouroshovaDetailsDataList = result };
            }
        }
    }
}
