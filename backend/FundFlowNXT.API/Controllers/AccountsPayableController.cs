using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Data;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsPayableController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountsPayableController(AppDbContext context)
        {
            _context = context;
        }

        // GET all invoices
        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> GetAll()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return Ok(invoices);
        }

        // GET by status
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<Invoice>>> GetByStatus(string status)
        {
            var filtered = await _context.Invoices
                .Where(i => i.Status.ToLower() == status.ToLower())
                .ToListAsync();

            if (!filtered.Any())
                return NotFound($"No invoices found with status: {status}");

            return Ok(filtered);
        }

        // POST create invoice
        [HttpPost]
        public async Task<ActionResult<Invoice>> Create(Invoice invoice)
        {
            invoice.Status = "Pending";
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = invoice.Id }, invoice);
        }

        // PUT approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> Approve(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound($"Invoice {id} not found");

            if (invoice.Status == "Paid")
                return BadRequest("Invoice is already paid");

            invoice.Status = "Approved";
            await _context.SaveChangesAsync();
            return Ok($"Invoice {id} approved successfully");
        }

        // PUT pay
        [HttpPut("{id}/pay")]
        public async Task<ActionResult> Pay(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound($"Invoice {id} not found");

            if (invoice.Status != "Approved")
                return BadRequest("Invoice must be approved before payment");

            invoice.Status = "Paid";
            await _context.SaveChangesAsync();
            return Ok($"Invoice {id} marked as paid");
        }

        // GET aging report
        [HttpGet("aging")]
        public async Task<ActionResult> GetAgingReport()
        {
            var invoices = await _context.Invoices.ToListAsync();
            var report = new
            {
                TotalInvoices = invoices.Count,
                TotalPending = invoices.Count(i => i.Status == "Pending"),
                TotalApproved = invoices.Count(i => i.Status == "Approved"),
                TotalPaid = invoices.Count(i => i.Status == "Paid"),
                TotalAmountPending = invoices
                    .Where(i => i.Status != "Paid")
                    .Sum(i => i.Amount)
            };
            return Ok(report);
        }
    }
}