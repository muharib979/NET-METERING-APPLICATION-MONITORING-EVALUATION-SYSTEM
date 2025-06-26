using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllBillCycleQuery:IRequest<List<BillCycleDTO>>
    {

        public class Handler : IRequestHandler<GetAllBillCycleQuery, List<BillCycleDTO>>
        {
            private readonly IBillCycleRepository _repository;

            public Handler(IBillCycleRepository billCycleRepository)
            {
                _repository = billCycleRepository;
            }


            public async Task<List<BillCycleDTO>> Handle(GetAllBillCycleQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllBillCycle();
                return result;
            }
        }
    }
}
