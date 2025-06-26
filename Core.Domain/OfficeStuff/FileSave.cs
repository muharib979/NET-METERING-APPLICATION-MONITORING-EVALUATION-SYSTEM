using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.OfficeStuff
{
    public class FileSave
    {
        public int ID { get; set; }
        public List<string>? FILE_NAME { get; set; }
        public List<string>? FILE_BASE_64 { get; set; }
    }
}
