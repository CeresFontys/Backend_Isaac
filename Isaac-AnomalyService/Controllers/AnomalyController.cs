using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
using Isaac_AnomalyService.Components;
using Isaac_AnomalyService.Components.Logic;
using Isaac_AnomalyService.Components.Services;
using Isaac_AnomalyService.Logic;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Isaac_AnomalyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnomalyController : ControllerBase
    {
        private AnomalyService _anomalyService;
        private FluxConnection _fluxConnection;
        private WeatherApiConnection _weatherApi;


        public AnomalyController( AnomalyService anomalyService, FluxConnection fluxConnection, WeatherApiConnection weatherApi)
        {
            _anomalyService = anomalyService;
            _fluxConnection = fluxConnection;
            _weatherApi = weatherApi;
        }

        // GET: api/<AnomalyController>
        [HttpGet]
        public async Task Get()
        {
            
        }

        // GET api/<AnomalyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("test")]
        public async IAsyncEnumerable<string> GetList(int id)
        {
            await foreach (SensorData sensorData in _fluxConnection.LoadSensorData())
            {
                yield return JsonConvert.SerializeObject(sensorData);
            }
        }

        //[HttpGet("testweather")]
        //public async Task<WeatherApiData> GetWeather()
        //{
        //    return await _weatherApi.GetWeatherApi();
        //}


        // POST api/<AnomalyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AnomalyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AnomalyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
