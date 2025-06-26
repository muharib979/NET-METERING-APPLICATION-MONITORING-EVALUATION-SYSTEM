using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class PenaltyBillNonCustPrintQuery : IRequest<List<PenaltyBillPrintDTO>>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public class Handler : IRequestHandler<PenaltyBillNonCustPrintQuery, List<PenaltyBillPrintDTO>>
        {
            private readonly IBillPrintRepository<PenaltyBillPrintDTO> _repository;

            public Handler(IBillPrintRepository<PenaltyBillPrintDTO> repository)
            {
                _repository = repository;
            }

            public async Task<List<PenaltyBillPrintDTO>> Handle(PenaltyBillNonCustPrintQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.PenaltyBillNonCustPrint(request.customerNumber, request.billNumber);

                return result;

            }
        }
    }
}
