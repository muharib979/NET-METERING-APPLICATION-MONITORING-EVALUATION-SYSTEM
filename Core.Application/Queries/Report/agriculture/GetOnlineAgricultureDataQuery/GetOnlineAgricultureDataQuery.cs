using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using Shared.DTOs.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.agriculture.GetOnlineAgricultureDataQuery
{
    public class GetOnlineAgricultureDataQuery : IRequest<List<OnlineAgricultureMergeDTO>>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetOnlineAgricultureDataQuery, List<OnlineAgricultureMergeDTO>>
        {
            private readonly IAgricultureRepository _agricultureRepo;
            private readonly IMapper _mapper;
            public Handler(IAgricultureRepository agricultureRepo, IMapper mapper)
            {
                _agricultureRepo = agricultureRepo;
                _mapper = mapper;
            }

            public async Task<List<OnlineAgricultureMergeDTO>> Handle(GetOnlineAgricultureDataQuery request, CancellationToken cancellationToken)
            {
                var result = await _agricultureRepo.GetOnlineAgriPoultryAndMinistryData(request.BillMonth, request.ZoneCode, request.LocationCode, request.ReportType);
                return _mapper.Map<List<OnlineAgricultureMergeDTO>>(result); 
                
            }
        }
    }
}
