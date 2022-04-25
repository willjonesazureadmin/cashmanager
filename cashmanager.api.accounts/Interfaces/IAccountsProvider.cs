using System;
using cashmanager.api.accounts.Models;

namespace cashmanager.api.accounts.Interfaces
{
    public interface IAccountsProvider
    {
        public (bool IsSuccess, IEnumerable<GetAccountModel>? accounts, string? ErrorMessage) GetAccounts();
        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) GetAccount(Guid Id);
        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) UpdateBalanceAsync(Guid Id, double amount, CancellationToken cancellationToken);
        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) AddAccount(AddAccountModel model);
    }
}
