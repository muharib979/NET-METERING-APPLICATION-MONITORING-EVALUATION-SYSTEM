using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Domain.MinistryCustomer
{
    public class MinistryCustomers
    {
        public string CUSTOMER_NO { get; set; }
        public string? NAME { get; set; }
        public string ADDRESS { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string CITYCORPORATION_CODE { get; set; }
        public string POUROSHOVA_CODE { get; set; }
        public string UNIONPARISHAD_CODE { get; set; }
        public string RELIGIOUS_CODE { get; set; }
        public string NON_BENGALI_CAMP_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string DB_CODE { get; set; }
        public string? DISTRICT_CODE { get; set; }
        public string? DIVISION_CODE { get; set; }
        public string? UPAZILA_CODE { get; set; }
    }
}
