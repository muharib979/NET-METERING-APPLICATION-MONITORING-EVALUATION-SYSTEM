using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class FinancialYearDTO
    {
        public int? Id { get; set; }
        public string? FinancialName { get; set; }
        public string? StartMonth { get; set; }
        public string? EndMonth { get; set; }
        public string? Code { get; set; }
    }
}
