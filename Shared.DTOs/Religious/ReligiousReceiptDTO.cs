using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Religious
{
    public class ReligiousReceiptDTO
    {
        public int? CustomerNo { get; set; }
        public int? CUST_ID { get; set; }
        public string? CustomerName { get; set; }
        public string? LocationCode { get; set; }
        public int? NewCustomerNumber { get; set; }
        public decimal? ReceiptAmt { get; set; }
        public string? BillMonth { get; set; }
        public string? CurrentDate { get; set; }
        public string? DbCode { get; set; }

        public string? CreateBy { get; set; }
    }


    public class ReligiousReceiptViewModel
    {
        public List<ReligiousReceiptDTO> NewCustomerList { get; set; }
        public List<ReligiousReceiptDTO> CustomerList { get; set; }
    }
}
