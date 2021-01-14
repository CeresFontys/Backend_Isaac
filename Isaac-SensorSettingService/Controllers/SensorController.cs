using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        [Route("/{floor}")]
        public List<SensorModel> GetSensorsByFloor(string floor)
        {
            return _sensorService.GetSensorsByFloor(floor);
        }
        [HttpGet]
        public List<SensorModel> GetAllSensors()
        {
            return _sensorService.GetSensors();
        }

        [HttpPost]
        public string AddSensor([FromBody] SensorModel sensor)
        {
            return _sensorService.AddSensor(sensor);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSensor([FromBody] SensorModel sensor)
        {
            return Ok(await _sensorService.UpdateSensor(sensor));
        }


        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "test";
        }
    }

    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            CreateMap<SensorModel, SensorModel>();
        }
    }
}
