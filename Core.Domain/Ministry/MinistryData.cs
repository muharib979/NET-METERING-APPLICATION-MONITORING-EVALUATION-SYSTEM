namespace Core.Domain.Ministry
{
    public class MinistryData
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string NAMEBN { get; set; }
        public string CODE { get; set; }
        public int? HAS_DEPARTMENT { get; set; }

    }

    public class MinistryDetails
    {
        public string CUSTOMER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public int CUSTOMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_DSC { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string ZONE_NAME_BN { get; set; }
        //public decimal PRN { get; set; }
        //public decimal LPS { get; set; }
        //public decimal VAT { get; set; }
        //public decimal TOTAL { get; set; }
        public decimal PREV_ARREAR_AMT { get; set; }
        public decimal CURR_MONTH_BILL { get; set; }
        public decimal COLLECTION_AMOUNT { get; set; }
        public decimal TOTAL_ARREAR_AMOUNT { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
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

    public class GetCustomerArrearModel
    {
        public int CUST_ID { get; set; }
        public int CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_NAME_BN { get; set; }
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
        public decimal? PREV_MONTH { get; set; }
        public string LOCATION_DESC { get; set; }
        public int NOM { get; set; }
        public string ZONE_NAME { get; set; }
        public string ZONE_NAME_BN { get; set; }
        public string ZONE_CODE { get; set; }
        public string TOTAL_CUSTOMER { get; set; }
        public decimal TOTAL_MINISTRY_ARREAR { get; set; }
        public string? IS_GOVT_CUSTOMER { get; set; }


    }

    public class CustomerZoneModel
    {
        public int CUSTOMER_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_NAME_BN { get; set; }
        public string ZONE_NAME { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME_BN { get; set; }
        public string? DB_CODE { get; set; }    
    }
    public class AllCustomerMode
    {
        public List<GetCustomerArrearModel> MinistryCustomer { get; set; }
        public List<GetCustomerArrearModel> PrivateCustomer { get; set; }
    }
        
        
}
