using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;

namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IGenerateTokenService
    {
        TokenResponseDTO GenerateToken(string username, string role);
    }
}
