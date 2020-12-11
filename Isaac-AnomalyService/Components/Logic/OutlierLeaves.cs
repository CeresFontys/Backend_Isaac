using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Isaac_AnomalyService.Components.Logic.Algoritm
{
    public class OutlierLeaves
    {
        private readonly List<IOutlierLeaf> _outlierLeaves = new List<IOutlierLeaf>();
        private List<SensorData> sensorDataList = new List<SensorData>();
        private readonly ILogger<OutlierLeaves> _logger;


        public OutlierLeaves(IConfiguration configuration, ILogger<OutlierLeaves> logger)
        {
            _logger = logger;
            _outlierLeaves.Add(new CheckExtremeTopLeaf(configuration));
            _outlierLeaves.Add(new CheckExtremeBottomLeaf(configuration)); 
            _outlierLeaves.Add(new CheckTopLeaf(configuration));
            _outlierLeaves.Add(new CheckBotLeaf(configuration));
            _outlierLeaves.Add(new CheckNextSensorLeaf(configuration));
        }

        public void FillSensorList(SensorData sensor)
        {
            sensorDataList.Add(sensor);
        }

        public List<SensorError>RunAlgo()
        {
            var list = new List<SensorError>();
            foreach (SensorData sensor in sensorDataList)
            {
                var sensorError = CheckSensorErrors(sensor);
                if (sensorError != null)
                {
                    list.Add(sensorError);
                }
            }
            
            return list;
        }

        private SensorError CheckSensorErrors(SensorData sensor)
        {
            
            foreach (var leaf in _outlierLeaves)
            {
                var result = leaf.Algorithm(sensor, sensorDataList);

                if (result != null)
                {
                    _logger.LogWarning(result.Error);
                    return result;
                }
            }

            return null;
        }


    }
}
