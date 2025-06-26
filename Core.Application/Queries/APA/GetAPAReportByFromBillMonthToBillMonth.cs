using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA
{
    public class GetAPAReportByFromBillMonthToBillMonth:IRequest<List<object>>
    {
        public string fromBillMonth { get;set; }
        public string toBillMonth { get;set; }

        public class Handler:IRequestHandler<GetAPAReportByFromBillMonthToBillMonth, List<object>>
        {
            private readonly IApaRepository _repository;
            public Handler(IApaRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<object>> Handle(GetAPAReportByFromBillMonthToBillMonth request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAPAReportByBillMonth(request.fromBillMonth, request.toBillMonth);
                return result;
            }
        }
    }
}
