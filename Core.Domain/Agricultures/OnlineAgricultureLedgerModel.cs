using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Agricultures
{
    public class OnlineAgricultureLedgerModel
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string KRISHI_DEPT_NAME_BN { get; set; }
        public string KRISHI_DEPARTMRNT_CODE { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME_BN { get; set; }
        public int IS_KRISHI { get; set; }
        public int IS_POULTRY { get; set; }
        public string CON_EXTG_NUM { get; set; }
        public string LOCATION_NAME { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal ARREAR_PRICIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public decimal CURR_PRINCIPAL { get; set; }
        public decimal CURR_LPS { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal Total_Receipt_Arrear { get; set; }
        public decimal ARREAR_RECEIPT_PRINCIPAL { get; set; }
        public decimal ARREAR_RECEIPT_VAT { get; set; }
    }
}
