using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class UpdateRcDateCommand : IRequest<bool>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public string? rcDate { get; set; }
        public string? userName { get; set; }
        public class Handler : IRequestHandler<UpdateRcDateCommand, bool>
        {
            private readonly IDcRcBillGenerateRepository _repository;

            public Handler(IDcRcBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(UpdateRcDateCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.UpdateRcDate(request.billNumber,request.customerNumber,request.rcDate,request.userName);
                return result;

            }
        }
    }
}
