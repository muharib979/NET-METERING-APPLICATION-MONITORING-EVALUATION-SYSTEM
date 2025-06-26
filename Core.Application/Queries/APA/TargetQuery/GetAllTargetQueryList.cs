using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.TargetQuery
{
    public class GetAllTargetQueryList: IRequest<List<TargetDTO>>
    {
        public class Handler : IRequestHandler<GetAllTargetQueryList, List<TargetDTO>>
        {
            private readonly ITargetRepository _repository;
            public Handler(ITargetRepository programRepository)
            {
                _repository = programRepository;
            }
            public async Task<List<TargetDTO>> Handle(GetAllTargetQueryList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetTargetList();
                return result;
            }
        }

    }
}
