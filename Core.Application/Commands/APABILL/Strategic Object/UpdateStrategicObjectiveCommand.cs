using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL
{
    public class UpdateStrategicObjectiveCommand: StrategicObjectiveDto, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateStrategicObjectiveCommand, bool>
        {
            private readonly IStrategicObjectiveRepository _repository;

            public Handler(IStrategicObjectiveRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(UpdateStrategicObjectiveCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.UpdateStrategicObjectiveBill(request);
                return result;

            }
        }
    }
}
