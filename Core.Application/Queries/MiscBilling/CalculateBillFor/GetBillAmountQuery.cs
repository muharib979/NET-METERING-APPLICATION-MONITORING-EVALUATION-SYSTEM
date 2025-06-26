using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.CalculateBillFor
{
    public class GetBillAmountQuery : CalculateBills, IRequest<CalculateBills>
    {
        public class Handler : IRequestHandler<GetBillAmountQuery, CalculateBills>
        {
            private readonly ICalculateBillGenerateRepository _repository;

            public Handler(ICalculateBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<CalculateBills> Handle(GetBillAmountQuery request, CancellationToken cancellationToken)
            {
                var result  = await _repository.GetCalculatePenaltyBill(request);
                return result;


            }
            
        }
    }
}