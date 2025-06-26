using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.VatLps
{
    public class GetSupplementaryVatLpsQuery : IRequest<VatLpsDTO>
    {
        public string customerNumber { get; set; }
        public string locationCode { get; set; }

        public class Handler : IRequestHandler<GetSupplementaryVatLpsQuery, VatLpsDTO>
        {
            private readonly IVatLpsRepository _repository;
            public Handler(IVatLpsRepository repository)
            {
                _repository = repository;
            }

            public async Task<VatLpsDTO> Handle(GetSupplementaryVatLpsQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetSupplementaryVatLps(request.customerNumber, request.locationCode);
                return result;

            }
        }
    }
}
