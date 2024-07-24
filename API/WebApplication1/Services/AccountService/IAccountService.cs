using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services.AccountService
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync();
        Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(string userId);
        Task<AccountDto> GetAccountByIdAsync(string id);
        Task<AccountDto> CreateAccountAsync(AccountDto accountDto);
        Task UpdateAccountAsync(string id, AccountDto accountDto);
        Task DeleteAccountAsync(string id);
    }
}
