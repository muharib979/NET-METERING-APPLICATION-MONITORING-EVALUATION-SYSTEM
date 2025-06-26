namespace Core.Domain.Report
{
    public class BillGroupDto
    {
        public string BillGroup { get; set; }
        public string DbName { get; set; }
    }

    public class BillGroupDropDownDto
    {
        public string[] DbCodes { get; set; }
        public string []? LocationCodes { get; set; }
        public string? BillGroupId { get; set; }
    }
}
