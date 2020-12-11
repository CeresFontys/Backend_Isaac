using Isaac_SensorSettingService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_SensorSettingService.Models;

namespace Isaac_SensorSettingService.Compontents
{
    public class GroupService
    {
        private readonly DataContext _dbContext;
        public GroupService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string AddGroup(SensorGroupModel group)
        {
            try
            {
                _dbContext.Group.Add(group);
                _dbContext.SaveChanges();
                return "Group added";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void UpdateGroup(SensorGroupModel group)
        {
            _dbContext.Group.Update(group);
        }

        public List<SensorGroupModel> GetGroups()
        {
            return _dbContext.Group.ToList();
        }

        public List<SensorGroupModel> GetGroupsByFloor(string floor)
        {
            return _dbContext.Group.Where(s => s.Floor == floor).ToList();
        }
    }
}
