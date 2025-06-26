using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class ImposedByDTO
    {
        public int ImposedById { get; set; }
        public string? ImposedByCode { get; set; }
        public string? ImposedByDesc { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
