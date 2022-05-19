using System;
using cashmanager.web.spa.Models;

namespace cashmanager.web.spa.Services
{
    public interface IAccountService
    {
        Task<List<GetAccountModel>> GetAccounts();
        Task<GetAccountModel> GetAccount(Guid id);
        Task<GetAccountModel> AddAccount(AddAccountModel model);
        Task<UpdateAccountModel> UpdateAccount(UpdateAccountModel model);
    }
}
