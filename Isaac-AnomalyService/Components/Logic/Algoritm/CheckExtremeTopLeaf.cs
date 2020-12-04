using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    public class CheckExtremeTopLeaf : IOutlierLeaf
    {
        private double paramaterTemp;
        private double parameterHum;
        public CheckExtremeTopLeaf(IConfiguration configuration)
        {
            paramaterTemp = configuration.GetValue<double>("AlgoConfig:ParameterExtremeTopTemp");
            parameterHum = configuration.GetValue<double>("AlgoConfig:ParameterExtremeTopHum");
        }
        public SensorError Algorithm(SensorData sensor)
        {
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;

                if (sensor.Value >= parameter)
                {
                    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds extreme given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.ExtremeTop, sensor.Value);
                    return sensorError;
                }

            return null;
        }

        
    }

    
}
