using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;
using FundFlowNXT.API.Services;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrantsController : ControllerBase
    {
        private readonly IGrantService _service;

        public GrantsController(IGrantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Grant>>> GetAll()
        {
            var grants = await _service.GetAllGrantsAsync();
            return Ok(grants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grant>> GetById(int id)
        {
            var grant = await _service.GetGrantByIdAsync(id);
            if (grant == null)
                return NotFound($"Grant {id} not found");
            return Ok(grant);
        }

        [HttpPost]
        public async Task<ActionResult<Grant>> Create(Grant grant)
        {
            try
            {
                var created = await _service.CreateGrantAsync(grant);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Grant grant)
        {
            var updated = await _service.UpdateGrantAsync(id, grant);
            if (updated == null)
                return NotFound($"Grant {id} not found");
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteGrantAsync(id);
            if (!deleted)
                return NotFound($"Grant {id} not found");
            return Ok($"Grant {id} deleted");
        }

        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var summary = await _service.GetSummaryAsync();
            return Ok(summary);
        }
    }
}
