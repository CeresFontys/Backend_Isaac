using MQTTnet.Extensions.ManagedClient;

namespace Isaac_DataService.Components.Connections
{
    public interface IMqttConnection
    {
        IManagedMqttClient Client { get; }
    }
}