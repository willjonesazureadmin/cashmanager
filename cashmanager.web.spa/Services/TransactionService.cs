using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using cashmanager.web.spa.Models;
using Microsoft.AspNetCore.Components;
using static cashmanager.web.spa.Helpers.AppSettings;

namespace cashmanager.web.spa.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _http;

        private readonly LocalConfigurations.API? _settings;

        private readonly IAccessTokenProvider _tokenProvider;


        private NavigationManager _navManager;

        public TransactionService(HttpClient httpClient, NavigationManager navigationManager, LocalConfigurations settings, IAccessTokenProvider TokenProvider)
        {
            this._navManager = navigationManager;
            this._settings = settings.apis.TransactionsAPI;
            this._http = httpClient;
            this._tokenProvider = TokenProvider;
        }

        public async Task<List<GetTransactionModel>> GetAll()
        {
            try
            {
                var dataRequest = await _http.GetAsync(String.Format("{0}/api/transactions", _settings.BaseUrl));
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<List<GetTransactionModel>>();

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

        public async Task<GetTransactionModel> GetTransaction(Guid Id)
        {
            try
            {
                var dataRequest = await _http.GetAsync(String.Format("{0}/api/transactions/{1}", _settings.BaseUrl, Id));
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<GetTransactionModel>();

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
        public async Task<List<GetTransactionModel>> GetTransactionByAccountId(Guid accountId, int? count)
        {
            try
            {
                string url;
                if (count != null)
                {
                    url = String.Format("{0}/api/transactions?AccountId={1}&Limit={2}", _settings.BaseUrl, accountId, count);
                }
                else
                {
                    url = String.Format("{0}/api/transactions?AccountId={1}", _settings.BaseUrl, accountId);
                }
                var dataRequest = await _http.GetAsync(url);
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<List<GetTransactionModel>>();

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

        public async Task<GetTransactionModel> AddTransaction(AddTransactionModel model)
        {
            try
            {
                var dataRequest = await _http.PostAsJsonAsync<AddTransactionModel>(String.Format("{0}/api/transactions", _settings.BaseUrl), model);
                if (!dataRequest.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Reason: {dataRequest.ReasonPhrase}"); //or add a custom logic here
                }
                return await dataRequest.Content.ReadFromJsonAsync<GetTransactionModel>();

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
