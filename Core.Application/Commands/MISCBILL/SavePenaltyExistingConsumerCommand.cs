using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SavePenaltyExistingConsumerCommand: PenaltyExistingBillDto,IRequest<bool>
    {
        public class Handler : IRequestHandler<SavePenaltyExistingConsumerCommand, bool>
        {
            private readonly IPenaltyBillGenerateRepository _penaltyBillGenerateRepository;
            public Handler(IPenaltyBillGenerateRepository penaltyBillGenerateRepository)
            {
                _penaltyBillGenerateRepository = penaltyBillGenerateRepository;
            }

            public async Task<bool> Handle(SavePenaltyExistingConsumerCommand request, CancellationToken cancellationToken)
            {
                var result = await _penaltyBillGenerateRepository.SavePenaltyExistingConsumer(request);
                return result;
            }
        }
    }
}
