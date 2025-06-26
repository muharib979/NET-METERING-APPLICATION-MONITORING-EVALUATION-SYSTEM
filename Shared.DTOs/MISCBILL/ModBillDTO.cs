using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class ModBillDTO
    {
        public int Sl { get; set; }
        public string ColumnName { get; set; }
        public decimal ColumnValue { get; set; }
        public string CenterName { get; set; }
    }
}
