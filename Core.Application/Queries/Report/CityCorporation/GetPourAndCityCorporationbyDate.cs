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
    public class GetPourAndCityCorporationbyDate:IRequest<Response<IEnumerable<CityCorporationDto>>>
    {
        public string billMonth { get; set; }
        public string reportType { get; set; }
        public string zoneCode { get; set; }
        public string? locationCode { get; set; }
        public string? circleCode { get; set; }

        public class Handler : IRequestHandler<GetPourAndCityCorporationbyDate, Response<IEnumerable<CityCorporationDto>>>
        {
            private readonly ICityCorporationRepository _cityCorporationRepository;
            public Handler(ICityCorporationRepository cityCorporationRepository)
            {
                _cityCorporationRepository = cityCorporationRepository;
            }
            public async Task<Response<IEnumerable<CityCorporationDto>>>Handle(GetPourAndCityCorporationbyDate request, CancellationToken cancellationToken)
            {
                var result = await _cityCorporationRepository.GetPourAndCityCorporbyDate(request.billMonth, request.reportType, request.zoneCode, request.locationCode,request.circleCode);
                if (result == null) return Response<IEnumerable<CityCorporationDto>>.Fail("Data Not Found");
                return Response<IEnumerable<CityCorporationDto>>.Success(result,"Successfully Retrived");
            }
        }
    }
}
