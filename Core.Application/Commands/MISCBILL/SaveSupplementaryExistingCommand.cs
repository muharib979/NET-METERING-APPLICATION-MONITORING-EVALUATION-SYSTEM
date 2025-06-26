using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveSupplementaryExistingCommand : SupplementaryExistingConsumerDTO, IRequest<bool>
    {

        public class Handler : IRequestHandler<SaveSupplementaryExistingCommand, bool>
        {
            private readonly ISupplementaryGenarateRepository _supplementaryBillGenerateRepository;
            public Handler(ISupplementaryGenarateRepository supplementaryBillGenerateRepository)
            {
                _supplementaryBillGenerateRepository = supplementaryBillGenerateRepository;
            }

            public async Task<bool> Handle(SaveSupplementaryExistingCommand request, CancellationToken cancellationToken)
            {
                var result = await _supplementaryBillGenerateRepository.SaveSupplementaryExistingConsumer(request);
                return result;
            }
        }
    }
}
