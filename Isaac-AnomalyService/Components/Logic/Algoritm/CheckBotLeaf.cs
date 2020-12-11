using System.Collections.Generic;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
        public SensorError Algorithm(SensorData sensor, List<SensorData> sensorDataList)
        {
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;

            if (sensor.Value <= parameter)
            {
                var sensorError = new SensorError();
                sensorError.X = sensor.X;
                sensorError.Y = sensor.Y;
                sensorError.Floor = sensor.Floor;
                sensorError.DateTime = sensor.DateTime;
                sensorError.Error = "Sensor exceeds normal given bottom parameters: ";
                sensorError.ValueFirst = sensor.Value;
                sensorError.Type = SensorError.ErrorType.NormalBottom;
                return sensorError;
            }
            
            return null;
        }


    }
}