using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class PenaltyBillSrPrintQuery : IRequest<List<PenaltyBillPrintDTO>>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public class Handler : IRequestHandler<PenaltyBillSrPrintQuery, List<PenaltyBillPrintDTO>>
        {
            private readonly IBillPrintRepository<PenaltyBillPrintDTO> _repository;

            public Handler(IBillPrintRepository<PenaltyBillPrintDTO> repository)
            {
                _repository = repository;
            }

            public async Task<List<PenaltyBillPrintDTO>> Handle(PenaltyBillSrPrintQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.PenaltyBillSrPrint(request.customerNumber, request.billNumber);

                return result;

            }
        }
    }
}
