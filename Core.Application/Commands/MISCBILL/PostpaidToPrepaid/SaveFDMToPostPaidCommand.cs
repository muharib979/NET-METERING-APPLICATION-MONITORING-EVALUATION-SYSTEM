using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.PostpaidToPrepaid
{
    public class SaveFDMToPostPaidCommand : PostpaidCustFDMDTO, IRequest< string>
    {
        public class Handler : IRequestHandler<SaveFDMToPostPaidCommand, string>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<string> Handle(SaveFDMToPostPaidCommand request, CancellationToken cancellationToken)
            {
                return (await _repository.FDMToPostpaidSave(request));
            }
        }
    }
}

