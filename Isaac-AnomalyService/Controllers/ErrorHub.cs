using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Microsoft.AspNetCore.SignalR;

namespace Isaac_AnomalyService.Controllers
{

    public class UserHandler
    {
        public static HashSet<String> connectedClients = new HashSet<string>();
    }

    public class ErrorHub : Hub
    {

        public void SendError(List<SensorError> list)
        {
            //var list = new List<SensorError>();

            //await foreach (var error in _dbContext.Errors.AsAsyncEnumerable())
            //{
            //    list.Add(error);
            //}
            foreach (var CID in UserHandler.connectedClients)
            {
               Clients.Client(CID).SendAsync("ReceiveErrors", "list");
            }

        }

        public override Task OnConnectedAsync()
        {
            UserHandler.connectedClients.Add(Context.ConnectionId);
            Clients.All.SendAsync("Connected", "User Connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.connectedClients.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }


        
    }
}
