using Core.Application.Interfaces.UnionPorishod;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.UnionPorishod
{
    public class GetUnionPorishodByDate: IRequest<List<ZoneWiseUnionPorishodDto>>
    {

        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string LocationCode { get; set; }
        public string? BillMonth { get; set; }
        public string ReportType { get; set; }
        
        public class Handler : IRequestHandler<GetUnionPorishodByDate, List<ZoneWiseUnionPorishodDto>>
        {
            private readonly IUnionPorishodRepository _repository;

               public Handler(IUnionPorishodRepository repository)
              {
                    _repository = repository;
               }
            public async Task<List<ZoneWiseUnionPorishodDto>> Handle(GetUnionPorishodByDate request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetUnionPorishodbyDate(request.ZoneCode,request.CircleCode,request.LocationCode,request.BillMonth, request.ReportType);
                return result;
            }
        }

       
    }
}
