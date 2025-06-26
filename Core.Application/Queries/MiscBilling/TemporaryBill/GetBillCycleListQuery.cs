using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetBillCycleListQuery : IRequest<List<BillCycleListDTO>>
    {
        public class Handler : IRequestHandler<GetBillCycleListQuery, List<BillCycleListDTO>>
        {
            private readonly IBillCycleRepository _repository;

            public Handler(IBillCycleRepository repository)
            {
                _repository = repository;
            }

           

            public async Task<List<BillCycleListDTO>> Handle(GetBillCycleListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetBillCycleList();
                return result;
            }
        }
    }
}
