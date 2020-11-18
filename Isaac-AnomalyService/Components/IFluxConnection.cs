using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;

namespace Isaac_AnomalyService.Service
{
    public interface IFluxConnection
    {
        InfluxDBClient Client { get; }
    }
}
