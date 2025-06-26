using Core.Application.Interfaces.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.APABILL.Program_object
{
    public class DeleteProgramCommand: IRequest<int>
    {

        public int id { get; set; }

        public class Handler : IRequestHandler<DeleteProgramCommand, int>
        {
            private readonly IProgramRepository _repository;

            public Handler(IProgramRepository repository)
            {
                _repository = repository;
            }



            public async Task<int> Handle(DeleteProgramCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteProgramBill(request.id);
                return result;
            }
        }
    }
}
