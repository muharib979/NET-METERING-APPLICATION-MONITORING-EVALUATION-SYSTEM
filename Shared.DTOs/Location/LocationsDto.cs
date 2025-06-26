using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Location
{
    public class LocationsDto
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameBn { get; set; }
        public string Code { get; set; }
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string DbCode { get; set; }
        public string DeptCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
    }
    public class ZoneCircleLocationDTO
    {
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string ZoneName { get; set; }
        public string ZoneCode { get; set; }
        public string CircleName { get; set; }
        public string CircleCode { get; set; }
    }
}
