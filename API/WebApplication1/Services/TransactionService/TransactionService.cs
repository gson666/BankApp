using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.DB;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDb _context;
        private readonly IMapper _mapper;

        public TransactionService(AppDb context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<TransactionDto> CreateTransactionAsync(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task UpdateTransactionAsync(string id, TransactionDto transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) throw new Exception("Transaction not found");

            _mapper.Map(transactionDto, transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) throw new Exception("Transaction not found");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(string accountId)
        {
            var transactions = await _context.Transactions.Where(t => t.SenderBankId == accountId || t.ReceiverBankId == accountId).ToListAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
    }
}
