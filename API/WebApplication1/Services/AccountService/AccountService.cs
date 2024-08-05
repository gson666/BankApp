using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(string userId)
        {
            var accounts = await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto> GetAccountByIdAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
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
    }
}
