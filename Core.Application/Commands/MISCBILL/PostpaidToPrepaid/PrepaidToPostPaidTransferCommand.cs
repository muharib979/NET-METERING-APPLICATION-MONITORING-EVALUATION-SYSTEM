using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.PostpaidToPrepaid
{
    public class PrepaidToPostPaidTransferCommand : PrepaidToPostPaidTransferDTO, IRequest<string>
    {
        public class Handler : IRequestHandler<PrepaidToPostPaidTransferCommand,  string>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<string> Handle(PrepaidToPostPaidTransferCommand request, CancellationToken cancellationToken)
            {
                return (await _repository.PrepaidToPostPaid(request));
            }
        }
    }
}

