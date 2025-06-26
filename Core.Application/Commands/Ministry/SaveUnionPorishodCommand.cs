using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Ministry
{
    public class SaveUnionPorishodCommand: UnionPorishodDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveUnionPorishodCommand, bool>
        {
            private readonly IMinistryRepository _repository;

            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
            }

            public  async Task<bool> Handle(SaveUnionPorishodCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveUnionPorishodBill(request);
                return result;
            }
        }

    }
}
