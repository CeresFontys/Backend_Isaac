using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;

namespace Isaac_AnomalyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : ControllerBase
    {
        private readonly Isaac_AnomalyServiceContext _context;

        public ErrorLogController(Isaac_AnomalyServiceContext context)
        {
            _context = context;
        }

        // GET: api/ErrorLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ErrorLog>>> GetErrorLog()
        {
            return await _context.ErrorLog.ToListAsync();
        }

        // GET: api/ErrorLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ErrorLog>> GetErrorLog(int id)
        {
            var errorLog = await _context.ErrorLog.FindAsync(id);

            if (errorLog == null)
            {
                return NotFound();
            }

            return errorLog;
        }

        // PUT: api/ErrorLog/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutErrorLog(int id, ErrorLog errorLog)
        {
            if (id != errorLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(errorLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ErrorLog
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ErrorLog>> PostErrorLog(ErrorLog errorLog)
        {
            _context.ErrorLog.Add(errorLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetErrorLog", new { id = errorLog.Id }, errorLog);
        }

        // DELETE: api/ErrorLog/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ErrorLog>> DeleteErrorLog(int id)
        {
            var errorLog = await _context.ErrorLog.FindAsync(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            _context.ErrorLog.Remove(errorLog);
            await _context.SaveChangesAsync();

            return errorLog;
        }

        private bool ErrorLogExists(int id)
        {
            return _context.ErrorLog.Any(e => e.Id == id);
        }
    }
}
