using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Configuration;

namespace Isaac_API.Services
{
    public class FluxConnection : IFluxConnection
    {
        private string username;
        private string password;
        private string url;
        private string orgId;

        public FluxConnection(IConfiguration configuration)
        {
            username = configuration.GetValue<string>("Influx:Username");
            password = configuration.GetValue<string>("Influx:Password");
            url = "https://" +
                  configuration.GetValue<string>("Influx:IP") +
                  ":" +
                  configuration.GetValue<string>("Influx:Port");
            
            orgId = configuration.GetValue<string>("Influx:Organisation");

            var options = InfluxDBClientOptions.Builder.CreateNew().Authenticate(username, password.ToCharArray()).Url(url).Build();
            
            
            Client = InfluxDBClientFactory.Create(options);
            
            
        }

        public InfluxDBClient Client { get; }

        public async Task EnsureBucket(string name, BucketRetentionRules retentionRules)
        {
            var api = Client.GetBucketsApi();
            if (await api.FindBucketByNameAsync(name)==null)
            {
                await api.CreateBucketAsync(name, retentionRules, orgId);
            }
        }
    }
}