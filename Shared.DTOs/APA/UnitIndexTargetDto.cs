using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class UnitIndexAndPerformValueDto
    {
        public string NameBn { get; set; }
        public string Code { get; set; }
        public string UnitNameBn { get; set; }
        public string UnitCode { get; set; }
        public decimal Value { get; set; }
    }

    public class TargetDto 
    {
        public string FiscalYearCode { get; set; }
        public string PerformCode { get; set; }
        public decimal TargetValue { get; set; }
    }
}
