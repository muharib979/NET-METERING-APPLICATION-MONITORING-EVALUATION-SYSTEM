using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PostpaidCustFDMDTO
    {
        public string? TransId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerType { get; set; }
        public string? LocationCode { get; set; }
        public string? FatherName { get; set; }
        public string? NID { get; set; }
        public string? PowerUtility { get; set; }
        public string? MaxPower { get; set; }
        public string? Mobile { get; set; }
        public string? AreaCode { get; set; }
        public string? MeterTypeCode { get; set; }
        public string? MeterNumber { get; set; }
        public string? MeterRemoveDate { get; set; }
        public string? TinNumber { get; set; }
        public int ? SanctionLoad { get; set; }
        public string Address { get; set; }
        public string? BusinessType { get; set; }
        public string? TariffName { get; set; }
        public decimal? ArrearAmount { get; set; }
        public string? ContactName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public string? Thana { get; set; }
        public string? MeterSealLeft { get; set; }
        public string? MeterSealRight { get; set; }
        public string? TerminalLoss { get; set; }
        public string? PrepaidCustomerNumber { get; set; }
        public string? PrepaidMeterNumber { get; set; }
        public decimal? LastReadingPeak { get; set; }
        public decimal? LastReadingOffPeak { get; set; }
        public decimal? LastReadingSr { get; set; }
        public string? InstalationDate { get; set; }
        public decimal? LastBillReadingSr { get; set; }
        public decimal? LastBillReadingPeak { get; set; }
        public decimal? LastBillReadingOffPeak { get; set; }
        public int? WalkOrder { get; set; }
        public string? TransferBy { get; set; }
    }
}
