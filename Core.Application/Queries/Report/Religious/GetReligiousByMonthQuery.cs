using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetReligiousByMonthQuery : IRequest<List<LocationWiseReligiousRptDTO>>
    {
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string BillMonth { get; set; }
        public class Handler : IRequestHandler<GetReligiousByMonthQuery, List<LocationWiseReligiousRptDTO>>
        {
            private readonly IReligiousRepository _religiousRepository;
            public Handler(IReligiousRepository religiousRepository)
            {
                _religiousRepository = religiousRepository;
            }
            public async Task<List<LocationWiseReligiousRptDTO>> Handle(GetReligiousByMonthQuery request, CancellationToken cancellationToken)
            {
                //if (request.ZoneCode == "null")
                //    request.ZoneCode = null;
                //if (request.LocationCode == "null")
                //    request.LocationCode = null;
                var result = await _religiousRepository.GetReligiousByMonth(request.BillMonth, request.ZoneCode, request.LocationCode);
                return result.ToList();
            }
        }
    }
}
