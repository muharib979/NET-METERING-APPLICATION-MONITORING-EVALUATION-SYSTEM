using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MinistryCustomer
{
    public class MinistryCustomerDTOs
    {
        public int? CustomerNo { get; set; }
        public string? CustomerNameBN { get; set; }
        public string? CustomerNameEN { get; set; }
        public string? DivisionCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? UpazilaCode { get; set; }

        public string? MinistryCode { get; set; }
        public string? DepartmentCode { get; set; }
        public string? CitycorporationCode { get; set; }
        public string? PouroshovaCode { get; set; }
        public string? UnionParishadCode { get; set; }

        public string? ReligiousCode { get; set; }
        public string? NonBengaliCampCode { get; set; }
        public string? IsPolice { get; set; }

        public string? ZoneCode { get; set; }
        public string? CircleCode { get; set; }
        public string? DbCode { get; set; }
        public string? LocationCode { get; set; }
        public string? UserName { get; set; }
    }

    public class CustomerNameDTO
    {
        public int? CUST_ID { get; set; }
        public string? CUSTOMER_NAME { get; set; }
        public string? ADDRESS { get; set; }
    }

    public class CustomerZoneCircleDTO
    {
        public string? ZONE_CODE { get; set; }
        public string? CIRCLE_CODE { get; set; }
        public string? LOCATION_CODE { get; set; }
        public string? DB_CODE { get; set; }
    }
}
