using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.PaymnetGateway
{
    public class PaymentGatwayDTO
    {
        public decimal? TotalAmount { get; set; }
        public string? BaseUrl { get; set; }
        public string? ConfirmUrl { get; set; }
        public string? FailedUrl { get; set; }
        public string? CancelUrl { get; set; }
        public string? CustomerNumber { get; set; }
        public string? BillNumber { get; set; }
    }
    public class PaymnetUrl
    {
         public string? url { get; set; } 
    }

}
