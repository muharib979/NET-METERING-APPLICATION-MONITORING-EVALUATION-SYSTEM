using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveMiscBillChargeCommand: MiscChargeDTO,IRequest<ReturnDTO>
    {


        public class Handler : IRequestHandler<SaveMiscBillChargeCommand, ReturnDTO>
        {
            private readonly IMiscChargeRepository _repository;
            public Handler(IMiscChargeRepository repository)
            {
                _repository = repository;

            }

            public async Task<ReturnDTO> Handle(SaveMiscBillChargeCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveMiscChargeBill(request);

                return result;
            }
        }
    }
}
