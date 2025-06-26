using Core.Application.Interfaces.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL
{
    public class DeleteStrategicObjectiveommand:IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeleteStrategicObjectiveommand, int>
        {
            private readonly IStrategicObjectiveRepository _repository;

            public Handler(IStrategicObjectiveRepository repository)
            {
                _repository = repository;
            }

           

            public async Task<int> Handle(DeleteStrategicObjectiveommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteStrategicObjectiveBill(request.id);
                return result;
            }
        }
    }
}
