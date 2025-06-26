using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveCencusBillCommand : CensusBillDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveCencusBillCommand, bool>
        {
            private readonly ITemporaryBillRepository _repository;

            public Handler(ITemporaryBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveCencusBillCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveCensusBill(request);
                return result;

            }
        }
    }
}
