using System;

namespace cashmanager.api.transactions.Models
{
    public class AddTransactionModel
    {
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public Double Amount { get; set; }
        public string? TransactionFriendlyName { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public TransactionType TransactionType { get; set; }
    }

    
}
