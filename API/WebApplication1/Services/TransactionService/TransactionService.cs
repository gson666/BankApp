using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<TransactionDto> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<TransactionDto> CreateTransactionAsync(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task UpdateTransactionAsync(int transactionId, TransactionDto transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) throw new Exception("Transaction not found");

            _mapper.Map(transactionDto, transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) throw new Exception("Transaction not found");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(int accountId)
        {
            var transactions = await _context.Transactions.Where(t => t.SenderAccountId == accountId || t.ReceiverAccountId == accountId).ToListAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> TransferMoneyAsync(int senderAccountId, int receiverAccountId, decimal amount, string paymentChannel, string category, string type)
        {
            var senderAccount = await _context.Accounts.FindAsync(senderAccountId);
            var receiverAccount = await _context.Accounts.FindAsync(receiverAccountId);

            if (senderAccount == null || receiverAccount == null)
                throw new Exception("One or both accounts not found");

            if (senderAccount.AvailAbleBalance < amount)
                throw new Exception("Insufficient balance");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                senderAccount.AvailAbleBalance -= amount;
                receiverAccount.AvailAbleBalance += amount;

                var newTransaction = new Transaction
                {
                    Name = "Transfer",
                    Amount = amount,
                    PaymentChannel = (PaymentChannel)Enum.Parse(typeof(PaymentChannel), paymentChannel),
                    Category = category,
                    Type = type,
                    SenderAccountId = senderAccountId,
                    ReceiverAccountId = receiverAccountId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<TransactionDto>(newTransaction);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
