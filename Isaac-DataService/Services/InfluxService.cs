using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Isaac_DataService.Components.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Isaac_DataService.Services
{
    public class InfluxService : IDisposable
    {
        private readonly IFluxConnection _connection;
        private readonly ILogger<InfluxService> _logger;
        private readonly ConcurrentQueue<PointData> _queue;

        public IReadOnlyCollection<PointData> QueueData => _queue.ToArray();
       

        public InfluxService(IFluxConnection connection, ILogger<InfluxService> logger, IEnumerable<PointData> data = null)
        {
            _connection = connection;
            _logger = logger;
            
            if (data != null)
            {
                _queue = new ConcurrentQueue<PointData>(data);
            }
            else
            {
                _queue = new ConcurrentQueue<PointData>();
            }
        }

        public async Task Initialize(string bucketname)
        {
            await _connection.SetBucket(bucketname);
        }

        public async Task<bool> UploadPoint(PointData data, bool isQueued = false)
        {
            var status = (await _connection.ReadyAsync()).Status;
            //Check if influxdb is ready to receive data
            if (status == Ready.StatusEnum.Ready)
            {
                //Check if this is a queued object to prevent endlessly calling itself
                if (data!=null && (isQueued || await SyncQueue()))
                {
                    await _connection.WritePointAsync(data);
                    return true;
                }

                HandleFailure("a verification error", data, status);
                return false;
            }
            HandleFailure("a connection error", data, status);
            return false;
        }


        private async Task<bool> SyncQueue()
        {
            while (_queue.TryDequeue(out var data))
            {
                if (await UploadPoint(data, true)) continue;
                return false;
            }
            return true;
        }

        private void HandleFailure(string message, PointData data, Ready.StatusEnum? status)
        {
            if (status!=Ready.StatusEnum.Ready)
            {
                _queue.Enqueue(data);
            }
            _logger.Log(LogLevel.Warning, new EventId(), data, null, (d, exception) => $"Point couldn't be written to the database due to {message} ready: {status} point: {d}");
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}