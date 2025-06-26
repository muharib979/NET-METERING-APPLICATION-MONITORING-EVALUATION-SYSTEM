using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class LocationWiseArrearDTO
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string ZoneNameBn { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string CircleNameBn { get; set; }
        public string LocationNameBn { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int ConsumerNo { get; set; }
        public string BillMonth { get; set; }
        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalArrearAmt { get; set; }

        //public decimal MinistryArrearAmt { get; set; }
        //public decimal CurrentBillAmt { get; set; }
        //public decimal TotalReceiptArrear { get; set; }
        //public decimal TotalReceipt { get; set; }

        //public decimal TotalReceiptAmt { get; set; }
        //public decimal ArrAfterReceiptAmt { get; set; }
        //public decimal TotalReceiptArrear { get; set; }
    }
}
