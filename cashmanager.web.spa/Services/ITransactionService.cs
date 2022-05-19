using System;
using cashmanager.web.spa.Models;

namespace cashmanager.web.spa.Services
{
    public interface ITransactionService
    {
        Task<List<GetTransactionModel>> GetAll();
        Task<GetTransactionModel> GetTransaction(Guid id);
        Task<List<GetTransactionModel>> GetTransactionByAccountId(Guid id, int? count);
        Task<GetTransactionModel> AddTransaction(AddTransactionModel model);
    }
}
