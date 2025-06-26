using Core.Application.Interfaces.DatabaseConfig;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetAllDatabase
{
    public class GetAllDatabaseHandler : IRequestHandler<GetAllDatabaseQuery, Response<PaginatedList<DatabaseConfigDto>>>
    {
        private readonly IDatabaseConfigService _service;
        private readonly List<string> _validationMessage;
        public GetAllDatabaseHandler(IDatabaseConfigService service)
        {
            _service = service;
            //_validationMessage = new List<string>();
        }
        

        public async Task<Response<PaginatedList<DatabaseConfigDto>>> Handle(GetAllDatabaseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var databaseConfig = await _service.GetAllAsync(request);
                if (databaseConfig == null)
                {
                    
                    return Response<PaginatedList<DatabaseConfigDto>>.Fail("No Database Information Found");
                }
                
                var paginatedList = PaginatedList<DatabaseConfigDto>.Create(databaseConfig, request.PageNumber, request.pageSize, 10);

                return Response<PaginatedList<DatabaseConfigDto>>.Success(paginatedList, "Successfully Retrieved All Database Infos");
            }
            catch (Exception e)
            {
                
                return Response<PaginatedList<DatabaseConfigDto>>.Fail("Failed to Retrieve Database Infos");
            }
        }
    }
}   
