using Core.Application.Interfaces.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Target_Object
{
    public class DeleteTargetCommand : IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeleteTargetCommand, int>
        {
            private readonly ITargetRepository _repository;

            public Handler(ITargetRepository repository)
            {
                _repository = repository;
            }



            public async Task<int> Handle(DeleteTargetCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteTargetBill(request.id);
                return result;
            }
        }
    }
}
