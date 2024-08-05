using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync();
        Task<TransactionDto> GetTransactionByIdAsync(int transactionId);
        Task<TransactionDto> CreateTransactionAsync(TransactionDto transactionDto);
        Task UpdateTransactionAsync(int transactionId, TransactionDto transactionDto);
        Task DeleteTransactionAsync(int transactionId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(int accountId);
        Task<TransactionDto> TransferMoneyAsync(int senderAccountId, int receiverAccountId, decimal amount, string paymentChannel, string category, string type);
    }
}
