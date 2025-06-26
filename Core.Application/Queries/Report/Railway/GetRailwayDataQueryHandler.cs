using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Railway
{
    public class GetRailwayDataQueryHandler:IRequest<List<RailwayDTO>>
    {
        public string ReportDate { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string ReportType { get; set; }
        public string CircleCode { get; set; }

        public class Handler:IRequestHandler<GetRailwayDataQueryHandler,List<RailwayDTO>>
        {
            private readonly IMinistryRepository _repository;
            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<RailwayDTO>> Handle(GetRailwayDataQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetRailwayDetails(request.ZoneCode, request.ReportDate, request.LocationCode, request.ReportType, request.CircleCode);
                return result;
            }
        }
    }
}
