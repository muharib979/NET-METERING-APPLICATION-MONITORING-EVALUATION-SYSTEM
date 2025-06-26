using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Temporary
{
    public class MRSBillCalculationDTO
    {
        public int? Status { get; set; }
        public string? ColumnName { get; set; }
        public string? ColumnValue { get; set; }
    }
}
