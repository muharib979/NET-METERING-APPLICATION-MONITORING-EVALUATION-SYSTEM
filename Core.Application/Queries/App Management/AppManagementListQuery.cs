using Core.Application.Interfaces.AppUserManagement;
using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.App_Management
{
    public class AppManagementListQuery : IRequest<List<AppUserManagementDTO>>
    {
        public class Handler : IRequestHandler<AppManagementListQuery, List<AppUserManagementDTO>>
        {
            private readonly IAppUserManagement _repository;
            public Handler(IAppUserManagement programRepository)
            {
                _repository = programRepository;
            }
            public async Task<List<AppUserManagementDTO>> Handle(AppManagementListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAppUserManagementList();
                return result;
            }
        }
    }
}
