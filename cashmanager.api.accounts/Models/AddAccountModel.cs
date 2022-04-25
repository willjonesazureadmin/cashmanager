using System;

namespace cashmanager.api.accounts.Models
{
    public class AddAccountModel
    {
        public Guid UserOwnerId { get; set; }

        public double Balance { get; set; }

        public string? FriendlyName { get; set; }   
    }
}
