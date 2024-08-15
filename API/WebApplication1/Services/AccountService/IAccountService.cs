using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services.AccountService
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync(bool includeDeleted);
        Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(string userId, bool includeDeleted);
        Task<AccountDto> GetAccountByIdAsync(int accountId, bool includeDeleted);
        Task<AccountDto> CreateAccountAsync(AccountDto accountDto);
        Task UpdateAccountAsync(int accountId, AccountDto accountDto);
        Task DeleteAccountAsync(int accountId);
        Task<int?> CountAccounts();
        Task<AccountDto> DeactivateAccount(int accountId);
        Task<AccountDto> ActivateAccount(int accountId);
        Task<decimal?> DepositForClient(int accountId, decimal amount);
        Task<decimal?> WithdrawForClient(int accountId, decimal amount);
    }
}
