using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Dbo.UserAddByCenterLocation
{
    public class UserUpdateByCenterLocationCommand : IRequest<Response<IActionResult>>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string UserPassword { get; set; }
        public List<int> Location { get; set; }
        public List<int> DB { get; set; }
        public int RoleId { get; set; }
        public string? UpdateBy { get; set; }
    }
}
