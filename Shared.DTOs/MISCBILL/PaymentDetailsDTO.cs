using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PaymentDetailsDTO
    {
        public string? TransactionId { get; set; }
        public string? BillNumber { get; set; }
        public string? LocationCode { get; set; }
        public string? CutomerNumber { get; set; }
        public Decimal? PayAmount { get; set; }
        public DateTime? PaymentDate { get; set; }

    }
}
