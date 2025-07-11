﻿using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetOnlineMinistryArrearByBillMonthQueryList
{
    public class GetOnlineMinistryArrearByBillMonthQuery:IRequest<List<OnlineMinistryArrearDetailsMergeDTO>>
    {
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string BillMonth { get; set; }

        public class Handler : IRequestHandler<GetOnlineMinistryArrearByBillMonthQuery, List<OnlineMinistryArrearDetailsMergeDTO>>
        {
            private readonly IMinistryRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IMinistryRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> Handle(GetOnlineMinistryArrearByBillMonthQuery request, CancellationToken cancellationToken)
            {
                List<OnlineMinistryArrearDetailsMergeDTO>? result = await _repository.GetOnlineMinistryArrearByBillMonth(request.ZoneCode,request.LocationCode,request.BillMonth);
                if (result.Count() > 0)
                {
                    var ministryArrearData = result.GroupBy(p => p.MinistryCode).Select(
                        g => new OnlineMinistryArrearDetailsMergeDTO
                        {
                            MinistryCode = g.Key,
                            MinistryName = g.First().MinistryName,
                            MinistryNameBn = g.First().MinistryNameBn,
                            ZoneCode = g.First().ZoneCode,
                            ZoneName = g.First().ZoneName,
                            ZoneNameBn = g.First().ZoneNameBn,
                            HasDepartment = g.First().HasDepartment,
                            DeptName = g.First().DeptName,
                            CurrReceiptPrincipal = g.Sum(p => p.CurrReceiptPrincipal),
                            CurrReceiptVat = g.Sum(p=>p.CurrReceiptVat),
                            CurrLps = g.Sum(p=>p.CurrLps),
                            CurrPrincipal = g.Sum(p=>p.CurrPrincipal),
                            CurrVat = g.Sum(p=>p.CurrVat),
                            ArrearLps = g.Sum(p=>p.ArrearLps),
                            ArrearPrincipal = g.Sum(p=>p.ArrearPrincipal),
                            ArrearVat = g.Sum(p => p.ArrearVat),
                            TotalReceiptArrear = (decimal)g.Sum(p=>p.TotalReceiptArrear),
                            ArrearReceiptPrincipal = g.Sum(p=>p.ArrearReceiptPrincipal),
                            ArrearReceiptVat = g.Sum(p=>p.ArrearReceiptVat)
                        }).OrderBy(p=>p.ORDERNO);
                        

                    return ministryArrearData.ToList();
                }
                else
                {
                    return _mapper.Map<List<OnlineMinistryArrearDetailsMergeDTO>>(result);
                }
                return result;
            }
        }
    }
}
