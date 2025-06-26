using Core.Application.Interfaces.DatabaseConfig;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetDbConfigDDList
{
    public class GetDbConfigDDListQuery:IRequest<Response<List<DropdownResultForStringKey>>>
    {
        public class Handler:IRequestHandler<GetDbConfigDDListQuery,Response<List<DropdownResultForStringKey>>>
        {
            private readonly IDatabaseConfigRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IDatabaseConfigRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            } 
            public async Task<Response<List<DropdownResultForStringKey>>>Handle(GetDbConfigDDListQuery request,CancellationToken cancellationtoken)
            {
                try
                {
                    var result = await _repository.GetAllDbConfigDDList();
                    return Response<List<DropdownResultForStringKey>>.Success(_mapper.Map<List<DropdownResultForStringKey>>(result), "Successfully Retrived");
                }
                catch (Exception ex)
                {

                    return Response<List<DropdownResultForStringKey>>.Fail("Error Occur");
                }
            }
        }
    }
}
