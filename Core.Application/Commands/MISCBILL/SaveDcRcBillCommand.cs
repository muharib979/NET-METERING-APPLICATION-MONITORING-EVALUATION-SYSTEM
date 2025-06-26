using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveDcRcBillCommand : DcRcBillDTO, IRequest<ReturnDTO>
    {
        public class Handler : IRequestHandler<SaveDcRcBillCommand, ReturnDTO>
        {
            private readonly IDcRcBillGenerateRepository _repository;
            public Handler(IDcRcBillGenerateRepository repository)
            {
                _repository = repository;

            }

            public async Task<ReturnDTO> Handle(SaveDcRcBillCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveDcRcBill(request);

                return result;
            }
        }
    }
}
