using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isaac_AnomalyService.Components.Logic.Algoritm;
using Isaac_AnomalyService.Controllers;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        private SqlConnection _sqlConnection;

        private int _serviceloopMinutes;
        private bool shouldContinue;
        private Thread thread;
        private readonly ILogger<AnomalyService> _logger;
        private IServiceScopeFactory _scopeFactory;
        private IHubContext<ErrorHub> _errorHub;

        private bool reloop = false;



        private Random random = new Random();
        private List<SensorData> Mockdata = new List<SensorData>();

        



        public AnomalyService(FluxConnection fluxConnection, WeatherApiConnection weatherApiConnection, IConfiguration configuration, ILogger<AnomalyService> logger, OutlierLeaves outlierLeaves, IServiceScopeFactory scopeFactory, IHubContext<ErrorHub> errorHub, SqlConnection sqlConnection)
        {
            _fluxConnection = fluxConnection;
            _weatherApiConnection = weatherApiConnection;
            _logger = logger;
            _outlierLeaves = outlierLeaves;
            _scopeFactory = scopeFactory;
            _errorHub = errorHub;
            _sqlConnection = sqlConnection;
            _serviceloopMinutes = configuration.GetValue<int>("LoopParameters:ServiceLoopMinutes");
            _logger.LogWarning("Log Test Anomaly aangemaakt");
        }

        public async Task Loop()
        {
            //Mockdata.Clear();

            //for (int i = 0; i < 50; i++)
            //{
            //    Mockdata.Add(new SensorData() { DateTime = DateTime.Now, Floor = 3, Value = random.Next(5, 35), X = random.Next(0, 3), Y = random.Next(0, 3), Type = DataType.Temperature });
            //    Mockdata.Add(new SensorData() { DateTime = DateTime.Now, Floor = 3, Value = random.Next(5, 120), X = random.Next(0, 3), Y = random.Next(0, 3), Type = DataType.Humidity });
            //}




            var list = new List<SensorError>();

            shouldContinue = true;
            while (shouldContinue)
            {
                _outlierLeaves.SetWeatherApiData(await _weatherApiConnection.GetWeatherApi());
                _logger.LogWarning("Loop werkt.");
                var sw = Stopwatch.StartNew();
                _outlierLeaves.ClearSensorList();
                foreach (SensorData sensorData in Mockdata)
                {
                    _outlierLeaves.FillSensorList(sensorData);
                }
                //await foreach (SensorData sensorData in _fluxConnection.LoadSensorData())
                //{
                //    _outlierLeaves.FillSensorList(sensorData);
                //}


                list?.Clear();
                list = _outlierLeaves.RunAlgo();
                if (list != null)
                {
                    await _sqlConnection.SaveData(list);
                    await _errorHub.Clients.All.SendAsync("ReceiveErrors", list);
                }



                var time = TimeSpan.FromMinutes(_serviceloopMinutes) - sw.Elapsed;
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
