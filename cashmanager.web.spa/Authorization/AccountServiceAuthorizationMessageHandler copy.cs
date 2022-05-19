using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using static cashmanager.web.spa.Helpers.AppSettings;

public class TransactionServiceAuthorizationMessageHandler : AuthorizationMessageHandler
{

    private readonly LocalConfigurations _settings;

    public TransactionServiceAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, LocalConfigurations settings) : base(provider, navigationManager)
    {

        this._settings = settings;
        ConfigureHandler(
            authorizedUrls: new[] { _settings.apis.TransactionsAPI.BaseUrl  },
            scopes: new[] { String.Format("{0}/{1}", _settings.apis.TransactionsAPI.ApplicationId, _settings.apis.TransactionsAPI.Scopes[0])}
            );
    }
}