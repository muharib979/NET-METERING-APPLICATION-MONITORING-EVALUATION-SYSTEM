﻿using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.CalculateBillFor
{
    public class GetNonConsumerPenaltyBillAmount: CalculateBills, IRequest<CalculateBills>
    {
        public class Handler : IRequestHandler<GetNonConsumerPenaltyBillAmount, CalculateBills>
        {
            private readonly ICalculateBillGenerateRepository _repository;

            public Handler(ICalculateBillGenerateRepository repository)
            {
                _repository = repository;
            }
            public async Task<CalculateBills> Handle(GetNonConsumerPenaltyBillAmount request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetCalculateNonConsumerpenaltyBill(request);
                return result;
            }
        }
    }
}
