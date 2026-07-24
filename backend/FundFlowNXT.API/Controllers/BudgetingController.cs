using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Services;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetingController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetingController(IBudgetService service)
        {
            _service = service;
        }

        // GET all budgets
        [HttpGet]
        public async Task<ActionResult<List<Budget>>> GetAll()
        {
            var budgets = await _service.GetAllBudgetsAsync();
            return Ok(budgets);
        }

        // GET budget vs actual comparison
        [HttpGet("comparison")]
        public async Task<ActionResult> GetComparison()
        {
            var comparison = await _service.GetComparisonAsync();
            return Ok(comparison);
        }

        // GET summary
        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var summary = await _service.GetSummaryAsync();
            return Ok(summary);
        }

        // POST create budget line
        [HttpPost]
        public async Task<ActionResult<Budget>> Create(Budget budget)
        {
            var created = await _service.CreateBudgetAsync(budget);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        // PUT update budget
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Budget updated)
        {
            var b = await _service.UpdateBudgetAsync(id, updated);
            if (b == null) return NotFound($"Budget {id} not found");
            return Ok(b);
        }

        // DELETE budget
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteBudgetAsync(id);
            if (!deleted) return NotFound($"Budget {id} not found");
            return Ok($"Budget {id} deleted");
        }
    }
}