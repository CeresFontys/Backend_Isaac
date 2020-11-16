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

        public SensorDataController(DataOutputService service)
        {
            _service = service;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = new List<FullSensorData>();
            var model = await _service.GatherData(new SensorDataModel());
            var humdata = model.Sensors.Where(data => data.Type==SensorType.Humidity).Cast<HumidityData>();
            var tempdata = model.Sensors.Where(data => data.Type==SensorType.Temperature).Cast<TemperatureData>();
            foreach (TemperatureData temperature in tempdata)
            {
                var humidity = humdata.SingleOrDefault(hum => hum.X == temperature.X && hum.Floor == temperature.Floor && hum.Y == temperature.Y);
                var data = new FullSensorData();
                if (humidity != null)
                {
                    data.Humidity = humidity.Value;
                }
                data.Temperature = temperature.Value;
                data.Floor = temperature.Floor;
                data.X = temperature.X;
                data.Y = temperature.Y;
                list.Add(data);
            }

            return Ok(list);
        }
    }

    public class FullSensorData : SensorData
    {
        public float Temperature { get; set; }
        
        public float Humidity { get; set; }
        
    }
}