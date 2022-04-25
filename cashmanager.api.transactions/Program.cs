using cashmanager.api.transactions.Db;
using cashmanager.api.transactions.Interfaces;
using cashmanager.api.transactions.Providers;
using Microsoft.EntityFrameworkCore;
using cashmanager.api.transactions.Profiles;
using AutoMapper;
using Microsoft.Extensions.Azure;
using Polly;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

//Add Azure App Configuration Connection String
var connectionString = builder.Configuration.GetConnectionString("AppConfig");

//Load Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString).Select($"{builder.Environment.ApplicationName}:*",builder.Environment.EnvironmentName);

});
builder.Services.AddAzureAppConfiguration();
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients( sbbuilder => {
    sbbuilder.AddServiceBusClient(builder.Configuration[$"{builder.Environment.ApplicationName}:ServiceBus:ConnectionString"]);
});

builder.Services.AddHttpClient("AccountService", config => {
    config.BaseAddress = new Uri(builder.Configuration[$"{builder.Environment.ApplicationName}:servicelocator:accountsbaseurl"]);
}).AddTransientHttpErrorPolicy( p=> p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
builder.Services.AddScoped<ITransactionProvider, TransactionProvider>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<TransactionsDbContext>(options =>
{
    options.UseInMemoryDatabase("TransactionsDb");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
