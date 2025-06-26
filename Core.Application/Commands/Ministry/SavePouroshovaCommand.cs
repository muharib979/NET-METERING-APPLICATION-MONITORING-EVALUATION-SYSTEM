using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Ministry
{
    public class SavePouroshovaCommand: PouroshovaDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SavePouroshovaCommand, bool>
        {
            private readonly IMinistryRepository _repository;

            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SavePouroshovaCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SavePouroshovaBill(request);
                return result;

            }
        }
    }
}
