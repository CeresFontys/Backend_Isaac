using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Isaac_AnomalyService.Components.Logic
{
    public class CheckTopLeaf : IOutlierLeaf
    {
        private double paramaterTemp;
        private double parameterHum;

        public CheckTopLeaf(IConfiguration configuration)
        {
            paramaterTemp = configuration.GetValue<double>("AlgoConfig:ParameterTopTemp");
            parameterHum = configuration.GetValue<double>("AlgoConfig:ParameterTopHum");
        }
        public SensorError Algorithm(SensorData sensor)
        {
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;

                if (sensor.Value >= parameter)
                {
                    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds normal given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.NormalTop, sensor.Value);
                    return sensorError;
                }
                
            return null;
        }


    }


    //public class CheckNextSensor
    //{
    //    var condition = sensorDataList.Where(pos => pos.X == sensor.X && pos.Y == sensor.Y).ToList();

    //}
}