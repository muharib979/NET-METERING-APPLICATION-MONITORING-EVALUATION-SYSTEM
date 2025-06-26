using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL
{
    public class SaveStrategicObjectCommand: StrategicObjectiveDto, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveStrategicObjectCommand, bool>
        {
            private readonly IStrategicObjectiveRepository _repository;

            public Handler(IStrategicObjectiveRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveStrategicObjectCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveStrategicObjectiveBill(request);
                return result;

            }
        }

    }
}
