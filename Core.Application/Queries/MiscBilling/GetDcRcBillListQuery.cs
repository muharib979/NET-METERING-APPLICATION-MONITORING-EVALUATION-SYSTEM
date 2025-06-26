using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetDcRcBillListQuery : DcRcBillViewDTO, IRequest<List<DcRcBillViewDTO>>
    {
        public class Handler : IRequestHandler<GetDcRcBillListQuery, List<DcRcBillViewDTO>>
        {
            private readonly IDcRcBillGenerateRepository _repository;

            public Handler(IDcRcBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<DcRcBillViewDTO>> Handle(GetDcRcBillListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetDcRcView(request);
                return result;

            }
        }
    }
}
