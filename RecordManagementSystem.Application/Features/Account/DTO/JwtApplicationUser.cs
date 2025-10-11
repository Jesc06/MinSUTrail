using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.DTO
{
    public class JwtTokenDTO
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}   
