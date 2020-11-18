using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Configuration;

namespace Isaac_DataService.Components.Connections
{
    public class FluxConnection : IFluxConnection
    {
        public readonly string OrgId;
        private readonly string password;
        private readonly string url;
        private readonly string username;

        public FluxConnection(IConfiguration configuration)
        {
            username = configuration.GetValue<string>("Influx:Username");
            password = configuration.GetValue<string>("Influx:Password");
            url = "http://" +
                  configuration.GetValue<string>("Influx:IP") +
                  ":" +
                  configuration.GetValue<string>("Influx:Port");

            OrgId = configuration.GetValue<string>("Influx:Organisation");

            var options = InfluxDBClientOptions.Builder.CreateNew().Authenticate(username, password.ToCharArray())
                .Org(OrgId)
                .Url(url).Build();

            Client = InfluxDBClientFactory.Create(options);
        }

        public InfluxDBClient Client { get; }

        public async Task EnsureBucket(string name, BucketRetentionRules retentionRules)
        {
            var api = Client?.GetBucketsApi();
            if (api != null && await api.FindBucketByNameAsync(name) == null)
                await api.CreateBucketAsync(name, retentionRules, OrgId);
        }

        public async Task<bool> SetRetention(string name, BucketRetentionRules rule)
        {
            var api = Client.GetBucketsApi();
            var oldBucket = await api.FindBucketByNameAsync(name);
            var oldRule = oldBucket.RetentionRules.Find(retentionRules =>
                retentionRules.Type == BucketRetentionRules.TypeEnum.Expire);
            if (oldRule != null) oldBucket.RetentionRules.Remove(oldRule);
            oldBucket.RetentionRules.Add(rule);
            var newBucket = await api.UpdateBucketAsync(oldBucket);

            if (newBucket.RetentionRules.Contains(rule)) return true;

            return false;
        }
    }
}