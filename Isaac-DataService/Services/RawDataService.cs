using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Isaac_DataService.Services
{
    public class RawDataService : IHostedService
    {
        private readonly IManagedMqttClient inputClient;
        private readonly WriteApiAsync outputClient;

        public RawDataService(MqttConnection mqttConnection, FluxConnection influxConnection)
        {
            inputClient = mqttConnection.Client;
            ConfigureInfluxOutput(influxConnection).Wait();
            outputClient = influxConnection.Client.GetWriteApiAsync();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            inputClient.UseApplicationMessageReceivedHandler(HandleMessage);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            inputClient.Dispose();
        }

        private async Task ConfigureInfluxOutput(FluxConnection influxConnection)
        {
            await influxConnection.EnsureBucket("sensordata",
                new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, 24 * 60 * 60 * 30));
        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs arg)
        {
            PointData point = null;
            var splitTopic = arg.ApplicationMessage.Topic.Split("/");
            
            if (splitTopic[0] == "humtemp")
            {
                var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                var floor = splitTopic[1];
                var x = splitTopic[2];
                var y = splitTopic[3];
                //splitTopic[4] is called sensor everywhere
                //Switch on different types of data
                switch (splitTopic[5])
                {
                    case "temperature":
                        //Transform to float and create a point representing the data.
                        float.TryParse(payload, out var temperature);
                        point = PointData.Measurement("sensortemperature")
                            .Tag("floor", floor)
                            .Tag("x", x)
                            .Tag("y", y)
                            .Field("value", temperature);
                        break;
                    case "humidity":
                        float.TryParse(payload, out var humidity);
                        point = PointData.Measurement("sensorhumidity")
                            .Tag("floor", floor)
                            .Tag("x", x)
                            .Tag("y", y)
                            .Field("value", humidity);
                        break;
                    case "uptime":
                        long.TryParse(payload, out var uptime);
                        point = PointData.Measurement("sensoruptime")
                            .Tag("floor", floor)
                            .Tag("x", x)
                            .Tag("y", y)
                            .Field("value", uptime);
                        break;
                }

                if (point != null) await outputClient.WritePointAsync("sensordata", "Isaac", point);
            }
        }
    }
}