using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isaac_AnomalyService.Components.Logic.Algoritm;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Isaac_AnomalyService.Components.Services
{
    public class AnomalyService : IHostedService
    {



        private FluxConnection _fluxConnection;
        private WeatherApiConnection _weatherApiConnection;
        private OutlierLeaves _outlierLeaves;

        private int _serviceloopMinutes;
        private bool shouldContinue;
        private Thread thread;
        private readonly ILogger<AnomalyService> _logger;
        private IServiceScopeFactory _scopeFactory;

        
        

        public AnomalyService(FluxConnection fluxConnection, WeatherApiConnection weatherApiConnection, IConfiguration configuration, ILogger<AnomalyService> logger, OutlierLeaves outlierLeaves, IServiceScopeFactory scopeFactory)
        {
            _weatherApiConnection = weatherApiConnection;
            _logger = logger;
            _outlierLeaves = outlierLeaves;
            _scopeFactory = scopeFactory;
            _serviceloopMinutes = configuration.GetValue<int>("LoopParameters:ServiceLoopMinutes");
            _logger.LogWarning("Log Test Anomaly aangemaakt");
        }

        public async Task Loop()
        {

            //await foreach(SensorData sensorData in _fluxConnection.LoadSensorData())
            //{
            //    _OutlierAlgo.SortData(sensorData);
            //}

            List<SensorData> mocksensors = new List<SensorData>();
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 150, X = 1, Y = 1, Type = DataType.Temperature });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 150, X = 1, Y = 1, Type = DataType.Humidity });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 1, X = 1, Y = 1, Type = DataType.Temperature });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 1, X = 1, Y = 1, Type = DataType.Humidity });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 29, X = 1, Y = 1, Type = DataType.Temperature });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 61, X = 1, Y = 1, Type = DataType.Humidity });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 17, X = 1, Y = 1, Type = DataType.Temperature });
            //mocksensors.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 29, X = 1, Y = 1, Type = DataType.Humidity });


            mocksensors.Add(new SensorData() { DateTime = (DateTime.Now), Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Temperature });
            mocksensors.Add(new SensorData() { DateTime = (DateTime.Now).AddSeconds(1), Floor = 3, Value = 24, X = 1, Y = 1, Type = DataType.Temperature });
            mocksensors.Add(new SensorData() { DateTime = (DateTime.Now).AddSeconds(2), Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Temperature });

            mocksensors.Add(new SensorData() { DateTime = DateTime.Now, Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Humidity });
            mocksensors.Add(new SensorData() { DateTime = (DateTime.Now).AddSeconds(1), Floor = 3, Value = 24, X = 1, Y = 1, Type = DataType.Humidity });
            mocksensors.Add(new SensorData() { DateTime = (DateTime.Now).AddSeconds(2), Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Humidity });

            shouldContinue = true;
            while (shouldContinue)
            {
               // _OutlierAlgo.SetWeatherApiData(await _weatherApiConnection.GetWeatherApi());
                _logger.LogWarning("Loop werkt.");
                var sw = Stopwatch.StartNew();
                foreach (SensorData sensor in mocksensors)
                {
                    _outlierLeaves.FillSensorList(sensor);
                }
               

                var list = _outlierLeaves.RunAlgo();
                if (list != null)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<Isaac_AnomalyServiceContext>();
                        await dbContext.AddRangeAsync(list);
                        await dbContext.SaveChangesAsync();
                    }
                }
                

                var time = TimeSpan.FromSeconds(_serviceloopMinutes) - sw.Elapsed;
                if (time < TimeSpan.Zero)
                {
                    time = TimeSpan.Zero;
                }
                Thread.Sleep(time);
            }

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
            await Loop();
        }
    }
}
