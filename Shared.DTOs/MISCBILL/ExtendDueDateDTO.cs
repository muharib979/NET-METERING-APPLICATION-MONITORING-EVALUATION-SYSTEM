using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class ExtendDueDateDTO
    {
        
        public int? BillId { get; set; }
        public string? BillNumber { get; set; }
        public string? CustomerNumber { get; set; }
        public string? ExtendDate { get; set; }
        public string? UserName { get; set; }

    }
}
