using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalEntryController : ControllerBase
    {
        private static List<JournalEntry> _entries = new List<JournalEntry>
        {
            new JournalEntry
            {
                Id = 1,
                Description = "Office Supplies",
                DebitAmount = 5000,
                CreditAmount = 5000,
                Date = DateTime.Now,
                Fund = "Operations Fund"
            },
            new JournalEntry
            {
                Id = 2,
                Description = "Education Program",
                DebitAmount = 10000,
                CreditAmount = 8000,
                Date = DateTime.Now,
                Fund = "Education Fund"
            }
        };

        [HttpGet]
        public ActionResult<List<JournalEntry>> GetAll()
        {
            return Ok(_entries);
        }

        [HttpGet("{id}")]
        public ActionResult<JournalEntry> GetById(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
                return NotFound();
            return Ok(entry);
        }

        [HttpPost]
        public ActionResult<JournalEntry> Create(JournalEntry entry)
        {
            entry.Id = _entries.Count + 1;
            _entries.Add(entry);

            if (!entry.IsBalanced)
                return BadRequest("Entry is not balanced. Debit must equal Credit.");

            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }
    }
}