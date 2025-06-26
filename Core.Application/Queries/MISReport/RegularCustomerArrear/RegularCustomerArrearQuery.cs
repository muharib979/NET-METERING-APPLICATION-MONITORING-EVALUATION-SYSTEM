using Core.Application.Interfaces.MISReport;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.RegularCustomerArrear
{
    

    public class RegularCustomerArrearQuery : IRequest<Response<List<RegularCustomerArrearDTO>>>
    {

        public int[] dbIds { get; set; }

        public int[] locationIds { get; set; }

        public string BillMonth { get; set; }

        public int ConnStatusId { get; set; }
        public int ArrearFrom { get; set; }

        public int  ArrrearTo { get; set; }

        public int TariffId { get; set; }

        public string Tariff { get; set; }
        public string BillGroupId { get; set; }

        public string BookId { get; set; }

        public bool isAll { get; set; }

        public bool isPrincipal { get; set; }

        public bool isVAT { get; set; }

        public bool isLPS { get; set; }

        public string OrderTypeId { get; set; }
        public int UId { get; set; }

        public int roleId { get; set; }

        public class Handler : IRequestHandler<RegularCustomerArrearQuery, Response<List<RegularCustomerArrearDTO>>>
        {
            private readonly IMisReportRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IMisReportRepository misReportRepository, IMapper mapper)
            {
                _repository = misReportRepository;
                _mapper = mapper;
            }

            public async Task<Response<List<RegularCustomerArrearDTO>>> Handle(RegularCustomerArrearQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetArrearRegularCustomerData(request.dbIds, request.locationIds, request.BillMonth, request.ConnStatusId, request.ArrearFrom, request.ArrrearTo, request.TariffId, request.Tariff, request.BillGroupId, request.BookId, request.isAll, request.isPrincipal, request.isVAT, request.isLPS, request.
                    OrderTypeId, request.UId,request.roleId);
                return Response<List<RegularCustomerArrearDTO>>.Success(_mapper.Map<List<RegularCustomerArrearDTO>>(result), "Success");
            }
        }
    }
}
