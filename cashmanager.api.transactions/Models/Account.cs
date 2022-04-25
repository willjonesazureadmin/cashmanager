using System;

namespace cashmanager.api.transactions.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public Guid UserOwnerId { get; set; }

        public double Balance { get; set; }
    }
}
