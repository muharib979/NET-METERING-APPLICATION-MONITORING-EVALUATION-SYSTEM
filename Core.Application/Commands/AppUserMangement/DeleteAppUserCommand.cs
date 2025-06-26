using Core.Application.Interfaces.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.AppUserMangement
{
    public class DeleteAppUserCommand:IRequest<int>
    {
        public int id { get; set; }

        public class Handler : IRequestHandler<DeleteAppUserCommand, int>
        {
            private readonly IAppUserManagement _repository;

            public Handler(IAppUserManagement repository)
            {
                _repository = repository;
            }



          

            public async Task<int> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.DeleteAppMangementBill(request.id);
                return result;
            }
        }

    }
}
