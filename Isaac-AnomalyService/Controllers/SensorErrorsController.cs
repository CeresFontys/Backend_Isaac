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
    [Route("SensorError")]
    [ApiController]
    public class SensorErrorsController : ControllerBase
    {
        private readonly Isaac_AnomalyServiceContext _context;

        public SensorErrorsController(Isaac_AnomalyServiceContext context)
        {
            _context = context;
        }

        // GET: api/SensorErrors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorError>>> GetErrors()
        {
            return await _context.Errors.ToListAsync();
        }

        // GET: api/SensorErrors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorError>> GetSensorError(long id)
        {
            var sensorError = await _context.Errors.FindAsync(id);

            if (sensorError == null)
            {
                return NotFound();
            }

            return sensorError;
        }

        // PUT: api/SensorErrors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorError(long id, SensorError sensorError)
        {
            if (id != sensorError.id)
            {
                return BadRequest();
            }

            _context.Entry(sensorError).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorErrorExists(id))
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

        // POST: api/SensorErrors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SensorError>> PostSensorError(SensorError sensorError)
        {
            _context.Errors.Add(sensorError);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensorError", new { id = sensorError.id }, sensorError);
        }

        // DELETE: api/SensorErrors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SensorError>> DeleteSensorError(long id)
        {
            var sensorError = await _context.Errors.FindAsync(id);
            if (sensorError == null)
            {
                return NotFound();
            }

            _context.Errors.Remove(sensorError);
            await _context.SaveChangesAsync();

            return sensorError;
        }

        private bool SensorErrorExists(long id)
        {
            return _context.Errors.Any(e => e.id == id);
        }
    }
}
