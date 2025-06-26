using Core.Application.Interfaces.VisitorDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.VisitorDetails
{
    public class GetVisitorCountQuery: IRequest<object>
    {
        public class Handler : IRequestHandler<GetVisitorCountQuery, object>
        {
            private readonly IVisitorDetails _visitorDetails;
            public Handler(IVisitorDetails visitorDetails)
            {
                _visitorDetails = visitorDetails;
            }
            public async Task<object> Handle(GetVisitorCountQuery request, CancellationToken cancellationToken)
            {
                var result = await _visitorDetails.GetVisitorCount();
                return result;
            }
        }
    }
}
