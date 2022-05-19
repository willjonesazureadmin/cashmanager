using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using cashmanager.api.transactions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cashmanager.api.transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionProvider transactionsProvider;

        private readonly IAccountService AccountService;

        public TransactionsController(IAccountService AccountService, ITransactionProvider transactionProvider)
        {
            this.AccountService = AccountService;
            this.transactionsProvider = transactionProvider;
        }

        [HttpGet]
        public IActionResult GetTransactions([FromQuery] string? accountId = "")
        {
            try
            {
                if (accountId != "")                
                {
                    Console.WriteLine("Getting by Id");
                    var result = transactionsProvider.GetTransactionsByAccountId(Guid.Parse(accountId));
                    if (result.IsSuccess)
                    {
                        return Ok(result.transactions);
                    }
                }
                else
                {
                    var result = transactionsProvider.GetTransactions();
                    if (result.IsSuccess)
                    {
                        return Ok(result.transactions);
                    }
                }

                return NotFound();
            }
            catch (System.FormatException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // [HttpGet]
        // public IActionResult GetTransactions()
        // {
        //     try
        //     {

        //         var result = transactionsProvider.GetTransactions();
        //         if (result.IsSuccess)
        //         {
        //             return Ok(result.transactions);
        //         }


        //         return NotFound();
        //     }
        //     catch (System.FormatException)
        //     {
        //         return NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }

        // }

        [HttpPut]
        [HttpPost]
        public async Task<IActionResult> AddTransactions(Models.AddTransactionModel model)
        {
            try
            {
                var result = await transactionsProvider.AddTransactionAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result.result);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);

                }
            }
            catch (System.FormatException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Guid}")]
        public IActionResult GetTransaction(string Guid)
        {
            try
            {
                Guid Id = new Guid(Guid);
                var result = transactionsProvider.GetTransaction(Id);
                if (result.IsSuccess)
                {
                    return Ok(result.transaction);
                }
                return NotFound();

            }
            catch (System.FormatException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
