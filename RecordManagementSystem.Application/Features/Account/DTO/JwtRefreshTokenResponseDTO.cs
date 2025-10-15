using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.DTO
{
    public class JwtRefreshTokenResponseDTO
    {
        public string newAccessToken { get; set; }
        public string newRefreshToken { get; set; }
    }
}
