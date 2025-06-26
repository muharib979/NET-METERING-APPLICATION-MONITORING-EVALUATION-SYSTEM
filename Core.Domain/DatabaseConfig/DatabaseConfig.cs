using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.DatabaseConfig
{
    public class DatabaseConfig
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int ISACTIVE { get; set; }
        public string UPDATE_BY { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string HOST { get; set; }
        public string PORT { get; set; }
        public string SERVICE_NAME { get; set; }
        public string USER_ID { get; set; }
        public string PASSWORD { get; set; }
        public string DB_NAME_BN { get; set; }
        public string CODE { get; set; }
        public string DB_LINK { get; set; }
        public int ORDER_NO { get; set; }
        public int IS_JOB { get; set;}
    }
    public class DbMappingDto
    {
        public int ID { get; set; }
        public int USERID { get; set; }
        public string DB_CODE { get; set; }
        public int FULLACCESS { get; set; }
    }
}
