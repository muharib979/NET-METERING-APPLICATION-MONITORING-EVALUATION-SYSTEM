using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.PostpaidToPrepaid
{
    public class UpdatePostToPrepaidCommand: List<PostpaidCustDetailsDTO>, IRequest<int>
    {
        public class Handler : IRequestHandler<UpdatePostToPrepaidCommand, int>
        {
            private readonly IPostpaidCustomerRepository _postPaidCustRepo;
            public Handler(IPostpaidCustomerRepository postPaidCustRepo)
            {
                _postPaidCustRepo = postPaidCustRepo;
            }
            public async Task<int> Handle(UpdatePostToPrepaidCommand request, CancellationToken cancellationToken)
            {
                var result = await _postPaidCustRepo.UpdatePostToPrepaidCustomer(request);
                return result;
            }
        }
    }
}
