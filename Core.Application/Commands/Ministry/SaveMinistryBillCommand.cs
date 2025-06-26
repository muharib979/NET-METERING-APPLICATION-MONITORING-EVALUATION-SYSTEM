using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Ministry
{
    public class MinistryBillCommand: ViewMinistryDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<MinistryBillCommand, bool>
        {
            private readonly IMinistryRepository _repository;

            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(MinistryBillCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveMinistryBill(request);
                return result;

            }
        }
    }
}
