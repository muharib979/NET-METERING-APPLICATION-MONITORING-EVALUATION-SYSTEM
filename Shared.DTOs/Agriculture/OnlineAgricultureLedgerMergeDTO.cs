using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Agriculture
{
    public class OnlineAgricultureLedgerMergeDTO
    {
        public int CustId { get; set; }
        public string ConsumerNo { get; set; }
        public string CustomerName { get; set; }
        public string KrishiDeptNameBn { get; set; }
        public string KrishiDepartmentCode { get; set; }
        public string MinistryCode { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        public int IsKrishi { get; set; }
        public int IsPoultry { get; set; }
        public string ConExtgNum { get; set; }
        public string LocationName { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal CurrPrincipal { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }
        public decimal TotalReceiptArrear { get; set; }
        public decimal ArrearReceiptPrincipal { get; set; }
        public decimal ArrearReceiptVat { get; set; }
    }
}
