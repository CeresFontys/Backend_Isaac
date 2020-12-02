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
    }
}