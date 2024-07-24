using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync();
        Task<TransactionDto> GetTransactionByIdAsync(string id);
        Task<TransactionDto> CreateTransactionAsync(TransactionDto transactionDto);
        Task UpdateTransactionAsync(string id, TransactionDto transactionDto);
        Task DeleteTransactionAsync(string id);
        Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(string accountId);
    }
}
