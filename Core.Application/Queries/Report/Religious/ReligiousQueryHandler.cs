using Core.Application.Interfaces.Religious;
using Microsoft.VisualBasic;
using Nancy;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class ReligiousQueryHandler:IRequest<Response<List<ReligiousDTOs>>>
    {
        public string ReportDate { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }
        public string CircleCode { get; set; }
        public class Handler:IRequestHandler<ReligiousQueryHandler,Response<List<ReligiousDTOs>>>
        {
            private readonly IReligiousRepository _repository;
            public Handler(IReligiousRepository repository)
            {
                _repository = repository;
            }
            public async Task<Response<List<ReligiousDTOs>>>Handle(ReligiousQueryHandler request,CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllReligiousSummaryData(request.ReportDate, request.ZoneCode, request.LocationCode, request.ReportType,request.CircleCode);
                return Response<List<ReligiousDTOs>>.Success(result, "Successfully Retrived");
            }
        }
    }
}
