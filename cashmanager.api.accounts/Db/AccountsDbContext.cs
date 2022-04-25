using System;
using Microsoft.EntityFrameworkCore;

namespace cashmanager.api.accounts.Db
{
    public class AccountsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;

        public AccountsDbContext(DbContextOptions options) : base (options)
        {
            
        }
        
    }
}
