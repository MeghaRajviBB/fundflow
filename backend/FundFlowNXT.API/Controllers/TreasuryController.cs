using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Services;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreasuryController : ControllerBase
    {
        private readonly ITreasuryService _service;

        public TreasuryController(ITreasuryService service)
        {
            _service = service;
        }

        // GET all transactions
        [HttpGet]
        public async Task<ActionResult<List<BankTransaction>>> GetAll()
        {
            var transactions = await _service.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        // GET uncleared only
        [HttpGet("uncleared")]
        public async Task<ActionResult<List<BankTransaction>>> GetUncleared()
        {
            var uncleared = await _service.GetUnclearedAsync();
            return Ok(uncleared);
        }

        // POST create transaction
        [HttpPost]
        public async Task<ActionResult<BankTransaction>> Create(BankTransaction transaction)
        {
            var created = await _service.CreateTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        // PUT update transaction
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, BankTransaction updated)
        {
            var t = await _service.UpdateTransactionAsync(id, updated);
            if (t == null) return NotFound($"Transaction {id} not found");
            return Ok(t);
        }

        // DELETE transaction
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteTransactionAsync(id);
            if (!deleted) return NotFound($"Transaction {id} not found");
            return Ok($"Transaction {id} deleted");
        }

        // PUT bulk clear
        [HttpPut("bulkclear")]
        public async Task<ActionResult> BulkClear([FromBody] List<int> ids)
        {
            var cleared = await _service.BulkClearAsync(ids);
            return Ok($"{cleared} transactions cleared successfully.");
        }

        // GET balance summary
        [HttpGet("balance")]
        public async Task<ActionResult> GetBalance()
        {
            var balance = await _service.GetBalanceAsync();
            return Ok(balance);
        }
    }
}