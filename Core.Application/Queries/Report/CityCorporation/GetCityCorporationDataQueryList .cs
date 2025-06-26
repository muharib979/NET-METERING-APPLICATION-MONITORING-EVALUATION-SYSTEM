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
    public class GetCityCorporationDataQueryList : IRequest<Response<IEnumerable<CityCorporationDataDto>>>
    {

        public class Handler : IRequestHandler<GetCityCorporationDataQueryList, Response<IEnumerable<CityCorporationDataDto>>>
        {
            private readonly ICityCorporationRepository _cityCorporationRepository;
            public Handler(ICityCorporationRepository cityCorporationRepository)
            {
                _cityCorporationRepository = cityCorporationRepository;
            }
            public async Task<Response<IEnumerable<CityCorporationDataDto>>>Handle(GetCityCorporationDataQueryList request, CancellationToken cancellationToken)
            {
                var result = await _cityCorporationRepository.GetAllCityCorporation();
                if (result == null) return Response<IEnumerable<CityCorporationDataDto>>.Fail("Data Not Found");
                return Response<IEnumerable<CityCorporationDataDto>>.Success(result,"Successfully Retrived");
            }
        }
    }
}
