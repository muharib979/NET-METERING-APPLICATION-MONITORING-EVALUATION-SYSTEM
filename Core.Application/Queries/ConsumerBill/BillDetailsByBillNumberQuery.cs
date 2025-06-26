using Core.Application.Interfaces.ConsumerBill;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.ConsumerBill;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ConsumerBill
{
    public class BillDetailsByBillNumberQuery : IRequest<List<ConsumerBillDTO>>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public string? user { get; set; }


        public class Handler : IRequestHandler<BillDetailsByBillNumberQuery, List<ConsumerBillDTO>>
        {
            private readonly IConsumerBillRepository _repository;

            public Handler(IConsumerBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ConsumerBillDTO>> Handle(BillDetailsByBillNumberQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.BillDetailsByBillNumber(request.customerNumber, request.billNumber, request.user);

                return result;

            }
        }
    }
}
