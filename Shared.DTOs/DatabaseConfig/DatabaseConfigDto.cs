using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.DatabaseConfig
{
    public class DatabaseConfigDto
    {
        public int Id { get; set; }
        public string DbName { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int OrderCol { get; set; }
        public string DbNameBn { get; set; }
    }
}
