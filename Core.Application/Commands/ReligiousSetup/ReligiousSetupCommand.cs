using Core.Application.Interfaces.ReligiousSetup;
using Shared.DTOs.ReligiousSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ReligiousSetup
{
    public class ReligiousSetupCommand: ReligiousSetupDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<ReligiousSetupCommand, bool>
        {
            private readonly IReligiousSetupRepository _religiousSetupRepository;
            public Handler(IReligiousSetupRepository religiousSetupRepository)
            {
                _religiousSetupRepository = religiousSetupRepository;
            }

            public async Task<bool> Handle(ReligiousSetupCommand request, CancellationToken cancellationToken)
            {
                var result = await _religiousSetupRepository.SetupReligious(request);
                return result;
            }
        }
    }
}
