using Core.Application.Interfaces.Location;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationByDbCode
{
    public class GetLocationByDbCodeQuery:IRequest<Response<List<DropdownResultForStringKey>>>
    {
        public string DbCode { get; set; }
        public class Handler:IRequestHandler<GetLocationByDbCodeQuery,Response<List<DropdownResultForStringKey>>>
        {
            private readonly ILocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILocationRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<DropdownResultForStringKey>>> Handle(GetLocationByDbCodeQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    
                    var result = await _repository.GetLocationBydbcode(request.DbCode);
                    return Response<List<DropdownResultForStringKey>>.Success(_mapper.Map<List<DropdownResultForStringKey>>(result), "Successfully Retrived");

                }
                catch (Exception ex)
                {
                    return Response<List<DropdownResultForStringKey>>.Fail("Error");
                }
            }
        }

    }
}
