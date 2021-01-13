using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Isaac_DataService.Services
{
    public class DataService : IHostedService, IDisposable
    {
        private readonly InfluxService _influx;
        private readonly QueryApi _queryApi;
        private readonly SensorDataModel _model;
        private readonly MqttConnection _inputClient;
        private readonly string _sourceBucket;
        private readonly string _sourceTopic;
        private readonly string _rawBucket;
        private readonly List<(string, string, string)> _uniqueSensors;
        private readonly ILogger<DataService> _logger;

        public DataService(InfluxService influx, IFluxConnection flux, IConfiguration configuration, ILogger<DataService> logger, MqttConnection mqtt)
        {
            _influx = influx;
            _logger = logger;
            _uniqueSensors = new List<(string, string, string)>();
            _queryApi = ((FluxConnection)flux).Client.GetQueryApi();
            _model = new SensorDataModel();
            _sourceBucket = configuration.GetValue<string>("Influx:BucketNameDownsampled");
            _sourceTopic = configuration.GetValue<string>("MQTT:Topic");
            _inputClient = mqtt;
            _rawBucket = configuration.GetValue<string>("Influx:BucketName");
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _influx.Initialize(_rawBucket);
            _inputClient.Client.UseApplicationMessageReceivedHandler(HandleMqttMessage);
            await _inputClient.Client.SubscribeAsync("#");
            await _inputClient.StartListen();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _inputClient.StopListen();
        }

        public async Task<SensorDataModel> GatherData()
        {
            var temperature = _queryApi.QueryAsyncEnumerable<TemperatureData>(
                $@"from(bucket:""{_sourceBucket}"") |> " +
                @"range(start: -2h) |> " +
                @"filter(fn: (r) => r._measurement == ""sensorTemperature"" )|> " +
                @"group(columns: [""x"",""y"",""floor""], mode: ""by"") |>" +
                @"last()"
                , CancellationToken.None);
            
            var humidity = _queryApi.QueryAsyncEnumerable<HumidityData>(
                $@"from(bucket:""{_sourceBucket}"") |> " +
                @"range(start: -2h) |> " +
                @"filter(fn: (r) => r._measurement == ""sensorHumidity"" )|> " + 
                @"group(columns: [""x"",""y"",""floor""], mode: ""by"") |>" +
                @"last()"
                , CancellationToken.None);

            await foreach (var data in temperature) await _model.UpdateSensor(data);

            await foreach (var data in humidity) await _model.UpdateSensor(data);

            return _model;
        }
        
        private async Task HandleMqttMessage(MqttApplicationMessageReceivedEventArgs arg)
        {
            var splitTopic = arg.ApplicationMessage.Topic.Split("/");

            if (splitTopic[0] == _sourceTopic)
            {
                try
                {
                    var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                    var floor = splitTopic[1];
                    var x = splitTopic[2];
                    var y = splitTopic[3];
                    var topic = splitTopic[5];
                    
                    await HandleMessage(floor, x, y, topic, payload);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred when processing message from mqtt");
                }
            }
        }

        private async Task HandleMessage(string floor, string x, string y, string topic, string payload)
        {
            CheckNewSensor(floor, x, y, topic);
            
            switch (topic)
            {
                case "temperature":
                    if (float.TryParse(payload, out var temperature))
                    {
                        //await UploadData(new TemperatureData(temperature/10f, x, y, floor));
                        await UploadData(new TemperatureData(temperature, x, y, floor));
                        break;
                    }
                    HandleParsingError(temperature);
                    break;
                case "humidity":
                    if (float.TryParse(payload, out var humidity))
                    {
                        //await UploadData(new HumidityData(humidity/10f, x, y, floor));
                        await UploadData(new HumidityData(humidity, x, y, floor));
                        break;
                    }
                    HandleParsingError(humidity);
                    break;
                case "uptime":
                    if (long.TryParse(payload, out var uptime))
                    {
                        await UploadData(new UptimeData(uptime, x, y, floor));
                        break;
                    }
                    HandleParsingError(uptime);
                    break;
            }
        }

        private void HandleParsingError(in long value)
        {
            _logger.LogError($"Error occured when parsing {value} as long");
        }
        
        private void HandleParsingError(in float value)
        {
            _logger.LogError($"Error occured when parsing {value} as float");
        }

        private void CheckNewSensor(string floor, string x, string y, string topic)
        {
            if (!_uniqueSensors.Contains((floor, x, y)))
            {
                _uniqueSensors.Add((floor, x, y));
                //TODO Request to other service to make sure they have an updated list of sensors
            }
        }

        private async Task<bool> UploadData<TData>(TData data) where TData : SensorData
        {
            var point = CreatePoint(data);
            return await _influx.UploadPoint(point);
        }

        private PointData CreatePoint<TData>(TData data) where TData : SensorData
        {
            var pointBase = CreatePointBase(data.Floor, data.X, data.Y, data.Type.ToString());

            return data switch
            {
                UptimeData uptime => pointBase.Field("value", uptime.Value),
                
                HumidityData humidity => pointBase.Field("value", humidity.Value),
                
                TemperatureData temperature => pointBase.Field("value", temperature.Value),
                
                _ => throw new InvalidSensorDataException("Unexpected type received: " + typeof(TData).Name)
            };
        }
        
        private PointData CreatePointBase(string floor, string x, string y, string topic)
        {
            return PointData.Measurement($"sensor{topic}")
                .Tag("floor", floor)
                .Tag("x", x)
                .Tag("y", y);
        }

        public void Dispose()
        {
            _influx?.Dispose();
        }
    }
}