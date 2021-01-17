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
        [Route("/{floor}")]
        public List<SensorGroupModel> GetGroupsByFloor(string floor)
        {
            return _groupService.GetGroupsByFloor(floor);
        }
        [HttpGet]
        public List<SensorGroupModel> GetAllGroups()
        {
            return _groupService.GetGroups();
        }

        [HttpPost]
        public SensorGroupModel AddGroup([FromBody] SensorGroupModel group)
        {
            return _groupService.AddGroup(group);
        }

        [HttpDelete]
        [Route("test/{id}")]
        public void RemoveGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "test";
        }
    }
}
