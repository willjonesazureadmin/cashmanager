using System;

namespace cashmanager.api.transactions.Db
{
    public class Transaction
    {
 public Guid Id { get; set; }


        public String? TransactionFriendlyName { get; set; }

        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }

        public Double Amount { get; set; }

        public DateTime TransactionDateTime { get; set; } = DateTime.Now;

        public TransactionType TransactionType { get; set; }

        public TransactionState TransactionState { get; set; } = TransactionState.Pending;
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
