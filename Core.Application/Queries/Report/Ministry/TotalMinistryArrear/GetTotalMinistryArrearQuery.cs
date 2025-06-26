using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.TotalMinistryArrear
{
    public class GetTotalMinistryArrearQuery : IRequest<List<LocationWiseArrearDTO>>
    {
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string? LocationCode { get; set; }
        public string BillMonth { get; set; }

        public class Handler : IRequestHandler<GetTotalMinistryArrearQuery, List<LocationWiseArrearDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<LocationWiseArrearDTO>> Handle(GetTotalMinistryArrearQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetTotalMinistryArrear(request.ZoneCode, request.CircleCode, request.LocationCode, request.BillMonth);
                return result;
            }
        }

    }
}
