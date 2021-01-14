using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Isaac_SensorSettingService.Controllers;
using Isaac_SensorSettingService.Data;
using Isaac_SensorSettingService.Models;

namespace Isaac_SensorSettingService.Compontents
{
    public class SensorService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public SensorService(DataContext dbContext)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<SensorProfile>();
            });
            _mapper = configuration.CreateMapper();
            
            _dbContext = dbContext;
        }
        public string AddSensor(SensorModel sensor)
        {
            try
            {
                _dbContext.Sensors.Add(sensor);
                _dbContext.SaveChanges();
                return "sensor added";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<bool> UpdateSensor(SensorModel sensor)
        {
          var foundSensor = await _dbContext.FindAsync<SensorModel>(sensor.Id); 
          _mapper.Map(sensor, foundSensor);
          
          await _dbContext.SaveChangesAsync();
          return true;
        }

        public async Task<bool> DeleteSensor(int id)
        {
            var foundSensor = await _dbContext.FindAsync<SensorModel>(id);
            if (foundSensor != null)
            {
                _dbContext.Remove(foundSensor);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public SensorModel GetSensor(string floor, int x, int y)
        { 
            return _dbContext.Sensors.Find(floor,x,y);
        }
        public List<SensorModel> GetSensors()
        {
            return _dbContext.Sensors.ToList();
        }

        public List<SensorModel> GetSensorsByFloor(string floor)
        {
            return _dbContext.Sensors.Where(s => s.Floor == floor).ToList();
        }
    }
}
