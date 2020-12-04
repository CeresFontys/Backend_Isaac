using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    public class CheckExtremeBottomLeaf : IOutlierLeaf
    {
        private double paramaterTemp;
        private double parameterHum;
        public CheckExtremeBottomLeaf(IConfiguration configuration)
        {
            paramaterTemp = configuration.GetValue<double>("AlgoConfig:ParameterExtremeBottomTemp");
            parameterHum = configuration.GetValue<double>("AlgoConfig:ParameterExtremeBottomHum");
        }
        public SensorError Algorithm(SensorData sensor)
        {
            
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;
            
                if (sensor.Value <= parameter)
                {
                    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds extreme given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.ExtremeBottom, sensor.Value);
                    return sensorError;
                }
            
            return null;
        }
    }
}