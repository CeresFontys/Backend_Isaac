using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    internal class CheckBotLeaf : IOutlierLeaf
    {
        private double paramaterTemp;
        private double parameterHum;
        public CheckBotLeaf(IConfiguration configuration)
        {
            paramaterTemp = configuration.GetValue<double>("AlgoConfig:ParameterBottomTemp");
            parameterHum = configuration.GetValue<double>("AlgoConfig:ParameterBottomHum");
        }
        public SensorError Algorithm(SensorData sensor)
        {
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;

            if (sensor.Value <= parameter)
            {
                SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds normal given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.NormalBottom, sensor.Value);
                return sensorError;
            }
            
            return null;
        }


    }
}