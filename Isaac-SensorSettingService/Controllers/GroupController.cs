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
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;
        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Route("groups/{floor}")]
        public List<SensorGroupModel> GetGroupsByFloor(string floor)
        {
            return _groupService.GetGroupsByFloor(floor);
        }
        [HttpGet]
        [Route("groups")]
        public List<SensorGroupModel> GetAllGroups()
        {
            return _groupService.GetGroups();
        }

        [HttpPost]
        [Route("add")]
        public string AddSensor([FromBody] SensorGroupModel group)
        {
            return _groupService.AddGroup(group);
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "test";
        }
    }
}
