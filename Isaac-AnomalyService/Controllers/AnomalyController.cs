using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
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

        private FluxConnection _fluxConnection;

        public AnomalyController(FluxConnection fluxConnection){
            _fluxConnection = fluxConnection;
        }

        // GET: api/<AnomalyController>
        [HttpGet]
        public async IAsyncEnumerable<string> Get()
        {
            await foreach(SensorData sensorData in _fluxConnection.LoadSensorData())
            {
                yield return JsonConvert.SerializeObject(sensorData);
            }
        }

        // GET api/<AnomalyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

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
