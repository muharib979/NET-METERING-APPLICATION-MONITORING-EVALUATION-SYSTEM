using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.IndexUnit_Object
{
    public class SaveUnitIndexCommand: IndexUnitDto, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveUnitIndexCommand, bool>
        {
            private readonly IIndexUnitRepository _repository;

            public Handler(IIndexUnitRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveUnitIndexCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveIndexUnitBill(request);
                return result;

            }
        }
    }
}
