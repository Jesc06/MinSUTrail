using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecordManagementSystem.Domain;
using RecordManagementSystem.Domain.Entities.Account;
using RecordManagementSystem.Domain.Entities.Token;

namespace RecordManagementSystem.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserIdentity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<StudentUserAccount> studentUserAccount { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
