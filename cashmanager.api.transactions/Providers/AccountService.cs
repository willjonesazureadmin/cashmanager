using System;
using cashmanager.api.transactions.Interfaces;
using cashmanager.api.transactions.Db;
using AutoMapper;
using cashmanager.api.transactions.Models;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace cashmanager.api.transactions.Providers
{
    public class AccountService : IAccountService
    {
        private const string queueName = "transactions-accounts";
        private readonly IHttpClientFactory http;

        private readonly ServiceBusClient serviceBusClient;
        private readonly ILogger logger;

        private readonly IMapper mapper;

        public AccountService(IHttpClientFactory httpClient, ILogger<TransactionProvider> logger, IMapper mapper, ServiceBusClient serviceBusClient)
        {
            this.http = httpClient;
            this.logger = logger;
            this.mapper = mapper;
            this.serviceBusClient = serviceBusClient;
        }


        public async Task<(bool IsSuccess, Account? account, string? ErrorMessage)> GetAccountAsync(Guid Id)
        {
            try
            {
                var client = http.CreateClient("AccountService");
                logger.LogInformation($"Getting account id from account service {Id}");
                logger.LogInformation($"Querying {client.BaseAddress}");
                var response = await client.GetFromJsonAsync<Account>($"api/accounts/{Id}");
                if (response != null)
                {
                    return (true, response, "");
                }
                else
                {
                    logger.LogInformation($"Account id from account service {Id} not found");
                    return (false, null, "Not Found");
                }

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateAccountBalanceAsync(Models.GetTransactionModel transaction)
        {
            try
            {
                ServiceBusSender sender = serviceBusClient.CreateSender(queueName);
                logger.LogInformation($"Connecting to {serviceBusClient.FullyQualifiedNamespace}/{queueName}");
                var message = JsonConvert.SerializeObject(transaction);
                ServiceBusMessage servicebusmessage = new ServiceBusMessage(message);
                await sender.SendMessageAsync(servicebusmessage);  
                logger.LogInformation($"Message sent {servicebusmessage.MessageId}");    
                return (true, null);      
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, ex.Message);
            }


        }

    }
}
