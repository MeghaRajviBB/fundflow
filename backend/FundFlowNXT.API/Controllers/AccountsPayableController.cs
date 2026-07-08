using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsPayableController : ControllerBase
    {
        private static List<Invoice> _invoices = new List<Invoice>
        {
            new Invoice { Id=1, VendorName="Office Depot", Description="Office Supplies", Amount=15000, InvoiceDate=DateTime.Now.AddDays(-30), DueDate=DateTime.Now.AddDays(-5), Status="Pending" },
            new Invoice { Id=2, VendorName="Azure Cloud", Description="Cloud Services", Amount=25000, InvoiceDate=DateTime.Now.AddDays(-15), DueDate=DateTime.Now.AddDays(10), Status="Approved" },
            new Invoice { Id=3, VendorName="Cleaning Co", Description="Office Cleaning", Amount=8000, InvoiceDate=DateTime.Now.AddDays(-45), DueDate=DateTime.Now.AddDays(-20), Status="Pending" },
            new Invoice { Id=4, VendorName="Tech Support", Description="IT Services", Amount=35000, InvoiceDate=DateTime.Now.AddDays(-10), DueDate=DateTime.Now.AddDays(15), Status="Paid" },
            new Invoice { Id=5, VendorName="Print Media", Description="Marketing Materials", Amount=12000, InvoiceDate=DateTime.Now.AddDays(-20), DueDate=DateTime.Now.AddDays(5), Status="Pending" },
        };

        // GET all invoices
        [HttpGet]
        public ActionResult<List<Invoice>> GetAll()
        {
            return Ok(_invoices);
        }

        // GET overdue invoices only
        [HttpGet("overdue")]
        public ActionResult<List<Invoice>> GetOverdue()
        {
            var overdue = _invoices.Where(i => i.IsOverdue).ToList();
            return Ok(overdue);
        }

        // GET invoices by status
        [HttpGet("status/{status}")]
        public ActionResult<List<Invoice>> GetByStatus(string status)
        {
            var filtered = _invoices
                .Where(i => i.Status.ToLower() == status.ToLower())
                .ToList();

            if (!filtered.Any())
                return NotFound($"No invoices found with status: {status}");

            return Ok(filtered);
        }

        // POST create new invoice
        [HttpPost]
        public ActionResult<Invoice> Create(Invoice invoice)
        {
            invoice.Id = _invoices.Count + 1;
            invoice.Status = "Pending";
            _invoices.Add(invoice);
            return CreatedAtAction(nameof(GetAll), new { id = invoice.Id }, invoice);
        }

        // PUT approve invoice
        [HttpPut("{id}/approve")]
        public ActionResult Approve(int id)
        {
            var invoice = _invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
                return NotFound($"Invoice {id} not found");

            if (invoice.Status == "Paid")
                return BadRequest("Invoice is already paid");

            invoice.Status = "Approved";
            return Ok($"Invoice {id} approved successfully");
        }

        // PUT mark as paid
        [HttpPut("{id}/pay")]
        public ActionResult Pay(int id)
        {
            var invoice = _invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
                return NotFound($"Invoice {id} not found");

            if (invoice.Status != "Approved")
                return BadRequest("Invoice must be approved before payment");

            invoice.Status = "Paid";
            return Ok($"Invoice {id} marked as paid");
        }

        // GET aging report
        [HttpGet("aging")]
        public ActionResult GetAgingReport()
        {
            var report = new
            {
                TotalInvoices = _invoices.Count,
                TotalPending = _invoices.Count(i => i.Status == "Pending"),
                TotalApproved = _invoices.Count(i => i.Status == "Approved"),
                TotalPaid = _invoices.Count(i => i.Status == "Paid"),
                TotalOverdue = _invoices.Count(i => i.IsOverdue),
                TotalAmountPending = _invoices
                    .Where(i => i.Status != "Paid")
                    .Sum(i => i.Amount),
                OverdueInvoices = _invoices
                    .Where(i => i.IsOverdue)
                    .Select(i => new { i.Id, i.VendorName, i.Amount, i.DueDate })
                    .ToList()
            };
            return Ok(report);
        }
    }
}