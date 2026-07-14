using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Data;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GrantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET all grants
        [HttpGet]
        public async Task<ActionResult<List<Grant>>> GetAll()
        {
            var grants = await _context.Grants.ToListAsync();
            return Ok(grants);
        }

        // GET single grant by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Grant>> GetById(int id)
        {
            var grant = await _context.Grants.FindAsync(id);
            if (grant == null)
                return NotFound($"Grant {id} not found");
            return Ok(grant);
        }

        // POST create grant
        [HttpPost]
        public async Task<ActionResult<Grant>> Create(Grant grant)
        {
            _context.Grants.Add(grant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = grant.Id }, grant);
        }
        // PUT update grant
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Grant updated)
        {
            var g = await _context.Grants.FindAsync(id);
            if (g == null) return NotFound($"Grant {id} not found");

            g.GrantName = updated.GrantName;
            g.FunderName = updated.FunderName;
            g.TotalAmount = updated.TotalAmount;
            g.SpentAmount = updated.SpentAmount;
            g.StartDate = updated.StartDate;
            g.EndDate = updated.EndDate;
            g.Status = updated.Status;

            await _context.SaveChangesAsync();
            return Ok(g);
        }

        // DELETE grant
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var g = await _context.Grants.FindAsync(id);
            if (g == null) return NotFound($"Grant {id} not found");

            _context.Grants.Remove(g);
            await _context.SaveChangesAsync();
            return Ok($"Grant {id} deleted");
        }
        // POST record spending against a grant
        [HttpPost("{id}/spend")]
        public async Task<ActionResult> RecordSpending(int id, [FromBody] decimal amount)
        {
            var grant = await _context.Grants.FindAsync(id);
            if (grant == null)
                return NotFound($"Grant {id} not found");

            if (grant.SpentAmount + amount > grant.TotalAmount)
                return BadRequest($"This spend will exceed the grant limit. Remaining balance is {grant.TotalAmount - grant.SpentAmount}");

            grant.SpentAmount += amount;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Spending of {amount} recorded successfully",
                NewSpentAmount = grant.SpentAmount,
                RemainingBalance = grant.TotalAmount - grant.SpentAmount
            });
        }

        // GET summary
        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var grants = await _context.Grants.ToListAsync();
            return Ok(new
            {
                TotalGrants = grants.Count,
                ActiveGrants = grants.Count(g => g.Status == "Active"),
                OverspentGrants = grants.Count(g => g.SpentAmount > g.TotalAmount),
                TotalFunding = grants.Sum(g => g.TotalAmount),
                TotalSpent = grants.Sum(g => g.SpentAmount),
                TotalRemaining = grants.Sum(g => g.TotalAmount - g.SpentAmount)
            });
        }
    }
}