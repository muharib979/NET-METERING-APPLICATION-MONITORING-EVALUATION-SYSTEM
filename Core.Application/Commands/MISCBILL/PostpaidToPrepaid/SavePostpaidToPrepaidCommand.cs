using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.PostpaidToPrepaid
{
    public class SavePostpaidToPrepaidCommand : PostpaidCustDetailsDTO, IRequest<(bool IsSaved, string messages)>
    {
        public class Handler : IRequestHandler<SavePostpaidToPrepaidCommand, (bool IsSaved, string messages)>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<(bool IsSaved, string messages)> Handle(SavePostpaidToPrepaidCommand request, CancellationToken cancellationToken)
            {
                return (await _repository.SavePostpaidToPrepaid(request));
            }
        }
    }
}
