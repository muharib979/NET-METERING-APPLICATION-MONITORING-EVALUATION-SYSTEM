using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.OfficeStuff
{
    public class OfficeStuff
    {
        public int  ID{ get; set; }
        public string? OFFICE_STUFF_NAME{ get; set; }
        public string? DESIGNATION { get; set; }
        public string? PHONE { get; set; }
        public string? EMAIL { get; set; }
        public int IS_ACTIVE { get; set; } //bool
        //public string? col_entry_by { get; set; }
        //public DateTime? col_entry_date { get; set; }
        //public string? col_updated_by { get; set; }
        //public DateTime? col_updated_date { get; set; }
    }
}
