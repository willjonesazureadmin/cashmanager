using System;

namespace cashmanager.api.accounts.Models
{
    public class GetAccountModel
    {
        public Guid Id { get; set; }

        public Guid UserOwnerId { get; set; }

        public double Balance { get; set; }
        public string? FriendlyName { get; set; }   

    }
}
