using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities.Token;
using RecordManagementSystem.Infrastructure.Migrations;
using RecordManagementSystem.Infrastructure.Persistence.Data;

namespace RecordManagementSystem.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshToken
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
             .FirstOrDefaultAsync(x => x.Token == token && !x.IsRevoked);
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }


    }
}
