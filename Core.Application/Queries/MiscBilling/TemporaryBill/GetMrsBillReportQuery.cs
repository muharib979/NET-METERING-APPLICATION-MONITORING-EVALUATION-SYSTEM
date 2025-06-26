using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetMrsBillReportQuery : IRequest<MRSBillPrintDTO>
    {
        public string locationCode { get; set; }
        public string customerNumber { get; set; }
        public string billNumber { get; set; }
        
        public class Handler : IRequestHandler<GetMrsBillReportQuery, MRSBillPrintDTO>
        {

            private readonly IMrsGenarateRepository _repository;

            public Handler(IMrsGenarateRepository repository)
            {
                _repository = repository;
            }

            public async Task<MRSBillPrintDTO> Handle(GetMrsBillReportQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMrsBillReport(request.locationCode, request.customerNumber, request.billNumber);
                return result;
            }
        }
    }
}
