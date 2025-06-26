using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Reconciliation
{
    public class ReconcilationStatusDTO
    {
        public string? PayDate { get; set; }

        public int? NoOfTransaction { get; set; }

        public int? TotalAmount { get; set; }

        public int? PrincipleAmount { get; set; }
        public int? VatAmount { get; set; }

       public string? User { get; set; }
   
    }
}
