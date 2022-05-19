using System;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using cashmanager.api.accounts.Db;
using cashmanager.api.accounts.Interfaces;
using cashmanager.api.accounts.Models;
using Newtonsoft.Json;
using Account = cashmanager.api.accounts.Models;

namespace cashmanager.api.accounts.Providers
{
    public class AccountsProvider : IAccountsProvider
    {
        private readonly AccountsDbContext dbContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public AccountsProvider(AccountsDbContext dbContext, ILogger<AccountsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Accounts.Any())
            {
                dbContext.Accounts.Add(new Db.Account() { Id = Guid.Parse("e7add886-7d0c-47f0-bb66-8a5759dc15f6"), Balance = 20, UserOwnerId = Guid.NewGuid(), FriendlyName = "Seed Account" });
                dbContext.SaveChanges();
            }
        }

        public (bool IsSuccess, IEnumerable<GetAccountModel>? accounts, string? ErrorMessage) GetAccounts()
        {
            try
            {
                var accounts = dbContext.Accounts.ToList();
                if (accounts.Any() && accounts != null)
                {
                    var result = mapper.Map<List<Db.Account>, List<Models.GetAccountModel>>(accounts);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) GetAccount(Guid Id)
        {
            try
            {
                var account = dbContext.Accounts.FirstOrDefault(a => a.Id == Id);
                if (account != null)
                {
                    var result = mapper.Map<Db.Account, Models.GetAccountModel>(account);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) UpdateBalanceAsync(Guid Id, double amount, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var account = dbContext.Accounts.FirstOrDefault(a => a.Id == Id);
                    if (account != null)
                    {
                        account.Balance = account.Balance + amount;
                        dbContext.Update(account);
                        dbContext.SaveChanges();
                        var result = mapper.Map<Db.Account, Models.GetAccountModel>(account);
                        return (true, result, null);
                    }
                    return (false, null, "Not Found");
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex.ToString());
                    return (false, null, ex.Message);
                }
            }
            return (false, null, "fail");
        }

        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) AddAccount(AddAccountModel model)
        {
            try
            {
                var account = dbContext.Accounts.Add(mapper.Map<Models.AddAccountModel, Db.Account>(model));
                dbContext.SaveChanges();
                if (account != null)
                {
                    var result = mapper.Map<Db.Account, Models.GetAccountModel>(account.Entity);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public (bool IsSuccess, GetAccountModel? account, string? ErrorMessage) UpdateAccount(UpdateAccountModel model)
        {
            try
            {
                var account = dbContext.Accounts.FirstOrDefault(a => a.Id == model.Id);
                if (account != null)
                {
                    account.AccountNumber = model.AccountNumber;
                    account.FriendlyName = model.FriendlyName;
                    account.Provider = model.Provider;
                    account.SortCode = model.SortCode;
                    account.AccountNumber = model.SortCode;
                    dbContext.Update(account);
                    dbContext.SaveChanges();
                    var result = mapper.Map<Db.Account, Models.GetAccountModel>(account);

                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
