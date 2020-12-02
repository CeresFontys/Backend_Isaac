using System;
using System.Drawing;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace Isaac_DataService.Components.Connections
{
    public interface IFluxConnection : IDisposable
    {
        public Task WritePointAsync(PointData data);
        public Task<Ready> ReadyAsync();

        public Task SetBucket(string name);
    }
}