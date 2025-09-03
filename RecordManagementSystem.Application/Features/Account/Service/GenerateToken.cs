using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class GenerateToken
    {
        private readonly IGenerateTokenService _generateGenerateToken;
        public GenerateToken(IGenerateTokenService generateGenerateToken)
        {
            _generateGenerateToken = generateGenerateToken;
        }

        public TokenResponseDTO Token(string token, string role) 
        { 
            return _generateGenerateToken.GenerateToken(token, role);
        }


    }
}
