using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Isaac_DataService.Components.Connections
{
    public class MqttConnection : IMqttConnection
    {
        private readonly ManagedMqttClientOptions options;

        public MqttConnection(IConfiguration configuration)
        {
            //Credentials uit configuratie halen
            var username = configuration.GetValue<string>("MQTT:Username");
            var password = configuration.GetValue<string>("MQTT:Password");
            var address = configuration.GetValue<string>("MQTT:IP");
            var port = configuration.GetValue<int>("MQTT:Port");

            //Client Opties
            options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("NetCoreClient")
                    .WithTcpServer(address, port)
                    .WithCredentials(username, password)
                    .Build()).Build();
            Client = new MqttFactory().CreateManagedMqttClient();
        }

        public IManagedMqttClient Client { get; }

        public async Task StartListen()
        {
            await Client.SubscribeAsync(new MqttTopicFilterBuilder().WithExactlyOnceQoS().WithTopic("#").Build());
            await Client.StartAsync(options);
        }
        
        public async Task StopListen()
        {
            if (Client.IsStarted)
            {
                await Client.StopAsync();
            }
        }
    }
}