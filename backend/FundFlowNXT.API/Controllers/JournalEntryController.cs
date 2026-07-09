using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Data;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JournalEntryController(AppDbContext context)
        {
            _context = context;
        }

        // GET all journal entries
        [HttpGet]
        public async Task<ActionResult<List<JournalEntry>>> GetAll()
        {
            var entries = await _context.JournalEntries.ToListAsync();
            return Ok(entries);
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<JournalEntry>> GetById(int id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound();
            return Ok(entry);
        }

        // POST create entry
        [HttpPost]
        public async Task<ActionResult<JournalEntry>> Create(JournalEntry entry)
        {
            if (entry.DebitAmount != entry.CreditAmount)
                return BadRequest("Entry is not balanced. Debit must equal Credit.");

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }
    }
}