using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetTemporaryBillViewQuery: TemporaryBillViewDTO,IRequest<List<TemporaryBillViewDTO>>
    {
        public class Handler : IRequestHandler<GetTemporaryBillViewQuery, List<TemporaryBillViewDTO>>
        {
            private readonly ITemporaryBillRepository _repository;

            public Handler(ITemporaryBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<TemporaryBillViewDTO>> Handle(GetTemporaryBillViewQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetTemporaryBillView(request);
                return result;
            }
        }
    }
}
