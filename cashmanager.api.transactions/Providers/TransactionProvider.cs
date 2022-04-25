using System;
using cashmanager.api.transactions.Interfaces;
using cashmanager.api.transactions.Db;
using AutoMapper;

namespace cashmanager.api.transactions.Providers
{
    public class TransactionProvider : ITransactionProvider
    {
        private readonly TransactionsDbContext dbContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        private readonly IAccountService AccountService;

        public TransactionProvider(TransactionsDbContext dbContext, ILogger<TransactionProvider> logger, IMapper mapper, IAccountService AccountService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.AccountService = AccountService;

            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Transactions.Any())
            {
                logger.LogInformation($"Seeding data to database");
                dbContext.Transactions.Add(new Db.Transaction() { 
                    AccountId = Guid.Parse("e7add886-7d0c-47f0-bb66-8a5759dc15f6"),
                    Amount = 20.00,
                    TransactionFriendlyName = "Seed Transaction",
                    TransactionDateTime = DateTime.Now,
                    Id = Guid.NewGuid()                   
                });
                dbContext.SaveChanges();
                logger.LogInformation($"Seed Complete");

            }
        }

        public (bool IsSuccess, IEnumerable<Models.GetTransactionModel>? transactions, string? ErrorMessage) GetTransactions()
        {
            try
            {
                logger.LogInformation($"Get All Transactions");
                var Transactions = dbContext.Transactions.ToList();
                if (Transactions.Any() && Transactions != null)
                {
                    var result = mapper.Map<IEnumerable<Db.Transaction>, IEnumerable<Models.GetTransactionModel>>(Transactions);
                    return (true, result, null);
                }
                logger.LogInformation($"No Transaction Found");
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public (bool IsSuccess, Models.GetTransactionModel? transaction, string? ErrorMessage) GetTransaction(Guid Id)
        {
            try
            {
                logger.LogInformation($"Get Transaction by {Id}");
                var Transaction = dbContext.Transactions.FirstOrDefault(a => a.Id == Id);
                if (Transaction != null)
                {
                    var result = mapper.Map<Db.Transaction, Models.GetTransactionModel>(Transaction);
                    return (true, result, null);
                }
                logger.LogInformation($"Transaction not found by {Id}");
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.GetTransactionModel? result, string? ErrorMessage)> AddTransactionAsync(Models.AddTransactionModel transaction)
        {
            try
            {
                logger.LogInformation($"Query for Account by {transaction.AccountId}");
                var accResult = await AccountService.GetAccountAsync(transaction.AccountId);
                if (accResult.account != null)
                {
                    var mappedobject = mapper.Map<Models.AddTransactionModel, Db.Transaction>(transaction);

                    var result = dbContext.Add(mappedobject);
                    dbContext.SaveChanges();
                    var mappedobject2 = mapper.Map<Db.Transaction, Models.GetTransactionModel>(result.Entity);
                    var sbresult = await AccountService.UpdateAccountBalanceAsync(mappedobject2);
                    if(sbresult.IsSuccess)
                    {
                        result.Entity.TransactionState = TransactionState.Pending;
                        dbContext.SaveChanges();
                        return (true, mappedobject2, null);
                    }
                    else
                    {
                        result.Entity.TransactionState = TransactionState.Failed;
                        dbContext.SaveChanges();
                        return (false, null, $"Transaction Failed");
                    }


                }
                logger.LogInformation($"Account {transaction.AccountId} not found");
                return (false, null, $"Account {transaction.AccountId} not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
