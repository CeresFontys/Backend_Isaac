using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Logic;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Newtonsoft.Json;

namespace Isaac_AnomalyService.Components.Services
{
    public class AnomalyService
    {

        private FluxConnection _fluxConnection;

        private OutlierAlgo _detectionAlgo;


        public AnomalyService(FluxConnection fluxConnection, OutlierAlgo detectionAlgo)
        {
            _fluxConnection = fluxConnection;
            _detectionAlgo = detectionAlgo;
        }

        public async Task Get()
        {

            await foreach(SensorData sensorData in _fluxConnection.LoadSensorData())
            { 
               await _detectionAlgo.SortData(sensorData);
            }

            await _detectionAlgo.RunOutlierAlgo();

        }
    }
}
