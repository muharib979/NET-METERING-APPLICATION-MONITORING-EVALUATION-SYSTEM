using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.OffiecStuff;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetScheduleMonthQuery : IRequest<List<ScheduleMonthDTO>>
    {
        public string Year { get; set; }
        public class Handler : IRequestHandler<GetScheduleMonthQuery, List<ScheduleMonthDTO>>
        {
           
            private readonly IScheduleRepository _repository;
            public Handler(IScheduleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ScheduleMonthDTO>> Handle(GetScheduleMonthQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetScheduleMonth(request.Year);
                
                return result;
            }
        }
    }
}
