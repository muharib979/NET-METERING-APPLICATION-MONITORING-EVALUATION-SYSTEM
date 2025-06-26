using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Temporary
{
    public class CensusBillDTO
    {
        public string? Address { get; set; }
        public List<InitialReadingList>? InitialReadingList { get; set; }
        public string? BillGroup { get; set; }
        public string? BlcokNumber { get; set; }
        public string? BusinessType { get; set; }
        public string? ConnectedLoad { get; set; }
        public string? CustomerName { get; set; }
        public string? FatherName { get; set; }
        public string? Feederdesc { get; set; }
        public string? FormerCapacity { get; set; }
        public string? FormerNumber { get; set; }
        public string? LocationCode { get; set; }
        public string? MeterCondition { get; set; }
        public string? MeterDigit { get; set; }
        public string? MeterNumber { get; set; }
        public string? MeterOwner { get; set; }
        public string? MeterType { get; set; }
        public string? MobileNumber { get; set; }
        public decimal? MonthlyConsumption { get; set; }
        public string? NidNUmber { get; set; }
        public string? OldAcNo { get; set; }
        public decimal? Omf { get; set; }
        public string? SanctionLoad { get; set; }
        public string? Tarrif { get; set; }
        public string? WalkOrder { get; set; }
        public string? UserName { get; set; }
    }

    public class InitialReadingList
    {
        public decimal Reading { get; set; }
        public string? TodCode { get; set; }
        public string? TodDesc { get; set; }
        public string? TimeCycleCode { get; set; }
        public string? TimeCycleDesc { get; set; }
        public string? ReadingTypeCode { get; set; }
        public string? ReadingTypeDesc { get; set; }
        public DateTime ReadingDate { get; set; }
    }
}
