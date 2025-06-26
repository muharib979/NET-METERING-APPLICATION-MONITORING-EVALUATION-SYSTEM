using Core.Application.Interfaces.MiscBilling;
using MediatR;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetModBillQuery: IRequest<List<ModBillDTO>>
    {
        public string locationCode { get; set; }
        public string billMonth { get; set; }

        public class Handler : IRequestHandler<GetModBillQuery, List<ModBillDTO>>
        {
            private readonly IModBillRepository _repository;
            public Handler(IModBillRepository repository)
            {
                _repository= repository;
            }
            public async Task<List<ModBillDTO>> Handle(GetModBillQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.getModBillByLocCodeAndBillMonth(request.locationCode, request.billMonth);
                return result;
            }
        }
    }
}
