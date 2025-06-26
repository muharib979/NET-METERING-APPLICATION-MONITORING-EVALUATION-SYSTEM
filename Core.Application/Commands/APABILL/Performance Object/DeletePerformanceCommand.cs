using Core.Application.Interfaces.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Performance_Object
{
    public class DeletePerformanceCommand: IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeletePerformanceCommand, int>
        {
            private readonly IPerformanceIndexRepository _repository;

            public Handler(IPerformanceIndexRepository repository)
            {
                _repository = repository;
            }



            public async Task<int> Handle(DeletePerformanceCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeletePerformanceBill(request.id);
                return result;
            }
        }
    }
}
