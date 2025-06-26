namespace NonBengeliMisc.Models
{
    public class MinistryDataReponseModel
    {
        public int CUST_ID { get; set; }
        public int CONSUMER_NO { get; set; }
        public string ADDRESS { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string RUN_BILL_CYCLE_CODE { get; set; }
        public string BOOK_NO { get; set; }
        public string LOC_CODE { get; set; }
        public decimal INVOICE_NUM { get; set; }
        public decimal ENERGY_ARR { get; set; }

        public decimal CURRENT_VAT { get; set; }
        public decimal CURRENT_PRIN { get; set; }
        public decimal SURCHARGE_ARR { get; set; }
        public decimal CURRENT_LPS { get; set; }

        public int MINISTRY_ID { get; set; }
        public int DB_ID { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public string CONS_EXTG_NUM { get; set; }
        public decimal VAT_ARR { get; set; }
    }
}
