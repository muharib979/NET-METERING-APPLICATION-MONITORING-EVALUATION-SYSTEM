using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Performance_Object
{
    public class SavePerformanceCommand : PerfomanceIndexDto, IRequest<bool>
    {

        public class Handler : IRequestHandler<SavePerformanceCommand, bool>
        {
            private readonly IPerformanceIndexRepository _repository;

            public Handler(IPerformanceIndexRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SavePerformanceCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SavePerformanceBill(request);
                return result;

            }
        }
    }
}
