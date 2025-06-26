using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ProsoftDataSync
{
    public class ProsoftUsers
    {
        public int ID { get; set; }
        public string? USER_NAME { get; set; }
        public string? FULL_NAME { get; set; }
        public string? EMAIL { get; set; }
        public string? PASSWORD { get; set; }
        public string? PASSWORD_SALT { get; set; }
        public string? TOKEN { get; set; }
        public DateTime? TOKEN_CREATEDATE { get; set; }
        public DateTime? TOKEN_MODIFYDATE { get; set; }
        public DateTime? TOKEN_EXPIRYDATE { get; set; }
        public int IS_ACTIVE { get; set; }//bool
        public string? ENTRY_BY { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public string? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
}
