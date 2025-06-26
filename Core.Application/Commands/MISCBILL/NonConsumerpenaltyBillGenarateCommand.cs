using Core.Application.Interfaces.MiscBilling;
using MediatR;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class NonConsumerpenaltyBillGenarateCommand:NonConsumerPenaltyBillDTO,IRequest<Returndto>
    {

        public class Handler : IRequestHandler<NonConsumerpenaltyBillGenarateCommand, Returndto>
        {
            private readonly IPenaltyBillGenerateRepository _repository;

            public Handler(IPenaltyBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<Returndto> Handle(NonConsumerpenaltyBillGenarateCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveNonConsumerBill(request);
                return result;
            }
        }

    }
}
