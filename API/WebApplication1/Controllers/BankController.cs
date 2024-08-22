using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Services.AccountService;
using WebApplication1.Services.KeyService;
using WebApplication1.Services.TransactionService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IKeyService _keyService;
        
        public BankController(IAccountService accountService, ITransactionService transactionService,IKeyService keyService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _keyService = keyService;
        }

        [HttpGet("user/{userId}/accounts")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByUserId(string userId, [FromQuery] bool includeDeleted = false)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(userId, includeDeleted);
            return Ok(accounts);
        }

        [HttpGet("accounts/{accountId}")]
        public async Task<IActionResult> GetAccountById(int accountId, [FromQuery] bool includeDeleted = false)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId, includeDeleted);
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(new { account, transactions });
        }

        [HttpPost("accounts")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAccount(AccountDto accountDto)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdAccount.AccountId }, createdAccount);
        }

        [HttpPut("accounts/{accountId}")]
        [Authorize]
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
            return NoContent();
        }

        [HttpPut("deactivate/{accountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivateAccount(int accountId)
        {
            var account = await _accountService.DeactivateAccount(accountId);
            return Ok(account);
        }

        [HttpPost("transactions")]
        [Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionDto transactionDto)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdTransaction.SenderAccountId }, createdTransaction);
        }

        [HttpGet("accounts/{accountId}/transactions")]
        [Authorize]
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
                var transaction = await _transactionService.TransferMoneyAsync(transactionDto.Name,
                    transactionDto.SenderAccountId, transactionDto.ReceiverAccountId, transactionDto.Amount,
                    transactionDto.PaymentChannel, transactionDto.Category, transactionDto.Type);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("accounts/{accountId}/deposit")]
        public async Task<IActionResult> DepositForAccount(int accountId,[FromBody]decimal amount)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var updatedBalance = await _accountService.DepositForClient(accountId, amount);
            if(updatedBalance == null)
            {
                return NoContent();
            }
            return Ok(new {AccountId = accountId, NewBalance = updatedBalance});
        }

        [Authorize]
        [HttpPost("accounts/{accountId}/user-deposit")]
        public async Task<IActionResult> UserSelfDeposit(int accountId, [FromBody] decimal amount, [FromQuery] string userId, [FromQuery] string key)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User ID is required" });
            }

            if (string.IsNullOrEmpty(key))
            {
                return BadRequest(new { message = "Key is required" });
            }

            var isValidKey = await _keyService.ValidateKeyAsync(userId, key);
            if (!isValidKey)
            {
                return BadRequest(new { message = "Invalid or expired key" });
            }

            var updatedBalance = await _accountService.DepositForClient(accountId, amount);
            if (updatedBalance == null)
            {
                return NotFound(new { message = "Account not found or operation failed" });
            }

            await _keyService.InvalidateKeyAsync(userId, key);

            return Ok(new { AccountId = accountId, NewBalance = updatedBalance });
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("accounts/{accountId}/withdraw")]
        public async Task<IActionResult> WithdrawFromAccount(int accountId, [FromBody] decimal amount)
        {
            var updatedBalance = await _accountService.WithdrawForClient(accountId, amount);
            if (updatedBalance == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            return Ok(new { AccountId = accountId, NewBalance = updatedBalance });
        }
    }
}
