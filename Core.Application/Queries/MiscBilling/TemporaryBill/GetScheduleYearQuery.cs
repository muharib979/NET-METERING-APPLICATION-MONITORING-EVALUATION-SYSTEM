using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetScheduleYearQuery : IRequest<List<ScheduleYearDTO>>
    {
        public class Handler : IRequestHandler<GetScheduleYearQuery, List<ScheduleYearDTO>>
        {
           
            private readonly IScheduleRepository _repository;
          
            public Handler(IScheduleRepository repository)
            {
                _repository= repository;
            }

            public async Task<List<ScheduleYearDTO>> Handle(GetScheduleYearQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetScheduleYear();
                return result;

            }
        }
    }
}
