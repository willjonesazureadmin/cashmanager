using AutoMapper;
using cashmanager.api.accounts.Db;
using cashmanager.api.accounts.Interfaces;
using cashmanager.api.accounts.Profiles;
using cashmanager.api.accounts.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;



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
builder.Services.AddScoped<IAccountsProvider, AccountsProvider>();
builder.Services.AddHostedService<TransactionService>();
builder.Services.AddCors(o => 
			o.AddDefaultPolicy(b => 
				b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<AccountsDbContext>(options =>
{
    options.UseInMemoryDatabase("AccountsDb");
});

 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(builder.Configuration, $"{builder.Environment.ApplicationName}:AzureAd");     

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{


}
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
