namespace Core.Domain.UnionPorishad
{
    public class UnionPorishadCustIdModel
    {
        public int CUST_ID { get; set; }
        public string UNIONPARISHAD_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string UNIONPORISHAD_NAMEBN { get; set; }
        public string UNIONPORISHAD_NAME { get; set; }
        public string LOCATION_NAMEBN { get; set; }
    }

    public class OnlineUnionPorisadMergeDataDto
    {
        public string ConsumerNo { get; set; }
        public string Address { get; set; }
        public string UnionPorishadName { get; set; }
        public string CustomerName { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public decimal ArrearReceiptAmount { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal CurrReceiptAmt { get; set; }
        public decimal TotalArrearAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal CurrPrin { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }
        public string UnionPorishadNameBn { get; set; }
        public string UnionPorishadCode { get; set; }
        public decimal TotalReceiptPrincipal { get; set; }
        public decimal TotalReceiptVat { get; set; }
        public string ReceiptBillMonth { get; set; }
        public decimal TotalReceiptArrear { get; set; }
    }
}
