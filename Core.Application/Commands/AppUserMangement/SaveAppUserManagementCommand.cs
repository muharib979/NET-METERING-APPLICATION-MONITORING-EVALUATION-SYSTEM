using Core.Application.Interfaces.AppUserManagement;
using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.AppUserMangement
{
    public class SaveAppUserManagementCommand : AppUserManagementDTO, IRequest<int>
    {
        public class Handler : IRequestHandler<SaveAppUserManagementCommand, int>
        {
            private readonly IAppUserManagement _repository;
            public Handler(IAppUserManagement repository)
            {
                _repository=repository;
            }

            public async Task<int> Handle(SaveAppUserManagementCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveAppUserManagementBill(request);
                return result;
            }
        }

    }
}
