using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MinistryCustomer.AddMinistryCustomer
{
    public class AddMinstryDataCommandHandler: MinistryCustomerDTOs,IRequest<MinistryDataViewDTO>
    {
        public class Handler : IRequestHandler<AddMinstryDataCommandHandler, MinistryDataViewDTO>
        {
            private  readonly IMinistryCustomerRepository _repository;

            public Handler(IMinistryCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<MinistryDataViewDTO> Handle(AddMinstryDataCommandHandler request, CancellationToken cancellationToken)
            {
                var result=await _repository.SaveMinistryCustomer(request);
                return result;
            }
        }
    }
}
