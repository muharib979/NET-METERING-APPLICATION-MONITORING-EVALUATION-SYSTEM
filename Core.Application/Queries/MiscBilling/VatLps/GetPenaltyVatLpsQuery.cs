using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.TemporaryBill;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.VatLps
{
    public class GetPenaltyVatLpsQuery : IRequest<VatLpsDTO>
    {
        public string customerNumber { get; set; }
        public string locationCode { get; set; }
        
        public class Handler : IRequestHandler<GetPenaltyVatLpsQuery, VatLpsDTO>
        {
            private readonly IVatLpsRepository _repository;
            public Handler(IVatLpsRepository repository)
            {
                _repository = repository;
            }

            public async Task<VatLpsDTO> Handle(GetPenaltyVatLpsQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetPenaltyVatLps(request.customerNumber, request.locationCode);
                return result;

            }
        }
    }
}
