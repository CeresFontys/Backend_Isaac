using System.Collections.Generic;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    public class CheckExtremeBottomLeaf : IOutlierLeaf
    {
        private double paramaterTemp;
        private double parameterHum;
        private double weatherApiTemp;
        private double weatherApiHum;
        public CheckExtremeBottomLeaf(IConfiguration configuration)
        {
            
            paramaterTemp = configuration.GetValue<double>("AlgoConfig:ParameterExtremeBottomTemp");
            parameterHum = configuration.GetValue<double>("AlgoConfig:ParameterExtremeBottomHum");
        }
        public SensorError Algorithm(SensorData sensor, List<SensorData> sensorDataList)
        {

            double parameter = sensor.Type == DataType.Temperature ? paramaterTemp : parameterHum;
            string id = sensor.X.ToString() + sensor.Y.ToString() + sensor.Floor.ToString() + sensor.DateTime.ToFileTime();

            if (sensor.Value <= parameter)
            {
                var sensorError = new SensorError();
                sensorError.id = id;
                sensorError.X = sensor.X;
                sensorError.Y = sensor.Y;
                sensorError.Floor = sensor.Floor;
                sensorError.DateTime = sensor.DateTime;
                sensorError.Error = "Sensor exceeds extreme given bottom parameters: ";
                sensorError.ValueFirst = sensor.Value;
                sensorError.ValueType = sensor.Type.ToString();
                sensorError.Type = SensorError.ErrorType.ExtremeBottom;
                return sensorError;
            }
            
            return null;
        }
    }
}