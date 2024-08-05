using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Services.TransactionService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionDto transactionDto)
        {
            var transaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, TransactionDto transactionDto)
        {
            var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
            if (existingTransaction == null)
                return NotFound();

            await _transactionService.UpdateTransactionAsync(id, transactionDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
            if (existingTransaction == null)
                return NotFound();

            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
