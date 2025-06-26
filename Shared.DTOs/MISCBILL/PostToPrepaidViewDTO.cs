using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PostToPrepaidViewDTO
    {
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? PowerUtility { get; set; }
        public string? MaxPower { get; set; }
        public string? TransferDate { get; set; }
    }
}
