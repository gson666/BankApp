using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services.AccountService
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync();
        Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(string userId);
        Task<AccountDto> GetAccountByIdAsync(int accountId);
        Task<AccountDto> CreateAccountAsync(AccountDto accountDto);
        Task UpdateAccountAsync(int accountId, AccountDto accountDto);
        Task DeleteAccountAsync(int accountId);
    }
}
