using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISReport
{
    public class IllegalConsumerPenaltyDTO
    {
       
        public string? LocationCode { get; set; }
        public decimal? PrevAmount { get; set; }
        public string? PrevConsumer { get; set; }
        public decimal? PrevReceiptAmount { get; set; }
        public decimal? CurrAmount { get; set; }
        public string? CurrConsumer { get; set; }
        public decimal? CurrReceiptAmount { get; set; }
        public string? TotalConsumer { get; set; }
        public decimal? TotalPenaltyAmount { get; set; }
        public decimal? TotalReceiptAmount { get; set; }


    }
}
