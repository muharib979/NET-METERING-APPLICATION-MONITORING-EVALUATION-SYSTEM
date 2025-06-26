using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class MRSGenarateDTO
    {
      
        public int? MeterReadingId { get; set; }
        public int? ReadingId { get; set; }
        public int? CustId { get; set; }
        public string? CustomerNum { get; set; }
        public string? CustomerName { get; set; }
        public string? WalkSequence { get; set; }
        public string? MeterNumber { get; set; }
        public string? TodCode { get; set; }
        public string? TodDesc { get; set; }
        public string? TimeCycleCode { get; set; }
        public string? TimeCycleDesc { get; set; }
        public string? ReadingTypeCode { get; set; }
        public string? ReadingDescr { get; set; }
        public string? PresentReading { get; set; }
        public string? OpenReading { get; set; }
        public string? Advance { get; set; }
        public string? MeterCondition { get; set; }
        public string? PowerFactor { get; set; }
        public int? Status { get; set; }
        public string? BillCycleCode { get; set; }
        public string? UserName { get; set; }
    }
}
