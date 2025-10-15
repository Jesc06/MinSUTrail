using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.DTO
{
    public class GenerateJwtTokenResponseDTO
    {
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; }
    }
}
