using Core.Application.Interfaces.MinistryCustomer;
using Nancy;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MinistryCustomer.DeleteMinistryCustomer
{
    public class DeleteMinistryDataCommandHandler:IRequest<int>
    {
        public int customerNo { get; set; }
        public class Handler : IRequestHandler<DeleteMinistryDataCommandHandler, int>
        {
            private readonly IMinistryCustomerRepository _repository;
            public Handler(IMinistryCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<int> Handle(DeleteMinistryDataCommandHandler request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteMinistryCustomer(request.customerNo);
                return result;
            }
        }
    }
}
