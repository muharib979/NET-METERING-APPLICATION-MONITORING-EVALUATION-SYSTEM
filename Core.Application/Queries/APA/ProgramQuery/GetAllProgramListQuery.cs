using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetProgramByStrategicCodeQueryList
{
    public class GetAllProgramListQuery: IRequest<List<ProgramDTO>>
    {

        public class Handler : IRequestHandler<GetAllProgramListQuery, List<ProgramDTO>>
        {
            private readonly IProgramRepository _programRepository;
            public Handler(IProgramRepository programRepository)
            {
                _programRepository = programRepository;
            }
            public async Task<List<ProgramDTO>> Handle(GetAllProgramListQuery request, CancellationToken cancellationToken)
            {
                var result = await _programRepository.GetAllProgramList();
                return result;
            }
        }
    }
}
