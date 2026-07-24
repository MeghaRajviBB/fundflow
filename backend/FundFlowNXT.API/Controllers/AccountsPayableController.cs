using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Services;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsPayableController : ControllerBase
    {
        private readonly IAccountsPayableService _service;

        public AccountsPayableController(IAccountsPayableService service)
        {
            _service = service;
        }

        // GET all invoices
        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> GetAll()
        {
            var invoices = await _service.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        // GET by status
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<Invoice>>> GetByStatus(string status)
        {
            var filtered = await _service.GetByStatusAsync(status);

            if (!filtered.Any())
                return NotFound($"No invoices found with status: {status}");

            return Ok(filtered);
        }

        // POST create invoice
        [HttpPost]
        public async Task<ActionResult<Invoice>> Create(Invoice invoice)
        {
            var created = await _service.CreateInvoiceAsync(invoice);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        // PUT update invoice
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Invoice updated)
        {
            var inv = await _service.UpdateInvoiceAsync(id, updated);
            if (inv == null) return NotFound($"Invoice {id} not found");
            return Ok(inv);
        }

        // DELETE invoice
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteInvoiceAsync(id);
            if (!deleted) return NotFound($"Invoice {id} not found");
            return Ok($"Invoice {id} deleted");
        }

        // PUT approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> Approve(int id)
        {
            try
            {
                var invoice = await _service.ApproveInvoiceAsync(id);
                if (invoice == null)
                    return NotFound($"Invoice {id} not found");
                return Ok($"Invoice {id} approved successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT pay
        [HttpPut("{id}/pay")]
        public async Task<ActionResult> Pay(int id)
        {
            try
            {
                var invoice = await _service.PayInvoiceAsync(id);
                if (invoice == null)
                    return NotFound($"Invoice {id} not found");
                return Ok($"Invoice {id} marked as paid");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET aging report
        [HttpGet("aging")]
        public async Task<ActionResult> GetAgingReport()
        {
            var report = await _service.GetAgingReportAsync();
            return Ok(report);
        }
    }
}