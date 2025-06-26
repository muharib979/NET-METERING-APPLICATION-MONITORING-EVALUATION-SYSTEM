using Core.Application.Interfaces.DatabaseConfig;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig
{
    public class GetdbCodebylocationQuery: IRequest<Core.Domain.DatabaseConfig.DatabaseConfig>
    {
        public string LocationCode { get; set; }

        public class Handler : IRequestHandler<GetdbCodebylocationQuery, Core.Domain.DatabaseConfig.DatabaseConfig>
        {
            private readonly IDatabaseConfigRepository _databaseConfig;
            public Handler(IDatabaseConfigRepository databaseConfig)
            {
                _databaseConfig = databaseConfig;
            }

            public async Task<Core.Domain.DatabaseConfig.DatabaseConfig> Handle(GetdbCodebylocationQuery request, CancellationToken cancellationToken)
            {
                var result = await _databaseConfig.GetDatabaseDataBylocCodeAsync(request.LocationCode);
                return result;
            }
        }
    }
}
