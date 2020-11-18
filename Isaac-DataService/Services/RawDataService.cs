using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Isaac_DataService.Services
{
    public class RawDataService : IHostedService
    {
        private readonly IManagedMqttClient inputClient;
        private readonly WriteApiAsync outputClient;
        private readonly List<(string, string, string)> uniqueSensors;
        private readonly string _settingServiceUrl;

        public RawDataService(MqttConnection mqttConnection, FluxConnection influxConnection, IConfiguration config)
        {
            inputClient = mqttConnection.Client;
            ConfigureInfluxOutput(influxConnection).Wait();
            outputClient = influxConnection.Client.GetWriteApiAsync();
            _settingServiceUrl = config.GetValue<string>("Services:Sensorsettings");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            inputClient.UseApplicationMessageReceivedHandler(HandleMessage);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            inputClient.Dispose();
        }

        //Defaults for ensuring the bucket exists
        private async Task ConfigureInfluxOutput(FluxConnection influxConnection)
        {
            await influxConnection.EnsureBucket("sensordata",
                new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, 24 * 60 * 60 * 30));
        }

        //Handles incoming MQTT Messages
        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs arg)
        {
            PointData point = null;
            var splitTopic = arg.ApplicationMessage.Topic.Split("/");

            if (splitTopic[0] == "humtemp")
            {
                try
                {
                    var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                    var floor = splitTopic[1];
                    var x = splitTopic[2];
                    var y = splitTopic[3];
                    if (!uniqueSensors.Contains(
                        (floor, x, y)
                    ))
                    {
                        uniqueSensors.Add((floor, x, y));
                        await HandleNewSensor(floor, x, y);
                    }

                    var topic = splitTopic[5];
                    //splitTopic[4] is called sensor everywhere
                    //Switch on different types of data
                    switch (topic)
                    {
                        case "temperature":
                        case "humidity":
                            //Transform to float and create a point representing the data.
                            float.TryParse(payload, out var value1);
                            if (value1 <= 100) point = CreatePointBase(floor, x, y, topic).Field("value", value1);
                            break;
                        case "uptime":
                            //Transform to long and create a point representing the data.
                            long.TryParse(payload, out var value2);
                            point = CreatePointBase(floor, x, y, topic).Field("value", value2);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                //Write point if message parsing was successful
                if (point != null) await outputClient.WritePointAsync("sensordata", "Isaac", point);
            }
        }

        private async Task HandleNewSensor(string floor, string x, string y)
        {
            using var httpClient = new HttpClient();
            await httpClient.PostAsync(_settingServiceUrl + "/addsensor",
                new StringContent($"{{floor: {floor}, x: {x}, y: {y}}}"));
        }

        private PointData CreatePointBase(string floor, string x, string y, string topic)
        {
            return PointData.Measurement($"sensor{topic}")
                .Tag("floor", floor)
                .Tag("x", x)
                .Tag("y", y);
        }
    }
}