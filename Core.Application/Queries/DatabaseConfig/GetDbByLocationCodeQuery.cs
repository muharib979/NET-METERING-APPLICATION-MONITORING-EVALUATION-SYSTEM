using Core.Application.Interfaces.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig
{
    public class GetDbByLocationCodeQuery : IRequest<DropdownResultForStringKey>
    {
        public string LocationCode { get; set; }
        public class Handler : IRequestHandler<GetDbByLocationCodeQuery, DropdownResultForStringKey>
        {
            private readonly IDatabaseConfigRepository _repository;
            public Handler(IDatabaseConfigRepository repository)
            {
                _repository = repository;
            }
            public async Task<DropdownResultForStringKey> Handle(GetDbByLocationCodeQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetDbByLocationCode(request.LocationCode);
            }
        }
    }
}
