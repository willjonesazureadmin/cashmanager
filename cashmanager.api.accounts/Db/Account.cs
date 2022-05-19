using System;

namespace cashmanager.api.accounts.Db
{
    public class Account
    {
        public Guid Id { get; set; }

        public Guid UserOwnerId { get; set; }

        public double Balance { get; set; } = 0;

        public string? FriendlyName { get; set; }   

        public string? Provider { get; set; }

        public string? AccountNumber { get; set; }

        public string? SortCode { get; set; }

    }
}
