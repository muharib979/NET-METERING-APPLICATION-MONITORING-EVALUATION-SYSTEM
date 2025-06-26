using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProsoftDataSync
{
    public class ProsoftTokenRequestDto
    {
        public string UserName { get; set; }
        //public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}
