using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_SensorSettingService.Data;
using Isaac_SensorSettingService.Models;

namespace Isaac_SensorSettingService.Compontents
{
    public class SensorService
    {
        private readonly DataContext _dbContext;
        public SensorService(DataContext dbContext)
        {
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

        public void UpdateSensor(SensorModel sensor)
        {
            _dbContext.Sensors.Update(sensor);
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
