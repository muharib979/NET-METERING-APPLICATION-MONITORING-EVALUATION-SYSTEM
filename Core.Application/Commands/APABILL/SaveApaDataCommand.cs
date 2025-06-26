using Core.Application.Commands.MISCBILL;
using Core.Application.Interfaces.APA;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL
{
    public class SaveApaDataCommand : List<ApaDTO>, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveApaDataCommand, bool>
        {
            private readonly IApaRepository _repository;

            public Handler(IApaRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveApaDataCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveApaData(request);
                return result;

            }
        }
    }
}
