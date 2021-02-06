using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diary.Data.Services.Entry;
using Diary.Lib.Entry;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntryController : Controller
    {
        public const int PageSize = 7;

        private readonly IEntryService _service;

        public EntryController(IEntryService service)
        {
            _service = service;
        }

        [HttpGet("entries")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
            return Ok(await _service.GetEntriesAsync(PageSize));
        }

        [HttpGet("next-entries")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetNextEntries()
        {
            return Ok(await _service.GetNextEntriesAsync(PageSize));
        }

        [HttpGet("previous-entries")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetPreviousEntries()
        {
            return Ok(await _service.GetPreviousEntriesAsync(PageSize));
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntriesByDate([FromBody] DateTime startDate,
            [FromBody] DateTime endDate)
        {
            return Ok(await _service.GetEntriesByDateBetweenAsync(startDate, endDate, PageSize));
        }

        [HttpGet("next-by_date")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetNextEntriesByDate([FromBody] DateTime startDate,
            [FromBody] DateTime endDate)
        {
            return Ok(await _service.GetNextEntriesByDateBetweenAsync(startDate, endDate, PageSize));
        }
        
        [HttpGet("previous-by_date")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetPreviousEntriesByDate([FromBody] DateTime startDate,
            [FromBody] DateTime endDate)
        {
            return Ok(await _service.GetPreviousEntriesByDateBetweenAsync(startDate, endDate, PageSize));
        }
        
        [HttpGet("one-by-date")]
        public async Task<ActionResult<Entry>> GetOneEntryByDate([FromBody] DateTime date)
        {
            var entry = await _service.GetEntryByDateAsync(date);
            if (entry != null)
            {
                return Ok(entry);
            }
            return BadRequest(date);
        }
        
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<Entry>> GetOneEntryById([FromRoute] int id)
        {
            var entry = await _service.GetByIdAsync(id);
            if (entry != null)
            {
                return Ok(entry);
            }
            return BadRequest(id);
        }

        [HttpPost("add-or-update")]
        public async Task<ActionResult<Entry>> AddOrUpdateEntry([FromBody] Entry entry)
        {
            return Ok(await _service.AddOrUpdateEntryAsync(entry));
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Entry>> Update([FromRoute] int id, [FromBody] Entry entry)
        {
            if (await _service.UpdateEntryAsync(id, entry))
            {
                return Ok(entry);
            }
            return BadRequest(id);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Entry>> Delete([FromRoute] int id)
        {
            if (await _service.DeleteEntryAsync(id))
            {
                return Ok(id);
            }
            return BadRequest(id);
        }
    }
}