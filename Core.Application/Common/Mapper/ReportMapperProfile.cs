using Core.Domain.MISReport;
using Core.Domain.Report;
using Core.Domain.Untracable;
using Shared.DTOs.MISReport;

namespace Core.Application.Common.Mapper
{
    public class ReportMapperProfile : Profile
    {
        public ReportMapperProfile()
        {
            CreateMap<BillGroupDto, BillGroup>()
                    .ForMember(d => d.BILL_GROUP, o => o.MapFrom(s => s.BillGroup))
                    .ForMember(d => d.DB_NAME, o => o.MapFrom(s => s.DbName))
                    .ReverseMap();

            CreateMap<BookDto, Book>()
                   .ForMember(d => d.BOOK_NUM, o => o.MapFrom(s => s.BookNum))
                   .ForMember(d => d.DB_NAME, o => o.MapFrom(s => s.DbName))
                   .ForMember(d => d.BOOK_ID, o => o.MapFrom(s => s.BookId))
                   .ReverseMap();

            CreateMap<CustomerArrearDto,CustomerArrear>()
                   .ForMember(d => d.LOC, o => o.MapFrom(s => s.Loc))
                   .ForMember(d => d.OFFICE, o => o.MapFrom(s => s.Office))
                   .ForMember(d => d.BG, o => o.MapFrom(s => s.Bg))
                   .ForMember(d => d.B_K, o => o.MapFrom(s => s.Bk))
                   .ForMember(d => d.CON_NO, o => o.MapFrom(s => s.ConNo))
                   .ForMember(d => d.WLK_OR, o => o.MapFrom(s => s.WlkOr))
                   .ForMember(d => d.PV_AC, o => o.MapFrom(s => s.PvAc))
                   .ForMember(d => d.NAME, o => o.MapFrom(s => s.Name))
                   .ForMember(d => d.FATHER_NAME, o => o.MapFrom(s => s.FatherName))
                   .ForMember(d => d.ADDR, o => o.MapFrom(s => s.Addr))
                   .ForMember(d => d.LIKELY, o => o.MapFrom(s => s.Likely))
                   .ForMember(d => d.TARIFF, o => o.MapFrom(s => s.Tarrif))
                   .ForMember(d => d.NOM, o => o.MapFrom(s => s.Nom))
                   .ForMember(d => d.PRINCIPAL_ARREAR, o => o.MapFrom(s => s.PrincipalArrear))
                   .ForMember(d => d.LPS_ARREAR, o => o.MapFrom(s => s.LpsArrear))
                   .ForMember(d => d.VAT_ARREAR, o => o.MapFrom(s => s.VatArrear))
                   .ForMember(d => d.TOTAL_ARREAR, o => o.MapFrom(s => s.TotalArrear))
                   .ForMember(d => d.BILL_CYCLE_CODE, o => o.MapFrom(s => s.BillCycleCode))
                   .ForMember(d => d.DISC_DATE, o => o.MapFrom(s => s.DiscDate))
                   .ForMember(d => d.STATUS, o => o.MapFrom(s => s.Status))
                   .ReverseMap();

            CreateMap<AllCustomerArrearSummaryDto, AllCustoemrArrearSummary>()
                 .ForMember(d => d.CENTER, o => o.MapFrom(s => s.Center))
                 .ForMember(d => d.NOC, o => o.MapFrom(s => s.Noc))
                 .ForMember(d => d.ARR_PRIN, o => o.MapFrom(s => s.ArrPrn))
                 .ForMember(d => d.ARR_LPS, o => o.MapFrom(s => s.ArrLps))
                 .ForMember(d => d.ARR_VAT, o => o.MapFrom(s => s.ArrVat))
                 .ForMember(d => d.TOTAL_BILL, o => o.MapFrom(s => s.TotalBill))
                 .ForMember(d => d.ORDER, o => o.MapFrom(s => s.Order))
                 .ForMember(d => d.LOC, o => o.MapFrom(s => s.Loc))
                 .ForMember(d => d.OFFICE, o => o.MapFrom(s => s.Office))

                 .ReverseMap();


            CreateMap<MergeUntraceableDto, MergeUntraceable>()
                .ForMember(d => d.NAME, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ADDR, o => o.MapFrom(s => s.Addr))
                .ForMember(d => d.CON_NO, o => o.MapFrom(s => s.ConNo))
                .ForMember(d => d.PV_AC, o => o.MapFrom(s => s.AccountNo))
                .ForMember(d => d.TOTAL_ARREAR, o => o.MapFrom(s => s.TatalArrear))
                .ForMember(d => d.STATUS, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.DISC_DATE, o => o.MapFrom(s => s.DisDate))
                .ForMember(d => d.CUST_ID, o => o.MapFrom(s => s.CustId))
                .ForMember(d => d.LAST_BILL_MONTH, o => o.MapFrom(s => s.LastBillMonth))
                .ForMember(d => d.UC_TYPE, o => o.MapFrom(s => s.UcType))
                .ReverseMap();
        }

    }
}