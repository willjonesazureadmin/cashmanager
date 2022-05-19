using System;

namespace cashmanager.api.accounts.Models
{
    public class UpdateAccountModel
    {
        public Guid Id { get; set; }

        public string? FriendlyName { get; set; }   

        public string? Provider { get; set; }

        public string? AccountNumber { get; set; }

        public string? SortCode { get; set; }
    }
}
