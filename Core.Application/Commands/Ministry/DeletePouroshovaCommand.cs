using Core.Application.Interfaces.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Ministry
{
    public class DeletePouroshovaCommand: IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeletePouroshovaCommand, int>
        {
            private readonly IMinistryRepository _repository;

            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
            }



            public async Task<int> Handle(DeletePouroshovaCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeletePouroshovaBill(request.id);
                return result;
            }
        }
    }
}
