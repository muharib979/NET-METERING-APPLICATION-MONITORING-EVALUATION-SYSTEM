using Core.Application.Interfaces.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.IndexUnit_Object
{
    public class DeleteUnitIndexCommand: IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeleteUnitIndexCommand, int>
        {
            private readonly IIndexUnitRepository _repository;

            public Handler(IIndexUnitRepository repository)
            {
                _repository = repository;
            }



            public async Task<int> Handle(DeleteUnitIndexCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteIndexUnitBill(request.id);
                return result;
            }
        }
    }
}
