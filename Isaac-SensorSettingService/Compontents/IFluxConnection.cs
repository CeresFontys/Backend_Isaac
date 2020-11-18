using InfluxDB.Client;

namespace Isaac_DataService.Services
{
    public interface IFluxConnection
    {
        InfluxDBClient Client { get; }
    }
}