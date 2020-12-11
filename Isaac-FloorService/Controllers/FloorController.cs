using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Isaac_FloorService;
using Isaac_FloorService.Data;
using Isaac_FloorService.Models;

namespace Isaac_FloorService.Controllers
{
    [Route("/Floor")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly FloorServiceContext _context;

        public FloorController(FloorServiceContext context)
        {
            _context = context;
        }

        // GET: /Floor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Floor>>> GetFloor()
        {
            return await _context.Floor.ToListAsync();
        }

        // GET: /Floor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Floor>> GetFloor(int id)
        {
            var floor = await _context.Floor.FindAsync(id);

            if (floor == null)
            {
                return NotFound();
            }

            return floor;
        }

        // PUT: api/Floor/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFloor(int id, [FromForm] FloorRequest floorRequest)
        {
            if (id != floorRequest.Floor.Id)
            {
                return BadRequest();
            }

            if (floorRequest.File != null)
            {
                await using var stream = Request.Form.Files.First().OpenReadStream();
                await using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                floorRequest.Floor.Image = memoryStream.ToArray();

                _context.Floor.Update(floorRequest.Floor);
            }
            else
            {
                var floor = await _context.Floor.FindAsync(id);
                floor.Length = floorRequest.Floor.Length;
                floor.Width = floorRequest.Floor.Width;
                floor.Name = floorRequest.Floor.Name;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloorExists(id))
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

        // POST: api/Floor
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Floor>> PostFloor([FromForm] FloorRequest floorRequest)
        {
            await using var stream = Request.Form.Files.First().OpenReadStream();
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            floorRequest.Floor.Image = memoryStream.ToArray();
            _context.Floor.Add(floorRequest.Floor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFloor", new { id = floorRequest.Floor.Id }, floorRequest.Floor);
        }

        // DELETE: api/Floor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Floor>> DeleteFloor(int id)
        {
            var floor = await _context.Floor.FindAsync(id);
            if (floor == null)
            {
                return NotFound();
            }

            _context.Floor.Remove(floor);
            await _context.SaveChangesAsync();

            return floor;
        }

        private bool FloorExists(int id)
        {
            return _context.Floor.Any(e => e.Id == id);
        }
    }
}
