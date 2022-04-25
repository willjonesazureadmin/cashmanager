using System;
using System.Linq;
using AutoMapper;
using cashmanager.api.accounts.Db;
using cashmanager.api.accounts.Profiles;
using cashmanager.api.accounts.Providers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace cashmanager.api.accounts.tests;

public class AccountsProviderTest
{
    private AccountsProvider AccountsProvider;
    private Guid randomId;
    
    public AccountsProviderTest()
    {
        var options = new DbContextOptionsBuilder<AccountsDbContext>().UseInMemoryDatabase(nameof(GetAccountsReturnsAllAccounts)).Options;
        var dbContext = new AccountsDbContext(options);
        SeedData(dbContext);
        var accountProfile = new AccountProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(accountProfile));
        var mapper = new Mapper(configuration);
        this.AccountsProvider = new AccountsProvider(dbContext, null, mapper);
    }
    [Fact]
    public void GetAccountsReturnsAllAccounts()
    {
       
        var account = this.AccountsProvider.GetAccounts();
        Assert.True(account.IsSuccess);
        Assert.True(account.accounts.Any());
    }

    [Fact]
    public void GetAccountReturnsAccountWithValidId()
    {
       
        var account = this.AccountsProvider.GetAccount(this.randomId);
        Assert.True(account.IsSuccess);
        Assert.NotNull(account.account);
        Assert.True(account.account.Id == this.randomId);
        Assert.Null(account.ErrorMessage);
    }

    [Fact]
    public void GetAccountReturnsAccountWithIncorrectId()
    {
       
        var account = this.AccountsProvider.GetAccount(Guid.Parse("e7add886-7d0c-47f0-bb66-8a5759dc15f7"));
        Assert.False(account.IsSuccess);
        Assert.Null(account.account);
        Assert.NotNull(account.ErrorMessage);
    }


    private void SeedData(AccountsDbContext dbContext)
    {
        this.randomId = Guid.NewGuid();
        if(!dbContext.Accounts.Any())
        {
            Random rand = new Random();
            dbContext.Accounts.Add(new Db.Account() { Id = randomId, Balance = 20, UserOwnerId = Guid.NewGuid(), FriendlyName = "Seed Account0" });
            dbContext.Accounts.Add(new Db.Account() { Id = Guid.NewGuid(), Balance = rand.NextDouble(), UserOwnerId = Guid.NewGuid(), FriendlyName = "Seed Account1" });
            dbContext.Accounts.Add(new Db.Account() { Id = Guid.NewGuid(), Balance = rand.NextDouble(), UserOwnerId = Guid.NewGuid(), FriendlyName = "Seed Account2" });
            dbContext.Accounts.Add(new Db.Account() { Id = Guid.NewGuid(), Balance = rand.NextDouble(), UserOwnerId = Guid.NewGuid(), FriendlyName = "Seed Account3" });
            dbContext.SaveChanges();
        }

    }
}