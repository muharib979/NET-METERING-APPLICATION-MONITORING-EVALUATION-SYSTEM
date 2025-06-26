using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Reconciliation
{
    public class ReconcilationStatus
    {
        public string? PAY_DATE { get; set; }

        public int? NO_OF_TRANSACTION { get; set; }

        public int? TOTAL_AMOUNT { get; set; }

        public int? PRINCIPAL_AMOUNT { get; set; }
        public int? VAT_AMOUNT { get; set; }
        public string? UPDATE_BY { get; set; }
    }
}
