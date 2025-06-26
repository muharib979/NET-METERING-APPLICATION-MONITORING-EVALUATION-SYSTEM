using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetStrategicObjectiveList
{
    public class StrategicObjectiveQueryList: IRequest<List<StrategicObjectiveDto>>
    {
        public class Handler : IRequestHandler<StrategicObjectiveQueryList, List<StrategicObjectiveDto>>
        {
            private IStrategicObjectiveRepository _strategicObject;
            public Handler(IStrategicObjectiveRepository strategicObject)
            {
                _strategicObject = strategicObject;
            }

            public async Task<List<StrategicObjectiveDto>> Handle(StrategicObjectiveQueryList request, CancellationToken cancellationToken)
            {
                var result = await _strategicObject.GetAllStrategicObjectiveList();
                return result;
            }
        }
    }
}
