using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json.Serialization;

namespace Isaac_API.Services
{
    public interface IMqttConnection
    {
        IManagedMqttClient Client { get; }
    }

    public class MqttConnection : IMqttConnection
    {
        public IManagedMqttClient Client
        {
            get { return client; }
        }

        private IManagedMqttClient client;
        private ManagedMqttClientOptions options;
        
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
                    .WithTls().Build()).Build();
            client = new MqttFactory().CreateManagedMqttClient();
            StartClient().Wait();
        }

        public async Task StartClient()
        {
            if (!client.IsStarted)
            {
                await client.SubscribeAsync(new MqttTopicFilterBuilder().WithExactlyOnceQoS().WithTopic("#").Build());
                await client.StartAsync(options);
            }
        }
    }
}