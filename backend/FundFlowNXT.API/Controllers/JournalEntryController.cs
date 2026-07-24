using FundFlowNXT.API.Services;
using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalEntryController : ControllerBase
    {
        private readonly IJournalEntryService _service;

        public JournalEntryController(IJournalEntryService service)
        {
            _service = service;
        }

        // GET all journal entries
        [HttpGet]
        public async Task<ActionResult<List<JournalEntry>>> GetAll()
        {
            var entries = await _service.GetAllEntriesAsync();
            return Ok(entries);
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<JournalEntry>> GetById(int id)
        {
            var entry = await _service.GetEntryByIdAsync(id);
            if (entry == null)
                return NotFound();
            return Ok(entry);
        }

        // POST create entry
        [HttpPost]
        public async Task<ActionResult<JournalEntry>> Create(JournalEntry entry)
        {
            try
            {
                var created = await _service.CreateEntryAsync(entry);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT update an existing entry
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, JournalEntry updated)
        {
            var entry = await _service.UpdateEntryAsync(id, updated);
            if (entry == null)
                return NotFound($"Entry {id} not found");
            return Ok(entry);
        }

        // DELETE an entry
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteEntryAsync(id);
            if (!deleted)
                return NotFound($"Entry {id} not found");
            return Ok($"Entry {id} deleted");
        }

        // POST analyze a single entry for anomalies (without saving)
        [HttpPost("analyze")]
        public ActionResult Analyze(JournalEntry entry)
        {
            var result = _service.AnalyzeEntry(entry);
            return Ok(result);
        }

        // GET scan all existing entries for anomalies
        [HttpGet("scan-anomalies")]
        public async Task<ActionResult> ScanAnomalies()
        {
            var result = await _service.ScanAnomaliesAsync();
            return Ok(result);
        }
    }
}