using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Program_object
{
    public class SaveProgramCommand : ProgramDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveProgramCommand, bool>
        {
            private readonly IProgramRepository _repository;

            public Handler(IProgramRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveProgramCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveProgramBill(request);
                return result;

            }
        }
    }
}
