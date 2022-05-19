using System;

namespace cashmanager.web.spa.Models
{
    public class GetTransactionModel
    {
        public Guid Id { get; set; }

        public String? TransactionFriendlyName { get; set; }

        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }

        public Double Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionState TransactionState { get; set; }
    }
    public enum TransactionType
    {
        Credit,
        Debit
    }

    public enum TransactionState
    {
        Pending,
        Failed,
        Confirmed
    }
}
