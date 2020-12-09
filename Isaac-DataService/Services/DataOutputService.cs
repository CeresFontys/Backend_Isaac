using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;

namespace Isaac_DataService.Services
{
    public class DataOutputService : IHostedService
    {
        private readonly DataService _inputService;
        private readonly MqttConnection _outputConnection;
        private bool _shouldContinue;
        private Thread _thread;

        public DataOutputService(DataService dataService, MqttConnection outputConnection, IConfiguration config)
        {
            _inputService = dataService;
            _outputConnection = outputConnection;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _thread = new Thread(Start);
            _thread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _shouldContinue = false;
            return Task.CompletedTask;
        }

        private async void Start()
        {
            await Loop(5);
        }

        private async Task Loop(int everySeconds = 30)
        {
            _shouldContinue = true;
            
            while (_shouldContinue)
            {
                var sw = Stopwatch.StartNew();
                var sensorDataModel = await _inputService.GatherData();
                await PublishData(sensorDataModel);
                if (sw.Elapsed <= TimeSpan.FromSeconds(everySeconds))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(everySeconds) - sw.Elapsed);
                }
            }
        }

        private async Task PublishData(SensorDataModel model)
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
            var message = new MqttApplicationMessage
            {
                Topic = $"frontend/{floor}/{x}/{y}/{type}",
                Payload = Encoding.UTF8.GetBytes(data.ToCharArray()),
                Retain = true
            };
            await _outputConnection.Client.PublishAsync(message, CancellationToken.None);
        }
    }
}