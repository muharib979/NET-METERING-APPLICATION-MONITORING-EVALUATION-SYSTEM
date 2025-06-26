//using Core.Application.Interfaces.VisitorDetails;
//using Shared.DTOs.VisitorDetails;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Application.Commands.VisitorDetails
//{
//    public class SaveVisitorDetailsCommand: VisitorCountDTO, IRequest<bool>
//    {
//        public class Handler : IRequestHandler<SaveVisitorDetailsCommand, bool>
//        {
//            private readonly IVisitorDetails _visitorDetails;
//            public Handler(IVisitorDetails visitorDetails)
//            {
//                _visitorDetails = visitorDetails;
//            }
//            public async Task<bool> Handle(SaveVisitorDetailsCommand request, CancellationToken cancellationToken)
//            {
//                var result = await _visitorDetails.SaveVisitorDetails(request);
//                return result;
//            }
//        }
//    }
//}
