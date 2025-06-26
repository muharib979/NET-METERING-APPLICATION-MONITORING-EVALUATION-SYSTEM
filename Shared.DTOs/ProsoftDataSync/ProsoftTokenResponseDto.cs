using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProsoftDataSync
{
    public class ProsoftTokenResponseDto
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
