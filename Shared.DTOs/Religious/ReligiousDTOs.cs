

namespace Shared.DTOs.Religious
{
    public class ReligiousDTOs
    {
        public string CustomerName { get; set; }
        public int CustomerNo { get; set; }
        public string Address { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ZoneName { get; set; }
        public int NoWorship { get; set; }
        public decimal Prn { get; set; }
        public decimal Vat { get; set; }
        public decimal Lps { get; set; }
        public decimal TotalArrear { get; set; }
    }

    public class NoOfMosqueAndOtherPlaceOfWorshipDTO 
    {
        public string CircleName { get; set; }
        public string CircleCode { get; set; }
        public int ConsumerCount { get; set; }
        public decimal Principal { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public string SetupMonth { get; set; }
    }

    public class LocationWiseReligiousRptDTO 
    {
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string ZoneName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        public int ConsumerCount { get; set; }
        public int CountMonth { get; set; }
        public string RebaitUnit { get; set; }
        public string TariffRate { get; set; }
        public decimal Principal { get; set; }
        public decimal DemandAmount { get; set; }
        public decimal DemandRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal WithoutVat { get; set; }
        public string SetupMonth { get; set; }
    }
}
