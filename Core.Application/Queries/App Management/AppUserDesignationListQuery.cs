using Core.Application.Interfaces.AppUserManagement;
using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.App_Management
{
    public class AppUserDesignationListQuery : IRequest<List<AppUserDesignationDTO>>
    {
        public class Handler : IRequestHandler<AppUserDesignationListQuery, List<AppUserDesignationDTO>>
        {
            private readonly IAppUserManagement _repository;
            public Handler(IAppUserManagement programRepository)
            {
                _repository = programRepository;
            }
            public async Task<List<AppUserDesignationDTO>> Handle(AppUserDesignationListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAppUserDesignationList();
                return result;
            }
        }
    }
}
