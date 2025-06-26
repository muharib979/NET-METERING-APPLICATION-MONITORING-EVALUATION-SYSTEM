using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ministry
{
    public class OnlineMinistryArrearDetailsMergeData
    {
        public int ID { get; set; }
        public string? ZONE_CODE { get; set; }
        public string? ZONE_NAME { get; set; }
        public string? ZONE_NAMEBN { get; set; }
        public string? CITY_NAMEBN { get; set; }
        public string? POUR_NAMEBN { get; set; }
        public string? CITY_CODE { get; set; }
        public string? POUROSHOVA_CODE { get; set; }
        public string? CITY_COR_NAME { get; set; }
        public string? POUR_NAME { get; set; }
        public string? CURR_VAT { get; set; }
        public string? CURR_LPS { get; set; }
        public string? CURR_PRIN { get; set; }
        public string? VALID_DATE { get; set; }
        public double PREV_MONTH { get; set; }
        public double CURR_BILL { get; set; }
        public double PREV_ARR_AMOUNT { get; set; }
        public double RECEIPT_PRN { get; set; }
        public double RECEIPT_VAT { get; set; }
        public double CURR_MONTH_BILL_AMOUNT { get; set; }
        public double TOTAL_RECEIPT_AMOUNT { get; set; }
        public double CURR_TOTAL_ARR_AMOUNT { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_PRINCIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_PRICIPAL { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public string? NO_OF_CONSUMER { get; set; }
        public decimal TOTAL_RECEIPT_ARREAR { get; set; }
    }
}
