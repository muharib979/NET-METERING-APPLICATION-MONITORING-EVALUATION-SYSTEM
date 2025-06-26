using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetScheduleBillGroupQuery : IRequest<List<CenBillGroupDTO>>
    {
        public class Handler : IRequestHandler<GetScheduleBillGroupQuery, List<CenBillGroupDTO>>
        {
           
            private readonly IScheduleRepository _repository;
            public Handler(IScheduleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<CenBillGroupDTO>> Handle(GetScheduleBillGroupQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetScheduleBillGroup();
                return result;

            }
        }
    }
}
