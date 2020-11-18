using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client.Core;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Hosting;
using MQTTnet;

namespace Isaac_DataService.Services
{
    public class DataOutputService : IHostedService
    {
        private readonly FluxConnection _inputConnection;
        private readonly MqttConnection _outputConnection;
        private bool shouldContinue;
        private Thread thread;

        public DataOutputService(FluxConnection inputConnection, MqttConnection outputConnection)
        {
            _inputConnection = inputConnection;
            _outputConnection = outputConnection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            thread = new Thread(Start);
            thread.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            shouldContinue = false;
        }

        private async void Start()
        {
            await Loop(5);
        }

        public async Task Loop(int everySeconds = 30)
        {
            shouldContinue = true;

            var sensorDataModel = new SensorDataModel();
            while (shouldContinue)
            {
                var sw = Stopwatch.StartNew();
                sensorDataModel = await GatherData(sensorDataModel);
                await PublishData(sensorDataModel);
                Thread.Sleep(TimeSpan.FromSeconds(everySeconds) - sw.Elapsed);
            }
        }

        public async Task<SensorDataModel> GatherData(SensorDataModel model)
        {
            var api = _inputConnection.Client.GetQueryApi();

            var fluxtemp = "from(bucket:\"sensordata-downsampled\") |> " +
                           "range(start: -2h) |> " +
                           "filter(fn: (r) => r._measurement == \"sensortemperature\" )|> " +
                           "group(columns: [\"x\",\"y\",\"floor\"], mode: \"by\") |>" +
                           "last()";
            var fluxhum = "from(bucket:\"sensordata-downsampled\") |> " +
                          "range(start: -2h) |> " +
                          "filter(fn: (r) => r._measurement == \"sensorhumidity\" )|> " +
                          "group(columns: [\"x\",\"y\",\"floor\"], mode: \"by\") |>" +
                          "last()";

            var temperature = api.QueryAsyncEnumerable<TemperatureData>(fluxtemp, CancellationToken.None);
            var humidity = api.QueryAsyncEnumerable<HumidityData>(fluxhum, CancellationToken.None);

            await foreach (var data in temperature) await model.UpdateSensor(data);

            await foreach (var data in humidity) await model.UpdateSensor(data);

            return model;
        }

        public async Task PublishData(SensorDataModel model)
        {
            foreach (var data in model.Sensors.OfType<TemperatureData>())
                await PublishData(data.Floor, data.X, data.Y, data.Type.ToString().ToLowerInvariant(),
                    data.Value.ToString());

            foreach (var data in model.Sensors.OfType<HumidityData>())
                await PublishData(data.Floor, data.X, data.Y, data.Type.ToString().ToLowerInvariant(),
                    data.Value.ToString());
        }

        private async Task PublishData(string floor, string x, string y, string type, string data)
        {
            var message = new MqttApplicationMessage();
            message.Topic = $"frontend/{floor}/{x}/{y}/{type}";
            message.Payload = Encoding.UTF8.GetBytes(data.ToCharArray());
            message.Retain = true;
            await _outputConnection.Client.PublishAsync(message, CancellationToken.None);
        }
    }

    public class SensorDataModel
    {
        public List<SensorData> Sensors = new List<SensorData>();

        public async Task UpdateSensor(SensorData data)
        {
            var oldData = Sensors.FirstOrDefault(sensorData =>
                sensorData.Floor == data.Floor && sensorData.X == data.X && sensorData.Y == data.Y &&
                sensorData.Type == data.Type);

            //Determine if upgrade of data is needed
            if (oldData != null && oldData.Time < data.Time)
            {
                Sensors.Remove(oldData);
                Sensors.Add(data);
            }
            else if (oldData == null)
            {
                Sensors.Add(data);
            }
        }
    }

    [Measurement("sensortemperature")]
    public class TemperatureData : SensorData
    {
        public TemperatureData()
        {
            Type = SensorType.Temperature;
        }

        [Column("value")] public float Value { get; set; }
    }

    [Measurement("sensorhumidity")]
    public class HumidityData : SensorData
    {
        public HumidityData()
        {
            Type = SensorType.Humidity;
        }

        [Column("value")] public float Value { get; set; }
    }

    [Measurement("sensoruptime")]
    public class UptimeData : SensorData
    {
        public UptimeData()
        {
            Type = SensorType.Uptime;
        }

        [Column("value")] public long Value { get; set; }
    }

    public abstract class SensorData
    {
        [Column(IsTimestamp = true)] public DateTime Time;
        public SensorType Type { get; protected set; }
        [Column("x", IsTag = true)] public string X { get; set; }
        [Column("y", IsTag = true)] public string Y { get; set; }
        [Column("floor", IsTag = true)] public string Floor { get; set; }
    }

    public enum SensorType
    {
        Temperature,
        Humidity,
        Uptime
    }
}