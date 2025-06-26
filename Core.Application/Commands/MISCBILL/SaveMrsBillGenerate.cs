using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveMrsBillGenerate : List<MRSGenarateDTO>, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveMrsBillGenerate, bool>
        {
            private readonly IMrsGenarateRepository _repository;

            public Handler(IMrsGenarateRepository repository)
            {
                _repository = repository;
            }

           
            public async Task<bool> Handle(SaveMrsBillGenerate request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveMrsGenerate(request);
                return result;

            }
        }
    }
}
