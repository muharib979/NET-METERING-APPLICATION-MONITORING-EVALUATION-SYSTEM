using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistryLedgerDTO
    {
        public string Code { get; set; }
        public string MinistryName { get; set; }
        public string MinistryNameBn { get; set; }

        //public decimal TotalArrear { get; set; }
        //public decimal TotalCurrAmount { get; set; }
        //public decimal TotalReceiptArrear { get; set; }
        //public decimal TotalReceipt { get; set; }

        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalArrearAmt { get; set; }

    }
}
