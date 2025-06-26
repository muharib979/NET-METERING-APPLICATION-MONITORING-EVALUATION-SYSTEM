namespace Shared.DTOs.ReplicationStatus
{
    public class ReplicationStatusDto
    {
        public DateTime? CardGenDate { get; set; }
        public DateTime? MrsEntryDate { get; set; }
        public DateTime? OverallProcDate { get; set; }
        public DateTime? OverallFinalDate { get; set; }
        public DateTime? BillGenDate { get; set; }
        public DateTime? BillFinalDate { get; set; }
        public DateTime? BillDespatchDate { get; set; }
        public string  DatabaseStatus { get; set; }
        public string  Database{ get; set; }
        public string  Color { get; set; }
    }
    public class ReplicationStatus
    {
        public DateTime? CARD_GEN_DATE { get; set; }
        public DateTime? MRS_ENTRY_DATE { get; set; }
        public DateTime? OVERALL_PROC_DATE { get; set; }
        public DateTime? OVERALL_FINAL_DATE { get; set; }
        public DateTime? BILL_GEN_DATE { get; set; }
        public DateTime? BILL_FINAL_DATE { get; set; }
        public DateTime? BILL_DESPATCH_DATE { get; set; }
        public string DATABASESTATUS { get; set; }
        public string DATABASE { get; set; }
        public string COLOR { get; set; }
    }

}
