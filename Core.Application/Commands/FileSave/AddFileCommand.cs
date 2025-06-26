using Core.Application.Commands.MISCBILL;
using Core.Application.Interfaces;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.FileSave
{
    public class AddFileCommand : List<FileSaveDto>, IRequest<string>
    {
        public class Handler : IRequestHandler<AddFileCommand, string>
        {
            private readonly IFileSaveRepository _repository;

            public Handler(IFileSaveRepository repository)
            {
                _repository = repository;
            }

            public async Task<string> Handle(AddFileCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.AddAsync(request);
                return result;

            }
        }
    }
}
