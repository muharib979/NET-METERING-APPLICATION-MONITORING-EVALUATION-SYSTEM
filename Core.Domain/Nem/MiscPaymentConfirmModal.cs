using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Nem
{
    public class MiscPaymentConfirmModal
    {
        public string BillNumber { get; set; }
        //public string LocationCode { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
