using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_SensorSettingService.Compontents;
using Isaac_SensorSettingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isaac_SensorSettingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorService _sensorService;
        public SensorController( SensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        [Route("sensors/{floor}")]
        public List<SensorModel> GetSensorsByFloor(string floor)
        {
            return _sensorService.GetSensorsByFloor(floor);
        }

        [HttpPost]
        [Route("add")]
        public string AddSensor([FromBody] SensorModel sensor)
        {
            return _sensorService.AddSensor(sensor);
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "test";
        }
    }
}
