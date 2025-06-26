using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Religious
{
    public class ReligiousReceiptCommand: ReligiousReceiptViewModel, IRequest<bool>
    {
         public class Handler : IRequestHandler<ReligiousReceiptCommand, bool>
        {
            private readonly IReligiousRepository _repository;

            public Handler(IReligiousRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(ReligiousReceiptCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveReligiousReceiptBill(request);
                return result;

            }
        }
    }
}
