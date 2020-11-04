using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_SensorSettingService.Compontents;
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
        public async Task<string> CreateTask()
        {
          await _settingsService.CreateTask();
            return "test";
        }
    }
}

