using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class TargetDTO
    {
        public int? Id { get; set; }
        public string? FiscalYearCode { get; set; }

        public string? FiscalYear { get; set; }
        public string? PerformanceIndexCode { get; set; }
        public string? PerformanceIndex { get; set; }
        public int? Value { get; set; }
    }
}
