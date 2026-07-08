using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetingController : ControllerBase
    {
        private static List<Budget> _budgets = new List<Budget>
        {
            new Budget { Id=1, Department="Education", Category="Salaries", BudgetedAmount=300000, ActualAmount=280000, FiscalYear=2025, Quarter="Q1" },
            new Budget { Id=2, Department="Operations", Category="Equipment", BudgetedAmount=150000, ActualAmount=175000, FiscalYear=2025, Quarter="Q1" },
            new Budget { Id=3, Department="Healthcare", Category="Supplies", BudgetedAmount=200000, ActualAmount=120000, FiscalYear=2025, Quarter="Q1" },
            new Budget { Id=4, Department="Fundraising", Category="Marketing", BudgetedAmount=100000, ActualAmount=95000, FiscalYear=2025, Quarter="Q1" },
        };

        // GET all budgets
        [HttpGet]
        public ActionResult<List<Budget>> GetAll()
        {
            return Ok(_budgets);
        }

        // GET over-budget departments
        [HttpGet("overbudget")]
        public ActionResult<List<Budget>> GetOverBudget()
        {
            var over = _budgets.Where(b => b.IsOverBudget).ToList();
            if (!over.Any())
                return Ok("All departments are within budget.");
            return Ok(over);
        }

        // GET budget vs actual comparison
        [HttpGet("comparison")]
        public ActionResult GetComparison()
        {
            var comparison = _budgets.Select(b => new
            {
                b.Department,
                b.Category,
                b.BudgetedAmount,
                b.ActualAmount,
                b.Variance,
                b.UtilizationPercent,
                Status = b.IsOverBudget ? "Over Budget" : "Within Budget"
            }).ToList();
            return Ok(comparison);
        }

        // GET overall financial summary
        [HttpGet("summary")]
        public ActionResult GetSummary()
        {
            var summary = new
            {
                TotalBudgeted = _budgets.Sum(b => b.BudgetedAmount),
                TotalActual = _budgets.Sum(b => b.ActualAmount),
                TotalVariance = _budgets.Sum(b => b.Variance),
                DepartmentsOverBudget = _budgets.Count(b => b.IsOverBudget),
                DepartmentsWithinBudget = _budgets.Count(b => !b.IsOverBudget)
            };
            return Ok(summary);
        }

        // POST create a new budget line
        [HttpPost]
        public ActionResult<Budget> Create(Budget budget)
        {
            budget.Id = _budgets.Count + 1;
            _budgets.Add(budget);
            return CreatedAtAction(nameof(GetAll), new { id = budget.Id }, budget);
        }
    }
}