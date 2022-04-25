using System;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using cashmanager.api.accounts.Db;
using cashmanager.api.accounts.Interfaces;
using cashmanager.api.accounts.Models;
using Newtonsoft.Json;

namespace cashmanager.api.accounts.Providers
{
    public class TransactionService : BackgroundService
    {
        private const string queueName = "transactions-accounts";
        private readonly ILogger logger;
        private readonly ServiceBusProcessor serviceBusProcessor;
        public IServiceProvider serviceProvider { get; }

        public TransactionService(ILogger<TransactionService> logger, IMapper mapper, ServiceBusClient serviceBusClient, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            var serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };
            this.serviceProvider = serviceProvider;
            this.serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);

        }

        public override void Dispose()
        {
            if (serviceBusProcessor != null)
            {
                serviceBusProcessor.DisposeAsync().ConfigureAwait(false);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Configure processor
            serviceBusProcessor.ProcessMessageAsync += ProcessMessagesAsync;
            serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;

            // Start receiving messages
            serviceBusProcessor.StartProcessingAsync();

            return Task.CompletedTask;
        }



        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            try
            {
                // the error source tells me at what point in the processing an error occurred
                Console.WriteLine(args.ErrorSource);
                // the fully qualified namespace is available
                Console.WriteLine(args.FullyQualifiedNamespace);
                // as well as the entity path
                Console.WriteLine(args.EntityPath);
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }
            catch
            {
                throw new NotImplementedException();

            }
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            try
            {
                GetTransactionModel transaction = JsonConvert.DeserializeObject<GetTransactionModel>(args.Message.Body.ToString());
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                            .GetRequiredService<IAccountsProvider>();

                    scopedProcessingService.UpdateBalanceAsync(transaction.AccountId, transaction.Amount, args.CancellationToken);
                }

                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
