using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentConfirmation
{
    public class PaymentStatusDto
    {
        public string BillNumber { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentDate { get; set; }
    }
}
