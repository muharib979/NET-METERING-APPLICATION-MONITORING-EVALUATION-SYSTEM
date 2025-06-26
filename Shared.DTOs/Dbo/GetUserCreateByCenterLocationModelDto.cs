using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dbo
{
    public class GetUserCreateByCenterLocationModelDto
    {
            public int RoleId { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int FullAccess { get; set; }
            public string DbCode { get; set; }
            public string Status { get; set; }
            public int LocationId { get; set; }
    }
}
