using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
using Isaac_SensorSettingService.Compontents;
using Isaac_SensorSettingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Isaac_SensorSettingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorSettingController : ControllerBase
    {
        private readonly SettingsService _settingsService;
      
        public SensorSettingController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        [HttpGet]
        [Route("CreateTask")]
        public async Task<string> CreateTask(string bucketName, string taskName, string floor)
        {
            await _settingsService.CreateTask(bucketName, taskName, floor);
            return "test";
        }

    }
}

