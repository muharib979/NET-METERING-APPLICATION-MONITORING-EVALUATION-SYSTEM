using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetOnlineCityCorporAndPouroBnDetailsQuery : IRequest<OnlineCityPourDataListModel>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
    }
    public class Handler : IRequestHandler<GetOnlineCityCorporAndPouroBnDetailsQuery, OnlineCityPourDataListModel>
    {
        private readonly ICityCorporationRepository _cityCorporationRepository;
        private readonly IMapper _mapper;
        public Handler(ICityCorporationRepository cityCorporationRepository, IMapper mapper)
        {
            _cityCorporationRepository = cityCorporationRepository;
            _mapper = mapper;
        }

        public async Task<OnlineCityPourDataListModel> Handle(GetOnlineCityCorporAndPouroBnDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<OnlineCityPourMergeDTO>>(await _cityCorporationRepository.GetAllOnlinePouroAndCityCorporBn(request.BillMonth, request.ZoneCode, request.LocationCode));

            return new OnlineCityPourDataListModel { CityCoporationDataList = null, PouroshovaDataList = null, CityPouroshovaDetailsDataList = result };
        }
    }
}
