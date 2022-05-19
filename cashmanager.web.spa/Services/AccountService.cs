using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using cashmanager.web.spa.Models;
using Microsoft.AspNetCore.Components;
using static cashmanager.web.spa.Helpers.AppSettings;

namespace cashmanager.web.spa.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _http;

        private readonly LocalConfigurations.API? _settings;

        private readonly IAccessTokenProvider _tokenProvider;


        private NavigationManager _navManager;

        public AccountService(HttpClient httpClient, NavigationManager navigationManager, LocalConfigurations settings,  IAccessTokenProvider TokenProvider)
        {
            this._navManager = navigationManager;
            this._settings = settings.apis.AccountsAPI;
            this._http = httpClient;
            this._tokenProvider = TokenProvider;
        }

        public async Task<List<GetAccountModel>> GetAccounts()
        {
            try
            {
                var dataRequest = await _http.GetAsync(String.Format("{0}/api/accounts", _settings.BaseUrl));
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<List<GetAccountModel>>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
                
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here

            }

            catch (Exception exception)
            {
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here
            }
        }

       public async Task<GetAccountModel> GetAccount(Guid Id)
        {
            try
            {
                var dataRequest = await _http.GetAsync(String.Format("{0}/api/accounts/{1}", _settings.BaseUrl, Id));
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<GetAccountModel>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
                
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here

            }

            catch (Exception exception)
            {
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here
            }
        }

        public async Task<GetAccountModel> AddAccount(AddAccountModel model)
        {
            try
            {
                var dataRequest = await _http.PostAsJsonAsync<AddAccountModel>(String.Format("{0}/api/accounts", _settings.BaseUrl), model);
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<GetAccountModel>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
                
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here

            }

            catch (Exception exception)
            {
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here
            }
        }
    
        public async Task<UpdateAccountModel> UpdateAccount(UpdateAccountModel model)
        {
            try
            {
                var dataRequest = await _http.PutAsJsonAsync<UpdateAccountModel>(String.Format("{0}/api/accounts", _settings.BaseUrl), model);
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<UpdateAccountModel>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
                
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here

            }

            catch (Exception exception)
            {
                throw new ApplicationException($"Reason: {exception.Message}"); //or add a custom logic here
            }
        }
    }
}
