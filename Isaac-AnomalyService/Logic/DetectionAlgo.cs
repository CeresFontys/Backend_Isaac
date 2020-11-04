using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;

namespace Isaac_AnomalyService.Logic
{
    public class DetectionAlgo
    {

        private FluxConnection _fluxConnection;

        public DetectionAlgo(FluxConnection fluxConnection)
        {
            _fluxConnection = fluxConnection;
        }

        public async Task DetectOutliers()
        {
            await foreach (SensorData sensor in _fluxConnection.LoadSensorData())
            {
                
            }
        }
    }
}
