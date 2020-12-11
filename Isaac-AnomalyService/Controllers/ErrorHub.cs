using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Microsoft.AspNetCore.SignalR;

namespace Isaac_AnomalyService.Controllers
{
    public class ErrorHub : Hub
    {

        public async Task SendError(List<SensorError> list)
        {
            //var list = new List<SensorError>();

            //await foreach (var error in _dbContext.Errors.AsAsyncEnumerable())
            //{
            //    list.Add(error);
            //}
            await (Clients?.All?.SendAsync("ReceiveErrors", list)??Task.CompletedTask);
        }
    }
}
