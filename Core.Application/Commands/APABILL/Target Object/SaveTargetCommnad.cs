using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Target_Object
{
    public class SaveTargetCommnad: TargetDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveTargetCommnad, bool>
        {
            private readonly ITargetRepository _repository;

            public Handler(ITargetRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveTargetCommnad request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveTargetBill(request);
                return result;

            }
        }
    }
}
