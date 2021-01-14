using Isaac_SensorSettingService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Isaac_SensorSettingService.Models;

namespace Isaac_SensorSettingService.Compontents
{
    public class GroupService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public GroupService(DataContext dbContext)
        {
            _dbContext = dbContext;
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<GroupProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        public SensorGroupModel AddGroup(SensorGroupModel group)
        {
            try
            {
                _dbContext.Group.Add(group);
                _dbContext.SaveChanges();
                return group;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void UpdateGroup(SensorGroupModel group)
        {
            var foundSensor =  _dbContext.FindAsync<SensorGroupModel>(group.Id);
            _mapper.Map(group, foundSensor);
            _dbContext.SaveChangesAsync();
        }

        public void DeleteGroup(int id)
        {
            var foundGroup = _dbContext.FindAsync<SensorGroupModel>(id);
            _dbContext.Remove(foundGroup); 
            _dbContext.SaveChangesAsync();
      
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
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<SensorGroupModel, SensorGroupModel>();
        }
    }
}
