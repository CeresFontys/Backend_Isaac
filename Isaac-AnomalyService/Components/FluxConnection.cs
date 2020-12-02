using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AnomalyService.Service
{
    public class FluxConnection : IFluxConnection
    {

        private readonly string orgId;
        private readonly string url;
        private readonly string username;
        private readonly string password;

        public FluxConnection(IConfiguration configuration)
        {
            username = configuration.GetValue<string>("Influx:Username");
            password = configuration.GetValue<string>("Influx:Password");
            url = "http://" +
                  configuration.GetValue<string>("Influx:IP") +
                  ":" +
                  configuration.GetValue<string>("Influx:Port");

            orgId = configuration.GetValue<string>("Influx:Organisation");

            var options = InfluxDBClientOptions.Builder.CreateNew().Authenticate(username, password.ToCharArray())
                .Url(url).Build();


            Client = InfluxDBClientFactory.Create(options);
        }


        public InfluxDBClient Client { get; }

        public async IAsyncEnumerable<SensorData> LoadSensorData(){

            var flux = "from(bucket:\"sensordata\") |> range(start: -5m)";
            var fluxTables = await Client.GetQueryApi().QueryAsync(flux, orgId);
            List<SensorData> dataList = new List<SensorData>();

            foreach(FluxTable fluxTable in fluxTables){

                var fluxRecords = fluxTable.Records;
                foreach(FluxRecord fluxRecord in fluxRecords)
                {
                    var sensordata = new SensorData();
                    var measurement = fluxRecord.GetMeasurement();
                    if (measurement == "sensorhumidity")
                    {
                        sensordata.Type = DataType.Humidity;
                    }
                    else if (measurement == "sensortemperature")
                    {
                        sensordata.Type = DataType.Temperature;
                    }
                    else
                    {
                        //uptime will be placed here
                        continue;
                    }

                    sensordata.Id = (string) fluxRecord.GetValueByKey("floor") + "," + (string)fluxRecord.GetValueByKey("x") + "," + (string)fluxRecord.GetValueByKey("y");
                    sensordata.Value = (double)fluxRecord.GetValue();
                    sensordata.Floor = Convert.ToInt32((string)fluxRecord.GetValueByKey("floor"));
                    sensordata.X = Convert.ToInt32((string)fluxRecord.GetValueByKey("x"));
                    sensordata.Y = Convert.ToInt32((string)fluxRecord.GetValueByKey("y"));
                    sensordata.DateTime = fluxRecord.GetTimeInDateTime() ?? DateTime.Now;
                    yield return sensordata;

                }
            } 
        }
    }
}
