using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Temporary
{
    public class BillCycleListDTO
    {
        public string? LocationCode { get; set; }
        public string? BillGroup { get; set; }
        public string? BillCycleCode { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public string? BillDate { get; set; }
    }
}
