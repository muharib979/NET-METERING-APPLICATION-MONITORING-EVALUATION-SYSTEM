using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetProgramByStrategicCodeQueryList
{
    public class GetProgramByStrategicCodeQueryList: IRequest<List<ProgramDTO>>
    {
        public string StrategicCode { get; set; }
        public class Handler : IRequestHandler<GetProgramByStrategicCodeQueryList, List<ProgramDTO>>
        {
            private readonly IProgramRepository _programRepository;
            public Handler(IProgramRepository programRepository)
            {
                _programRepository= programRepository;
            }
            public async Task<List<ProgramDTO>> Handle(GetProgramByStrategicCodeQueryList request, CancellationToken cancellationToken)
            {
                var result = await _programRepository.GetAllProgramDataByStrategicCode(request.StrategicCode);
                return result;
            }
        }
    }
}
