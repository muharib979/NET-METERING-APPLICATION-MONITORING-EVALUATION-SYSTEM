using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetSupplementaryListQuery:SupplementaryBillViewDTO,IRequest<List<SupplementaryBillViewDTO>>
    {
        public class Handler : IRequestHandler<GetSupplementaryListQuery, List<SupplementaryBillViewDTO>>
        {
            private readonly ISupplementaryGenarateRepository _repository;
            public Handler(ISupplementaryGenarateRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<SupplementaryBillViewDTO>> Handle(GetSupplementaryListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetSupplementaryView(request);
                return result;
            }
        }
    }
}
