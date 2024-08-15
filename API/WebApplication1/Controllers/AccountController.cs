using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services.AccountService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] bool includeDeleted = false)
        {
            var accounts = await _accountService.GetAccountsAsync(includeDeleted);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id, [FromQuery] bool includeDeleted = false)
        {
            var account = await _accountService.GetAccountByIdAsync(id, includeDeleted);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAccount(AccountDto accountDto)
        {
            accountDto.InstitutionId = BankType.Id;
            var account = await _accountService.CreateAccountAsync(accountDto);
            account.InstitutionId = accountDto.InstitutionId;
            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDto accountDto)
        {
            if (id != accountDto.AccountId)
            {
                return BadRequest();
            }
            await _accountService.UpdateAccountAsync(id, accountDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }

        [HttpPut("deactivate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivateAccount(int id)
        {
            var account = await _accountService.DeactivateAccount(id);
            return Ok(new { account });
        }
        [HttpPut("activate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ActivateAccount(int id)
        {
            var account = await _accountService.ActivateAccount(id);
            return Ok(new { account });
        }

        [HttpGet("count-accounts")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CountAccounts()
        {
            return Ok(await _accountService.CountAccounts());
        }
    }
}
