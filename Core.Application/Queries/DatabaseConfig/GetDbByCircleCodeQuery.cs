using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetAllLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig
{
    public class GetDbByCircleCodeQuery : IRequest<List<DropdownResultForStringKey>>
    {
        public string CircleCode { get; set; }
        public class Handler : IRequestHandler<GetDbByCircleCodeQuery, List<DropdownResultForStringKey>>
        {
            private readonly IDatabaseConfigRepository _repository;
            public Handler(IDatabaseConfigRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetDbByCircleCodeQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetDbByCircleCode(request.CircleCode);
            }
        }
    }
}
