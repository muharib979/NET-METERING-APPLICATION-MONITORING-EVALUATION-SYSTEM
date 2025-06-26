using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dbo
{
    public class UserAccessMenuDTO
    {
        public string UserName { get; set; }
        public int PageId { get; set; }
        public string MenuName { get; set; }
        public bool IsCreated { get; set; }
        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }
    }
}
