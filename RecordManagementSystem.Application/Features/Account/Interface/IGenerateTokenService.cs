using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IGenerateTokenService
    {
        string GenerateToken(string username, string role);
    }
}
