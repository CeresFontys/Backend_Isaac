using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Unicode;
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
        private FluxConnection _inputConnection;
        private MqttConnection _outputConnection;
        private Thread thread;
        private bool shouldContinue;

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

        private async void Start()
        {
            await Loop(5);
        }

        public async Task Loop(int everySeconds=30)
        {
            shouldContinue = true;
            
            var sensorDataModel = new SensorDataModel();
            while (shouldContinue)
            {
                Stopwatch sw = Stopwatch.StartNew();
                await GatherData(sensorDataModel);
                await PublishData(sensorDataModel);
                Thread.Sleep(TimeSpan.FromSeconds(everySeconds)-sw.Elapsed);
            }
        }

        private async Task GatherData(SensorDataModel model)
        {
            var api = _inputConnection.Client.GetQueryApi();

            var flux = "from(bucket:\"sensordata-downsampled\") |> " +
                       "range(start: -30m) |> " +
                       "group(columns: [\"x\",\"y\",\"floor\"], mode: \"by\") |>" +
                       "last()";

            var temperature = api.QueryAsyncEnumerable<TemperatureData>(flux, CancellationToken.None);
            var humidity = api.QueryAsyncEnumerable<HumidityData>(flux, CancellationToken.None);

            await foreach (var data in temperature)
            {
                model.UpdateSensor(data);
            }
            
            await foreach (var data in humidity)
            {
                model.UpdateSensor(data);
            }
        }

        public async Task PublishData(SensorDataModel model)
        {
            foreach (var data in model.Sensors.OfType<TemperatureData>())
            {
                await PublishData(data.Floor, data.X, data.Y, data.Type.ToString().ToLowerInvariant(), data.Value.ToString());
            }
            
            foreach (var data in model.Sensors.OfType<HumidityData>())
            {
                await PublishData(data.Floor, data.X, data.Y, data.Type.ToString().ToLowerInvariant(), data.Value.ToString());
            }
        }  

        private async Task PublishData(string floor, string x, string y, string type, string data)
        {
            var message = new MqttApplicationMessage();
            message.Topic = $"frontend/{floor}/{x}/{y}/{type}";
            message.Payload = System.Text.Encoding.UTF8.GetBytes(data.ToCharArray());
            message.Retain = true;
            await _outputConnection.Client.PublishAsync(message, CancellationToken.None);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            shouldContinue = false;
        }
    }

    public class SensorDataModel
    {
        public List<SensorData> Sensors = new List<SensorData>();

        public void UpdateSensor(SensorData data)
        {
            var oldData = Sensors.FirstOrDefault(sensorData => sensorData.Floor == data.Floor && sensorData.X == data.X && sensorData.Y == data.Y && sensorData.Type == data.Type);
            
            //Determine if upgrade of data is needed
            if (oldData != null && oldData.Time < data.Time)
            {
                Sensors.Remove(oldData);
                Sensors.Add(data);
            } else if (oldData == null)
            {
                Sensors.Add(data);
            }
            Console.WriteLine("Updated local sensor");
        }
    }

    [Measurement("sensortemperature")]
    public class TemperatureData : SensorData
    {
        [Column("value")] public float Value { get; set; }
        public TemperatureData()
        {
            Type = SensorType.Temperature;
        }
    }
    
    [Measurement("sensorhumidity")]
    public class HumidityData : SensorData
    {
        [Column("value")] public float Value { get; set; }
        public HumidityData()
        {
            Type = SensorType.Humidity;
        }
    }
    
    [Measurement("sensoruptime")]
    public class UptimeData : SensorData
    {
        [Column("value")] public long Value { get; set; }

        public UptimeData()
        {
            Type = SensorType.Uptime;
        }
        
    }
    public abstract class SensorData
    {
        public SensorType Type { get; protected set; }
        [Column("x", IsTag = true)] public string X { get; set; }
        [Column("y", IsTag = true)] public string Y { get; set; }
        [Column("floor", IsTag = true)] public string Floor { get; set; }
        [Column(IsTimestamp = true)] public DateTime Time;
    }

    public enum SensorType
    {
        Temperature, Humidity, Uptime
    }
}