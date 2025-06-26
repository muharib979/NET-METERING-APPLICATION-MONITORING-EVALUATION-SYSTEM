using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentConfirmation
{
    public class PaymentConfirmationDto
    {
        public string CustomerNumber { get; set; }
        public decimal TotalBillAmount { get; set; }
        public string LocationCode { get; set; }
        public int BillNumber { get; set; }
        public string PaymentDate { get; set; }
    }
}
