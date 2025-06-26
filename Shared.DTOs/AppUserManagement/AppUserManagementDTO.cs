using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AppUserManagement
{
    public class AppUserManagementDTO
    {
        public int? UserId { get; set; }
        public string?   UserName{ get; set; }
        public string?   UserPassword{ get; set; }
        public string? Email { get; set; }
        public string? MobileNo{ get; set; }
        public string? UserCode { get; set; }
        public string? DesignationCode { get; set; }
        public string?Db { get; set; }
        public string?Location { get; set; }

    }
}
