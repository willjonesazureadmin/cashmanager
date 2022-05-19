using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using static cashmanager.web.spa.Helpers.AppSettings;

public class AccountServiceAuthorizationMessageHandler : AuthorizationMessageHandler
{

    private readonly LocalConfigurations _settings;

    public AccountServiceAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, LocalConfigurations settings) : base(provider, navigationManager)
    {

        this._settings = settings;
        ConfigureHandler(
            authorizedUrls: new[] { _settings.apis.AccountsAPI.BaseUrl  },
            scopes: new[] { String.Format("{0}/{1}", _settings.apis.AccountsAPI.ApplicationId, _settings.apis.AccountsAPI.Scopes[0])}
            );
    }
}