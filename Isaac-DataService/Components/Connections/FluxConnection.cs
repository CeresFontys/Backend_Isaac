using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Isaac_DataService.Components.Connections
{
    public class FluxConnection : IFluxConnection
    {
        private readonly string _orgId;
        private string _sourceBucket;
        public string BucketName { get; private set; }

        public FluxConnection(IConfiguration configuration, ILogger<FluxConnection> logger)
        {
            _sourceBucket = configuration.GetValue<string>("Influx:BucketName");;
            var username = configuration.GetValue<string>("Influx:Username");
            var password = configuration.GetValue<string>("Influx:Password");
            var url = "http://" +
                      configuration.GetValue<string>("Influx:IP") +
                      ":" +
                      configuration.GetValue<string>("Influx:Port");

            _orgId = configuration.GetValue<string>("Influx:Organisation");
            
            var options = InfluxDBClientOptions.Builder.CreateNew().Authenticate(username, password.ToCharArray())
                .Org(_orgId)
                .Url(url).Build();

            Client = InfluxDBClientFactory.Create(options);
        }

        public InfluxDBClient Client { get; }

        private async Task EnsureBucket(string name)
        {
            var api = Client?.GetBucketsApi();
            
            if (api != null && await api.FindBucketByNameAsync(name) == null)
                await api.CreateBucketAsync(name,
                    null,
                    _orgId);
        }

        public async Task SetBucket(string name)
        {
            await EnsureBucket(name);
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        public async Task WritePointAsync(PointData data)
        {
            await Client.GetWriteApiAsync().WritePointAsync(_sourceBucket,_orgId, data);
        }

        public async Task<Ready> ReadyAsync()
        {
           return await Client.ReadyAsync();
        }
    }
}