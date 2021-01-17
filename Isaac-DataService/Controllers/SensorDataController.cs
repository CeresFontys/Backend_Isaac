using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_DataService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Isaac_DataService.Controllers
{
    [Route("data")]
    [ApiController]
    public class SensorDataController : Controller
    {
        private readonly DataOutputService _service;
        private readonly DataService _data;

        public SensorDataController(DataOutputService service, DataService data)
        {
            _service = service;
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> SensorData([FromQuery] long time, [FromQuery] string floor)
        {
            if (time == default || floor == null || floor == "")
            {
                BadRequest();
            }
            return Ok(await _data.GatherData(time, floor, TimeSpan.FromMinutes(5)));
        }
    }
}