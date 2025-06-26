namespace ReligeousWorkerService.Models
{
    public class CustomerArrear
    {
        public int CUST_ID { get; set; }
        public string CUSTOMER_NAMEBN { get; set; }
        public int CUSTOMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATIONBN { get; set; }
        public int CON_EXTG_NUM { get; set; }
        public string NAMEBN { get; set; }
        public string PROVIDER_NAME { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal DUESTOTAL { get; set; }
        public string DIVISION_NAMEBN { get; set; }
        public int CONSUMER_NO { get; set; }
        public string CUSTOMERNAMEBN { get; set; }
        public decimal RECEIVE_PRN { get; set; }
        public decimal RECEIVE_LPS { get; set; }
        public decimal RECEIVE_VAT { get; set; }
        public decimal RECEIVE_TOTAL { get; set; }
        public decimal PRINCIPAL { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal CURR_LPS { get; set; }
        public DateTime CURR_DATE { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal CURR_BILL { get; set; }
        public decimal PREV_MONTH { get; set; }
        public decimal RECEIPT_AMT { get; set; }
    }
}
