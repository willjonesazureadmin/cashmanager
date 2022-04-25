using System;
using cashmanager.api.accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cashmanager.api.accounts.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsProvider accountsProvider;
        private readonly ILogger logger;
        public AccountsController(IAccountsProvider accountsProvider, ILogger<AccountsController> logger)
        {
            this.accountsProvider = accountsProvider;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            try
            {
                logger.LogInformation("Get all accounts");
                var result = accountsProvider.GetAccounts();
                if (result.IsSuccess)
                {
                    return Ok(result.accounts);
                }
                logger.LogInformation("No accounts found");
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult AddAccount(Models.AddAccountModel model)
        {
            try
            {
                logger.LogInformation("Add new account");
                if (ModelState.IsValid)
                {
                    var result = accountsProvider.AddAccount(model);
                    if (result.IsSuccess)
                    {
                        return Ok(result.account);
                    }
                    else
                    {
                        return BadRequest(result.ErrorMessage);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{Guid}")]
        public IActionResult GetAccount(string Guid)
        {
            try
            {
                logger.LogInformation($"Get account by {Guid}");
                Guid Id = new Guid(Guid);
                var result = accountsProvider.GetAccount(Id);
                if (result.IsSuccess)
                {
                    return Ok(result.account);
                }
                return NotFound();

            }
            catch (System.FormatException ex)
            {
                logger.LogError(ex.ToString());
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
