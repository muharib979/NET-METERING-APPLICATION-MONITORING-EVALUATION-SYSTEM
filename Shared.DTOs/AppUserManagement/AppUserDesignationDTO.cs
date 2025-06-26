using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AppUserManagement
{
    public class AppUserDesignationDTO
    {
        public int? DesignationId { get; set; }

        public string? Name { get; set; }
        public string? DesignationCode { get; set; }
        public string? CreatedBy { get; set; }

        public string? CreatedDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateDate { get; set; }
    }
}
