using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.DTO
{
    public class TokenResponseDTO
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string Role { get; set; }

    }
}
