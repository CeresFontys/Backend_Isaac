using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Api.Service;
using InfluxDB.Client.Core;
using Isaac_DataService.Services;
using Isaac_SensorSettingService.Data;
using Isaac_SensorSettingService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Isaac_SensorSettingService.Compontents
{
    public class SettingsService
    {
        private readonly TasksApi _tasksApi;
        private readonly FluxConnection _fluxConnection;
        private readonly string _orgId;
        private readonly SettingContext _dbContext;


        public SettingsService(FluxConnection fluxConnection, IConfiguration configuration, SettingContext dbContext)
        {
            _fluxConnection = fluxConnection;
            _tasksApi = _fluxConnection.Client.GetTasksApi();
            _orgId = configuration.GetValue<string>("Influx:Organisation");
            _dbContext = dbContext;
        }


        public async Task CreateTask(string bucketName, string taskName, string floor)
        {
            
           await _fluxConnection.EnsureBucket(bucketName, null);

           if (!await TaskExists(taskName))
           {
               SettingsModel settings = new SettingsModel()
               {
                   Id = floor,
                   RefreshRate = "5m",
                   KeepData = true
               };
               var flux =
                   "option task = {\n\tname: \""+ taskName + "\"," +
                   "\n\tevery: "+ settings.RefreshRate + "," +
                   "\n\toffset: 0m," +
                   "\n\tconcurrency: 1," +
                   "\n\tretry: 5,\n}" +
                   "\n\ndata = from(bucket: \"sensordata\")\n\t|> " +
                   "range(start: -duration(v: int(v: task.every) * 2))\n\t|> " +
                   "filter(fn: (r) =>\n\t\t(r._measurement == \"sensortemperature\" or r._measurement == \"sensorhumidity\" and r.floor == \"" + floor + "\"))" +
                   "\n\ndata\n\t|> aggregateWindow(every: " + settings.RefreshRate + ", fn: mean)\n\t|>" +
                   " to(bucket: \""+bucketName+"\")";
           
               var task = new TaskCreateRequest("", _orgId, null, TaskStatusType.Active, flux, "task");
               var taskType = await _tasksApi.CreateTaskAsync(task);

               _dbContext.Settings.Add(settings);
           }
        }
        public async Task UpdateTask(SettingsModel settings)
        {
            await _fluxConnection.EnsureBucket("sensordata-downsampled", null);

            if (await TaskExists("downsampler"))
            {
                var flux =
                    "option task = {\n\tname: \"downsampler\",\n\tevery: 1h,\n\toffset: 0m,\n\tconcurrency: 1,\n\tretry: 5,\n}\n\ndata = from(bucket: \"sensordata\")\n\t|> range(start: -duration(v: int(v: task.every) * 2))\n\t|> filter(fn: (r) =>\n\t\t(r._measurement == \"sensortemperature\" or r._measurement == \"sensorhumidity\"))\n\ndata\n\t|> aggregateWindow(every: 1h, fn: mean)\n\t|> to(bucket: \"sensordata-downsampled\")";

                var request = new TaskUpdateRequest(TaskStatusType.Active, flux);

                var taskType = await _tasksApi.UpdateTaskAsync("downsampler", request);
            }
        }

        public async Task<bool> TaskExists(string name)
        { 
            return (await _tasksApi.FindTasksAsync(orgId: _orgId)).Where((type => type.Name == "downsampler")).Any();

        }
    }
}
