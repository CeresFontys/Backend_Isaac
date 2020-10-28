using System;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Configuration;

namespace Isaac_DataService.Services
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
            url = "https://" +
                  configuration.GetValue<string>("Influx:IP") +
                  ":" +
                  configuration.GetValue<string>("Influx:Port");

            orgId = configuration.GetValue<string>("Influx:Organisation");

            var options = InfluxDBClientOptions.Builder.CreateNew().Authenticate(username, password.ToCharArray())
                .Url(url).Build();


            Client = InfluxDBClientFactory.Create(options);
        }

        public InfluxDBClient Client { get; }

        public async Task EnsureBucket(string name, BucketRetentionRules retentionRules)
        {
            var api = Client.GetBucketsApi();
            if (await api.FindBucketByNameAsync(name) == null) await api.CreateBucketAsync(name, retentionRules, orgId);
        }

        public async Task<bool> SetRetention(string name, BucketRetentionRules rule)
        {
            var api = Client.GetBucketsApi();
            var oldBucket = await api.FindBucketByNameAsync(name);
            var oldRule = oldBucket.RetentionRules.Find(retentionRules => retentionRules.Type == BucketRetentionRules.TypeEnum.Expire);
            if (oldRule!=null)
            {
                oldBucket.RetentionRules.Remove(oldRule);
            }
            oldBucket.RetentionRules.Add(rule);
            var newBucket = await api.UpdateBucketAsync(oldBucket);
            
            if (newBucket.RetentionRules.Contains(rule))
            {
                return true;
            }

            return false;
        }
    }
}