using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SupplementaryBillGenarateCommand : SupplementaryBillDTO, IRequest<ReturnDto>
    {
        public class Handler : IRequestHandler<SupplementaryBillGenarateCommand, ReturnDto>
        {
            private readonly ISupplementaryGenarateRepository _repository;
            public Handler(ISupplementaryGenarateRepository repository)
            {
                _repository = repository;
            }
            public async Task<ReturnDto> Handle(SupplementaryBillGenarateCommand request, CancellationToken cancellationToken)
            {
                var res = await _repository.SaveSupplenmentaryBill(request);
                return res;
            }
        }
    }
}
