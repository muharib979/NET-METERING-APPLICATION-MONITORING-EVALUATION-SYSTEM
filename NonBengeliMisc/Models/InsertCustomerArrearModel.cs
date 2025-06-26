namespace NonBengeliMisc.Models
{
    public class InsertCustomerArrearModel
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_DESC { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public string ADDRESS { get; set; }
        public string RECEIPT_BILLMONTH { get; set; }
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
        public string DB_CODE { get; set; }

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
    public class FinancialYearModel
    {
        public string? START_BILLCYCLE { get; set; }
        public string? END_BILLCYCLE { get; set; }

    }
}
