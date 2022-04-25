using System;
using Microsoft.EntityFrameworkCore;

namespace cashmanager.api.transactions.Db
{
    public class TransactionsDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;

        public TransactionsDbContext(DbContextOptions options) : base (options)
        {
            
        }
        
    }
}
