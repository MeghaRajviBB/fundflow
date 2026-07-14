using FundFlowNXT.API.Services;
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
        private readonly AnomalyDetectionService _anomalyService;

        public JournalEntryController(AppDbContext context, AnomalyDetectionService anomalyService)
        {
            _context = context;
            _anomalyService = anomalyService;
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
        // PUT update an existing entry
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, JournalEntry updated)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound($"Entry {id} not found");

            entry.Description = updated.Description;
            entry.DebitAmount = updated.DebitAmount;
            entry.CreditAmount = updated.CreditAmount;
            entry.Date = updated.Date;
            entry.Fund = updated.Fund;

            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        // DELETE an entry
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound($"Entry {id} not found");

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return Ok($"Entry {id} deleted");
        }
        // POST analyze a single entry for anomalies (without saving)
        [HttpPost("analyze")]
        public ActionResult Analyze(JournalEntry entry)
        {
            var result = _anomalyService.AnalyzeJournalEntry(entry);
            return Ok(result);
        }

        // GET scan all existing entries for anomalies
        [HttpGet("scan-anomalies")]
        public async Task<ActionResult> ScanAnomalies()
        {
            var entries = await _context.JournalEntries.ToListAsync();
            var flagged = new List<object>();

            foreach (var entry in entries)
            {
                var result = _anomalyService.AnalyzeJournalEntry(entry);
                if (result.HasAnomaly)
                {
                    flagged.Add(new
                    {
                        entry.Id,
                        entry.Description,
                        entry.Fund,
                        entry.DebitAmount,
                        entry.CreditAmount,
                        result.Severity,
                        result.Explanation,
                        result.Recommendation
                    });
                }
            }

            return Ok(new
            {
                TotalScanned = entries.Count,
                AnomaliesFound = flagged.Count,
                Anomalies = flagged
            });
        }
    }
}