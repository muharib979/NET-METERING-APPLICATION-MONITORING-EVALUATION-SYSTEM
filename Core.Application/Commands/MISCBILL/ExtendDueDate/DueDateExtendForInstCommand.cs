using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.ExtendDueDate
{
    public class DueDateExtendForInstCommand : ExtendDueDateDTO, IRequest<int>
    {
        public class Handler : IRequestHandler<DueDateExtendForInstCommand, int>
        {
            private readonly IDueDateExtendRepository _repository;
            public Handler(IDueDateExtendRepository repository)
            {
                _repository = repository;

            }

            public async Task<int> Handle(DueDateExtendForInstCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.ExtendDueDateForInst(request);

                return result;
            }
        }
    }
}
