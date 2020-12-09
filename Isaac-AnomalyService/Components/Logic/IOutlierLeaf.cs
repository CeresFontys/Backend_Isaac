using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;

namespace Isaac_AnomalyService.Components.Logic
{
    public interface IOutlierLeaf
    {
        public SensorError Algorithm(SensorData sensor, List<SensorData> sensorDataList);

    }
}
