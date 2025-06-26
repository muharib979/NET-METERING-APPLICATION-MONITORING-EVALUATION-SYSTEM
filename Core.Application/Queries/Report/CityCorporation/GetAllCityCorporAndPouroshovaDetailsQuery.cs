using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetAllCityCorporAndPouroshovaDetailsQuery: IRequest<List<CityCorporationDetailsDTO>>
    {
        public string billMonth { get; set; }
        public string reportType { get; set; }
        public string zoneCode { get; set; }
        public string locationCode { get; set; }
        public string circleCode { get; set; }
        public class Handler : IRequestHandler<GetAllCityCorporAndPouroshovaDetailsQuery, List<CityCorporationDetailsDTO>>
        {
            private readonly ICityCorporationRepository _cityCorporRepository;
            public Handler(ICityCorporationRepository cityCorporRepository)
            {
                _cityCorporRepository = cityCorporRepository;
            }

            public async Task<List<CityCorporationDetailsDTO>> Handle(GetAllCityCorporAndPouroshovaDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _cityCorporRepository.GetAllCityCorporAndPouroshovaArrearDetails(request.billMonth, request.reportType, request.zoneCode, request.locationCode,request.circleCode);
                return result;
            }
        }
    }
}
