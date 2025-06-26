using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class PreModDataDTO
    {
        public int Code { get; set; }
        public string TariffName { get; set; }
        public int ConsumerNo { get; set; }
        public int VendCust { get; set; }
        public int VandingCustomer { get; set; }
        public int SoldUnit { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal RepayAmount { get; set; }
        public decimal EnergyAmount { get; set; }
        public decimal DemandCharge { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal ArrearAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int MeterRent1p { get; set; }
        public int MeterRent3p { get; set; }
        public int Rate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int IsTransfer { get; set; }
        public string DeptCode { get; set; }
        public string FromModDate { get; set; }
        public string ToModDate { get; set; }
        public string BillCycleCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
    }

    public class MODLocationModel
    {
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string DB_CODE { get;}
    }
}
