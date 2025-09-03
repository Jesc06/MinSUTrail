using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities;
using RecordManagementSystem.Domain.Entities.Token;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class RefreshTokenServiceApp
    {
        private readonly IRefreshToken _refreshToken;
        public RefreshTokenServiceApp(IRefreshToken refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _refreshToken.AddAsync(refreshToken);
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await _refreshToken.GetByTokenAsync(token);
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            await _refreshToken.UpdateAsync(refreshToken);
        }


    }
}
