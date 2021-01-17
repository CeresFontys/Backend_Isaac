using System;
using System.Collections.Generic;
using System.Linq;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    public class CheckNextSensorLeaf : IOutlierLeaf
    {
        private double maxDifferenceTemp;
        private double maxDifferenceHum;
        private double weatherApiTemp;
        private double weatherApiHum;

        private readonly ValidationComposite validate = new ValidationComposite();

        public CheckNextSensorLeaf(IConfiguration configuration)
        {
           
            maxDifferenceTemp = configuration.GetValue<double>("AlgoConfig:MaxDifferenceTemp");
            maxDifferenceHum = configuration.GetValue<double>("AlgoConfig:MaxDifferenceHum");
        }

        public SensorError Algorithm(SensorData sensor, List<SensorData> sensorDataList)
        {
            var condition = sensorDataList.Where(pos => pos.X == sensor.X && pos.Y == sensor.Y && pos.Type == sensor.Type).ToList();

            if (sensor != condition[^1])
            {

                double parameter = sensor.Type == DataType.Temperature ? maxDifferenceTemp : maxDifferenceHum;
                string id = sensor.X.ToString() + sensor.Y.ToString() + sensor.Floor.ToString() + sensor.DateTime.ToFileTime();
                //Get next sensor
                SensorData sensorNext = condition.SkipWhile(x => x != sensor).Skip(1).DefaultIfEmpty(condition[0]).FirstOrDefault();

                //Calc minute dif
                var minuteDif = (sensorNext.DateTime - sensor.DateTime).TotalMinutes;


                //Flag sensor
                if (!validate.IsValueValid(sensor.Value, sensorNext.Value, minuteDif, parameter))
                {
                    var sensorError = new SensorError();
                    sensorError.id = id;
                    sensorError.X = sensor.X;
                    sensorError.Y = sensor.Y;
                    sensorError.Floor = sensor.Floor;
                    sensorError.DateTime = sensor.DateTime;
                    sensorError.DateTimeNext = sensorNext.DateTime;
                    sensorError.Error = "Difference with next sensor is to big: ";
                    sensorError.ValueFirst = sensor.Value;
                    sensorError.ValueSecond = sensorNext.Value;
                    sensorError.ValueType = sensor.Type.ToString();
                    sensorError.Type = SensorError.ErrorType.NextDif; 
                    return sensorError;
                }

            }

            return null;
        }

    }
}