using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class InitialReadingDTO
    {
        public string? TodCode { get; set; }
        public string? TodDesc { get; set; }
        public string? TimeCycleCode { get; set; }
        public string? TimeCycleDesc { get; set; }
        public string? ReadingTypeCode { get; set; }
        public string? ReadingTypeDesc { get; set; }
        public string? ReadingDate { get; set; }
    }
}
