using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Services.AccountService;
using WebApplication1.Services.TransactionService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public BankController(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpGet("user/{userId}/accounts")]
        public async Task<IActionResult> GetAccountsByUserId(string userId)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("accounts/{accountId}")]
        public async Task<IActionResult> GetAccountById(string accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(new { account, transactions });
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> CreateAccount(AccountDto accountDto)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdAccount.Id }, createdAccount);
        }

        [HttpPut("accounts/{accountId}")]
        public async Task<IActionResult> UpdateAccount(string accountId, AccountDto accountDto)
        {
            if (accountId != accountDto.Id)
            {
                return BadRequest();
            }

            await _accountService.UpdateAccountAsync(accountId, accountDto);
            return NoContent();
        }

        [HttpDelete("accounts/{accountId}")]
        public async Task<IActionResult> DeleteAccount(string accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
            return NoContent();
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction(TransactionDto transactionDto)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdTransaction.SenderBankId }, createdTransaction);
        }

        [HttpGet("accounts/{accountId}/transactions")]
        public async Task<IActionResult> GetTransactionsByAccountId(string accountId)
        {
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(transactions);
        }
    }
}
