using InfluxDB.Client;

namespace Isaac_API.Services
{
    public interface IFluxConnection
    {
        InfluxDBClient Client { get; }
    }
}