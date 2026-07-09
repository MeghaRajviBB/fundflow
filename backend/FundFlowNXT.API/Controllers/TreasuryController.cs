using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Data;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreasuryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TreasuryController(AppDbContext context)
        {
            _context = context;
        }

        // GET all transactions
        [HttpGet]
        public async Task<ActionResult<List<BankTransaction>>> GetAll()
        {
            var transactions = await _context.BankTransactions.ToListAsync();
            return Ok(transactions);
        }

        // GET uncleared only
        [HttpGet("uncleared")]
        public async Task<ActionResult<List<BankTransaction>>> GetUncleared()
        {
            var uncleared = await _context.BankTransactions
                .Where(t => !t.IsCleared)
                .ToListAsync();
            return Ok(uncleared);
        }

        // POST create transaction
        [HttpPost]
        public async Task<ActionResult<BankTransaction>> Create(BankTransaction transaction)
        {
            _context.BankTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = transaction.Id }, transaction);
        }

        // PUT bulk clear
        [HttpPut("bulkclear")]
        public async Task<ActionResult> BulkClear([FromBody] List<int> ids)
        {
            var cleared = 0;
            foreach (var id in ids)
            {
                var transaction = await _context.BankTransactions.FindAsync(id);
                if (transaction != null)
                {
                    transaction.IsCleared = true;
                    cleared++;
                }
            }
            await _context.SaveChangesAsync();
            return Ok($"{cleared} transactions cleared successfully.");
        }

        // GET balance summary
        [HttpGet("balance")]
        public async Task<ActionResult> GetBalance()
        {
            var transactions = await _context.BankTransactions.ToListAsync();
            var totalCredits = transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            var totalDebits = transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);

            return Ok(new
            {
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                Balance = totalCredits - totalDebits,
                UnclearedTransactions = transactions.Count(t => !t.IsCleared)
            });
        }
    }
}