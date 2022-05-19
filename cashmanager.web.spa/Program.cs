using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using cashmanager.web.spa;
using cashmanager.web.spa.Helpers;
using cashmanager.web.spa.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var localConfigurations = builder.Configuration.Get<AppSettings.LocalConfigurations>();
builder.Services.AddSingleton(localConfigurations);

builder.Services.AddSingleton<IHistoryService, HistoryService>();
builder.Services.AddScoped<AccountServiceAuthorizationMessageHandler>();
builder.Services.AddScoped<TransactionServiceAuthorizationMessageHandler>();

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<IAccountService, AccountService>(localConfigurations.apis.AccountsAPI.ClientName, 
            client => client.BaseAddress = new Uri(localConfigurations.apis.AccountsAPI.BaseUrl))
                .AddHttpMessageHandler<AccountServiceAuthorizationMessageHandler>();


builder.Services.AddHttpClient<ITransactionService, TransactionService>(localConfigurations.apis.AccountsAPI.ClientName, 
            client => client.BaseAddress = new Uri(localConfigurations.apis.TransactionsAPI.BaseUrl))
                .AddHttpMessageHandler<TransactionServiceAuthorizationMessageHandler>();


builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(string.Format("{0}/{1}",localConfigurations.apis.AccountsAPI.ApplicationId, ".default"));
    foreach(var s in options.ProviderOptions.DefaultAccessTokenScopes)
    {
        Console.WriteLine(s);
        }
    // options.ProviderOptions.DefaultAccessTokenScopes.Remove("offline_access");
});


await builder.Build().RunAsync();
