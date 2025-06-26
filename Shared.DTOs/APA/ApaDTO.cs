using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class ApaDTO
    {
        public int? Id { get; set; }
        public string? StratigicCode { get; set; }
        public string? ProgramCode { get; set; }
        public string? PerformanceCode { get; set; }
        public string? PerformanceName { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }
        public string? IndexName { get; set; }
        public string? TargetName { get; set; }
        public string? BillMonth { get; set; }
        public decimal? FinancialAmount { get; set; }
        public string? Comments { get; set; }
    }
}
