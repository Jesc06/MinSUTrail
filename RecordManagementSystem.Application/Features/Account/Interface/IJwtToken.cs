using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;


namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IJwtToken
    {
        string GenerateAccessJwtToken(JwtApplicationUser user, IEnumerable<Claim>? additionalClaims = null);
        string GenerateRefreshJwtToken();
        string HashRefreshToken(string refreshJwtToken);
        bool VerfiyHashedJwtToken(string hashedToken, string refreshJwtToken);
        ClaimsPrincipal? GetPrincipalFromExpiredJwtToken(string JwtToken);
    }
}
