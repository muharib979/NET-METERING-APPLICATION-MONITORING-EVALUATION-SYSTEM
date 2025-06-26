namespace AdminSystem.Job.Models
{
    public class KrishiCustomerResponseModel
    {
        public IEnumerable<object> KrishiLocations { get; set; }
        public List<dynamic> KrishiCustomers { get; set; }
        public string TotalDues { get; set; }

    }
}
