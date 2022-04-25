using System;
using cashmanager.api.transactions.Models;

namespace cashmanager.api.transactions.Interfaces
{
    public interface IAccountService
    {
        Task<(bool IsSuccess, Account? account, string? ErrorMessage)> GetAccountAsync(Guid Id);
        
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateAccountBalanceAsync(GetTransactionModel model);                
    }
}
