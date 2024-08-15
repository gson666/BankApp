using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DB;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly AppDb _context;
        private readonly IMapper _mapper;

        public AccountService(AppDb context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsAsync(bool includeDeleted = false)
        {
            var query = _context.Accounts.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(a => !a.IsDeleted);
            }

            var accounts = await query.ToListAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(string userId, bool includeDeleted = false)
        {
            var query = _context.Accounts.Where(a => a.UserId == userId);
            if (!includeDeleted)
            {
                query = query.Where(a => !a.IsDeleted);
            }

            var accounts = await query.ToListAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto> GetAccountByIdAsync(int accountId, bool includeDeleted = false)
        {
            var query = _context.Accounts.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(a => !a.IsDeleted);
            }

            var account = await query.FirstOrDefaultAsync(a => a.AccountId == accountId);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> CreateAccountAsync(AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountDto>(account);
        }

        public async Task UpdateAccountAsync(int accountId, AccountDto accountDto)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            _mapper.Map(accountDto, account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> CountAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            int NumberOfAccounts = accounts.Count;
            return NumberOfAccounts;
        }
        public async Task<AccountDto> DeactivateAccount(int accountId)
        {
            var accountToDelete = await _context.Accounts.FindAsync(accountId);
            if (accountToDelete == null) throw new Exception("No account found");

            accountToDelete.IsDeleted = true;
            _context.Accounts.Update(accountToDelete);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountDto>(accountToDelete);
        }
        public async Task<AccountDto> ActivateAccount(int accountId)
        {
            var accountToActivate = await _context.Accounts.FindAsync(accountId);
            if (accountToActivate == null) throw new Exception("No account found");

            accountToActivate.IsDeleted = false;
            _context.Accounts.Update(accountToActivate);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountDto>(accountToActivate);
        }
        public async Task<decimal?> DepositForClient(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return null;

            account.AvailAbleBalance += amount;
            await _context.SaveChangesAsync();

            return account.AvailAbleBalance;
        }

        public async Task<decimal?> WithdrawForClient(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return null;
            if(account.MaxCredit < amount)
            {
                return null;
            }
            account.AvailAbleBalance -= amount;
            await _context.SaveChangesAsync();
            return account.AvailAbleBalance;
        }
    }
}
