using Core.Application.Interfaces.DatabaseConfig;
using Shared.DTOs.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetDBbyUserId
{
    public class GetDBbyUserIdQuery:IRequest<List<Core.Domain.DatabaseConfig.DatabaseConfig>>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public class Handler:IRequestHandler<GetDBbyUserIdQuery,List<Core.Domain.DatabaseConfig.DatabaseConfig>>
        {
            private readonly IDatabaseConfigRepository _databaseConfigRepository;
            public Handler(IDatabaseConfigRepository databaseConfigRepository)
            {
                _databaseConfigRepository = databaseConfigRepository;
            }

            public async Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> Handle(GetDBbyUserIdQuery request, CancellationToken cancellationToken)
            {
                var result =await _databaseConfigRepository.GetDatabaseByUserIdRoleId(request.UserId, request.RoleId);
                return result;
            }
        }
    }
}
