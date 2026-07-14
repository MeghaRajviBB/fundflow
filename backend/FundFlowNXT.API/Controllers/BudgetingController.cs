using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Data;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetingController(AppDbContext context)
        {
            _context = context;
        }

        // GET all budgets
        [HttpGet]
        public async Task<ActionResult<List<Budget>>> GetAll()
        {
            var budgets = await _context.Budgets.ToListAsync();
            return Ok(budgets);
        }

        // GET budget vs actual comparison
        [HttpGet("comparison")]
        public async Task<ActionResult> GetComparison()
        {
            var budgets = await _context.Budgets.ToListAsync();
            var comparison = budgets.Select(b => new
            {
                b.Department,
                b.Category,
                b.BudgetedAmount,
                b.ActualAmount,
                Variance = b.BudgetedAmount - b.ActualAmount,
                Status = b.ActualAmount > b.BudgetedAmount ? "Over Budget" : "Within Budget"
            }).ToList();
            return Ok(comparison);
        }

        // GET summary
        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var budgets = await _context.Budgets.ToListAsync();
            return Ok(new
            {
                TotalBudgeted = budgets.Sum(b => b.BudgetedAmount),
                TotalActual = budgets.Sum(b => b.ActualAmount),
                TotalVariance = budgets.Sum(b => b.BudgetedAmount - b.ActualAmount),
                DepartmentsOverBudget = budgets.Count(b => b.ActualAmount > b.BudgetedAmount),
                DepartmentsWithinBudget = budgets.Count(b => b.ActualAmount <= b.BudgetedAmount)
            });
        }

        // POST create budget line
        [HttpPost]
        public async Task<ActionResult<Budget>> Create(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = budget.Id }, budget);
        }
        // PUT update budget
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Budget updated)
        {
            var b = await _context.Budgets.FindAsync(id);
            if (b == null) return NotFound($"Budget {id} not found");

            b.Department = updated.Department;
            b.Category = updated.Category;
            b.BudgetedAmount = updated.BudgetedAmount;
            b.ActualAmount = updated.ActualAmount;
            b.FiscalYear = updated.FiscalYear;
            b.Quarter = updated.Quarter;

            await _context.SaveChangesAsync();
            return Ok(b);
        }

        // DELETE budget
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var b = await _context.Budgets.FindAsync(id);
            if (b == null) return NotFound($"Budget {id} not found");

            _context.Budgets.Remove(b);
            await _context.SaveChangesAsync();
            return Ok($"Budget {id} deleted");
        }
    }
}