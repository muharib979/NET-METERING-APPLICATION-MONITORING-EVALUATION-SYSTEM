using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllLocationWiseMinistryArrearQuery
{
    public class GetAllLocationWiseMinistryArrearQuery: IRequest<List<LocationWiseArrearDTO>>
    {
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string? LocationCode { get; set; }
        public string BillMonth { get; set; }

        public class Handler : IRequestHandler<GetAllLocationWiseMinistryArrearQuery, List<LocationWiseArrearDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<LocationWiseArrearDTO>> Handle(GetAllLocationWiseMinistryArrearQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetLocationWiseMinistryArrear(request.ZoneCode, request.CircleCode, request.LocationCode, request.BillMonth);
                return result;
            }
        }
    }
}
