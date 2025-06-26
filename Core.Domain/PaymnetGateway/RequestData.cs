using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.PaymnetGateway
{
    public class RequestData
    {
        public string? BILL_NO { get; set; }
        public string? CUSTOMER_NUM { get; set; }
        public string? LOCATION_CODE { get; set; }
        public string? CUST_NAME { get; set; }
        public string? CUST_ADDRESS { get; set; }
        public string? PHONE { get; set; }
        public decimal? TOTAL_BILL_AMOUNT { get; set; }
        public decimal? TOTAL_PRINCIPAL_AMOUNT { get; set; }
        public decimal? TOT_VAT_AMOUNT { get; set; }
        public string? TransactionId { get; set; }
        public string? UserName { get; set; }
        public string? OrderId { get; set; }
    }
}
