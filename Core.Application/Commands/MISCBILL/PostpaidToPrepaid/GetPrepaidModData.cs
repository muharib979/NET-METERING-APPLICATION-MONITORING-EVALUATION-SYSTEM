using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.PostpaidToPrepaid
{
    public class GetPrepaidModData : PrePaidToPostPaidMOD, IRequest<Result>
    {
        public class Handler : IRequestHandler<GetPrepaidModData, Result>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;

            }

            public async Task<Result> Handle(GetPrepaidModData request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPrepaidMODDATA(request);

                return result;
            }
        }
    }
}

