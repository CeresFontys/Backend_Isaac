using System;
using System.Collections.Generic;
using System.Linq;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        public SensorError Algorithm(SensorData sensor, List<SensorData> sensorDataList)
        {
            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;
            string id = sensor.X.ToString() + sensor.Y.ToString() + sensor.Floor.ToString() + sensor.DateTime.ToFileTime();

            if (sensor.Value >= parameter)
            {
                var sensorError = new SensorError();
                sensorError.id = id;
                sensorError.X = sensor.X;
                sensorError.Y = sensor.Y;
                sensorError.Floor = sensor.Floor;
                sensorError.DateTime = sensor.DateTime;
                sensorError.Error = "Sensor exceeds normal given top parameters: ";
                sensorError.ValueFirst = sensor.Value;
                sensorError.ValueType = sensor.Type.ToString();
                sensorError.Type = SensorError.ErrorType.NormalTop;
                return sensorError;
            }
                
            return null;
        }


    }

}