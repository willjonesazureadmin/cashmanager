using System;
using cashmanager.api.transactions.Models;

namespace cashmanager.api.transactions.Interfaces
{
    public interface ITransactionProvider
    {
        public (bool IsSuccess, IEnumerable<GetTransactionModel>? transactions, string? ErrorMessage) GetTransactions();
        public (bool IsSuccess, IEnumerable<Models.GetTransactionModel>? transactions, string? ErrorMessage) GetTransactionsByAccountId(Guid AccountId);
        public (bool IsSuccess, GetTransactionModel? transaction, string? ErrorMessage) GetTransaction(Guid Id);
        Task<(bool IsSuccess, Models.GetTransactionModel? result, string? ErrorMessage)> AddTransactionAsync(Models.AddTransactionModel transaction);
    }
}
