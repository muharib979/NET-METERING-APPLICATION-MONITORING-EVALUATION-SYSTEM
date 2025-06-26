using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ministry
{
    public class PublicSecurityDivisionDetails
    {
        public int ID { get; set; }
        public int CUST_ID { get; set; }
        public int CONSUMER_NO { get; set; }
        public string? ADDRESS { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_DESC { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_NAME_BN { get; set; }
        public string? ZONE_CODE { get; set; }
        public string? ZONE_NAME { get; set; }
        public string ZONE_NAME_BN { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal CURR_LPS { get; set; }
        public string CURR_PRIN { get; set; }
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
        public decimal RECEIPT_BILLMONTH { get; set; }
        public decimal ARREAR_RECEIPT_PRINCIPAL { get; set; }
        public decimal ARREAR_RECEIPT_VAT { get; set; }
        public int HAS_DEPARTMENT { get; set; }
        public string DEPT_NAME { get; set; }
    }
}
