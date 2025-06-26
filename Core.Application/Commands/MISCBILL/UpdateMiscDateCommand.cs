using Core.Application.Interfaces.MiscBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class UpdateMiscDateCommand: IRequest<bool>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public string? rcDate { get; set; }

        public class Handler : IRequestHandler<UpdateMiscDateCommand, bool>
        {
            private readonly IMiscChargeRepository _repository;

            public Handler(IMiscChargeRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(UpdateMiscDateCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.UpdateMiscDate(request.billNumber, request.customerNumber, request.rcDate);
                return result;

            }
        }
    }
}
