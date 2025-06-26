using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OffiecStuff
{
    public class OfficeStuffDto
    {
        public int Id { get; set; }
        public string? OfficeStuffName { get; set; }
        public string? Designation { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int IsActive { get; set; } //bool

    }
}
