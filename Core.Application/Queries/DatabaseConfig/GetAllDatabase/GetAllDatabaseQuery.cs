using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetAllDatabase
{
    public class GetAllDatabaseQuery : PaginationParams, IRequest<Response<PaginatedList<DatabaseConfigDto>>>
    {
    }
}
