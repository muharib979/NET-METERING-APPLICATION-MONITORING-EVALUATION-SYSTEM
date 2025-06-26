namespace Shared.DTOs.MISReport
{
    public class UntraceableCustomerDTO
    {
        public string NAME { get; set; }
        public string ADDR { get; set; }
        public string CON_NO { get; set; }
        public string PV_AC { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
        public string STATUS { get; set; }
        public string METER_STATUS { get; set; }
        public string TEMP_DISCON_DATE { get; set; }
        public string PERM_DISCON_DATE { get; set; }
        public string DISC_DATE { get; set; }
        public int CUST_ID { get; set; }
        public string LAST_BILL_MONTH { get; set; }
    }


    public class MergeUntraceableDto
    {
        public string Name { get; set; }
        public string Addr { get; set; }
        public string ConNo { get; set; }
        public string AccountNo { get; set; }
        public decimal TatalArrear { get; set; }
        public string Status { get; set; }
        public string MeterStatus { get; set; }
        public string TemDisDate { get; set; }
        public string PermDisconDate { get; set; }
        public string DisDate { get; set; }
        public int CustId { get; set; }
        public string LastBillMonth { get; set; }
        public string UcType { get; set; }

    }
}
