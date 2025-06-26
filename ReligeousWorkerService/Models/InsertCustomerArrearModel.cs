namespace ReligeousWorkerService.Models
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
        public string LOCATION_DESC { get; set; }
        public decimal ARREAR_PRICIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public decimal CURR_PRINCIPAL { get; set; }
        public decimal CURR_LPS { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal TOTAL_PRINCIPAL_ARREAR { get; set; }
        public decimal TOTAL_LPS_ARREAR { get; set; }
        public decimal TOTAL_VAT_ARREAR { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal TOTAL_RECEIPT_PRINCIPAL { get; set; }
        public decimal TOTAL_RECEIPT_VAT { get; set; }
        public decimal ARREAR_RECEIPT_AMOUNT { get; set; }
        public decimal FINANCIAL_AMOUNT { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }


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
