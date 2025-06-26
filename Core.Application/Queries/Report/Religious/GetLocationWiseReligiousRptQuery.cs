using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetLocationWiseReligiousRptQuery: IRequest<List<LocationWiseReligiousRptDTO>>
    {
        public string? ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }

        public class Handler : IRequestHandler<GetLocationWiseReligiousRptQuery, List<LocationWiseReligiousRptDTO>>
        {
            private readonly IReligiousRepository _religiousRepository;
            public Handler(IReligiousRepository religiousRepository)
            {
                _religiousRepository = religiousRepository;
            }

            public async Task<List<LocationWiseReligiousRptDTO>> Handle(GetLocationWiseReligiousRptQuery request, CancellationToken cancellationToken)
            {
                if (request.ZoneCode == "null")
                    request.ZoneCode = null;
                if (request.LocationCode == "null")
                    request.LocationCode = null;
                var result = await _religiousRepository.GetLocationWiseRpt(request.ZoneCode, request.LocationCode, request.StartMonth, request.EndMonth);
                return result.ToList();
            }
        }
    }
}
