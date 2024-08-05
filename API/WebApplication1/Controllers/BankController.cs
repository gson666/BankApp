using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(new { account, transactions });
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> CreateAccount(AccountDto accountDto)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdAccount.AccountId }, createdAccount);
        }

        [HttpPut("accounts/{accountId}")]
        public async Task<IActionResult> UpdateAccount(int accountId, AccountDto accountDto)
        {
            if (accountId != accountDto.AccountId)
            {
                return BadRequest();
            }

            await _accountService.UpdateAccountAsync(accountId, accountDto);
            return NoContent();
        }

        [HttpDelete("accounts/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
            return NoContent();
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction(TransactionDto transactionDto)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdTransaction.SenderAccountId }, createdTransaction);
        }

        [HttpGet("accounts/{accountId}/transactions")]
        public async Task<IActionResult> GetTransactionsByAccountId(int accountId)
        {
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(transactions);
        }
        [Authorize]
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferMoney(TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transaction = await _transactionService.TransferMoneyAsync(
                    transactionDto.SenderAccountId, transactionDto.ReceiverAccountId, transactionDto.Amount,
                    transactionDto.PaymentChannel, transactionDto.Category, transactionDto.Type);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    
}
