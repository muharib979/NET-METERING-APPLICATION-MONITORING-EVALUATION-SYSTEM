namespace AdminSystem.Job.Models
{
    public class InsertCustomerArrearModel
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal RECEIVE_PRN { get; set; }
        public decimal RECEIVE_LPS { get; set; }
        public decimal RECEIVE_VAT { get; set; }
        public decimal RECEIVE_TOTAL { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal ARREAR_AMT { get; set; }
        public decimal RECEIPT_AMT { get; set; }
        public decimal CURR_BILL { get; set; }
        public decimal PREV_MONTH { get; set; }

    }
    public class CustomerModel
    {
        public int? CUSTOMER_NO { get; set; }
    }

    public class CustIdInsertModel
    {
        public int? CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }

    }
}
