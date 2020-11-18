using InfluxDB.Client;

namespace Isaac_DataService.Components.Connections
{
    public interface IFluxConnection
    {
        InfluxDBClient Client { get; }
    }
}