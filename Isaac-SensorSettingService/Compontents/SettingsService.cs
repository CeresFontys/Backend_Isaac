using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Api.Service;
using InfluxDB.Client.Core;
using Isaac_DataService.Services;
using Microsoft.Extensions.Configuration;

namespace Isaac_SensorSettingService.Compontents
{
    public class SettingsService
    {
        private readonly TasksApi _tasksApi;
        private readonly FluxConnection _fluxConnection;
        private readonly string orgId;


        public SettingsService(FluxConnection fluxConnection, IConfiguration configuration)
        {
            _fluxConnection = fluxConnection;
            _tasksApi = _fluxConnection.Client.GetTasksApi();

            orgId = configuration.GetValue<string>("Influx:Organisation");

        }


        public async Task CreateTask()
        {
           await _fluxConnection.EnsureBucket("sensordata-downsampled",null);

           var flux =
               "from(bucket: 'sensordata') |> range(start: -task.every) |> filter(fn: (r) => r._measurement == 'sensor_temperature' ) |> aggregateWindow( every: 5m, fn: mean ) |> to(bucket: 'sensordata-downsampled')";
            

           await _tasksApi.CreateTaskCronAsync("down-sample-task", flux, "0 0 * * *", orgId);

        }

    }
}
