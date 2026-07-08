using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrantsController : ControllerBase
    {
        private static List<Grant> _grants = new List<Grant>
        {
            new Grant { Id=1, GrantName="Education Initiative", FunderName="Bill Gates Foundation", TotalAmount=500000, SpentAmount=320000, StartDate=DateTime.Now.AddMonths(-6), EndDate=DateTime.Now.AddMonths(6), Status="Active" },
            new Grant { Id=2, GrantName="Healthcare Outreach", FunderName="Government of India", TotalAmount=200000, SpentAmount=210000, StartDate=DateTime.Now.AddMonths(-12), EndDate=DateTime.Now.AddMonths(1), Status="Active" },
            new Grant { Id=3, GrantName="Women Empowerment", FunderName="UN Foundation", TotalAmount=350000, SpentAmount=100000, StartDate=DateTime.Now.AddMonths(-3), EndDate=DateTime.Now.AddMonths(9), Status="Active" },
            new Grant { Id=4, GrantName="Rural Development", FunderName="World Bank", TotalAmount=1000000, SpentAmount=1000000, StartDate=DateTime.Now.AddMonths(-24), EndDate=DateTime.Now.AddMonths(-1), Status="Closed" },
        };

        // GET all grants
        [HttpGet]
        public ActionResult<List<Grant>> GetAll()
        {
            return Ok(_grants);
        }

        // GET overspent grants — restricted fund violation
        [HttpGet("overspent")]
        public ActionResult<List<Grant>> GetOverspent()
        {
            var overspent = _grants.Where(g => g.IsOverspent).ToList();
            if (!overspent.Any())
                return Ok("No overspent grants. All funds within limit.");
            return Ok(overspent);
        }

        // GET grant summary
        [HttpGet("summary")]
        public ActionResult GetSummary()
        {
            var summary = new
            {
                TotalGrants = _grants.Count,
                ActiveGrants = _grants.Count(g => g.Status == "Active"),
                OverspentGrants = _grants.Count(g => g.IsOverspent),
                ExpiredGrants = _grants.Count(g => g.IsExpired),
                TotalFunding = _grants.Sum(g => g.TotalAmount),
                TotalSpent = _grants.Sum(g => g.SpentAmount),
                TotalRemaining = _grants.Sum(g => g.RemainingBalance)
            };
            return Ok(summary);
        }

        // POST record spending against a grant
        [HttpPost("{id}/spend")]
        public ActionResult RecordSpending(int id, [FromBody] decimal amount)
        {
            var grant = _grants.FirstOrDefault(g => g.Id == id);
            if (grant == null)
                return NotFound($"Grant {id} not found");

            if (grant.IsExpired)
                return BadRequest("Cannot spend against an expired grant");

            if (grant.SpentAmount + amount > grant.TotalAmount)
                return BadRequest($"This spend of {amount} will exceed the grant limit. Remaining balance is {grant.RemainingBalance}");

            grant.SpentAmount += amount;
            return Ok(new
            {
                Message = $"Spending of {amount} recorded successfully",
                NewSpentAmount = grant.SpentAmount,
                RemainingBalance = grant.RemainingBalance
            });
        }

        // GET single grant by id
        [HttpGet("{id}")]
        public ActionResult<Grant> GetById(int id)
        {
            var grant = _grants.FirstOrDefault(g => g.Id == id);
            if (grant == null)
                return NotFound($"Grant {id} not found");
            return Ok(grant);
        }
    }
}