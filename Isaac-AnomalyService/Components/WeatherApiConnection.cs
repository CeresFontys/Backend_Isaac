using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Isaac_AnomalyService.Components
{
    public class WeatherApiConnection
    {
        private IHttpClientFactory _clientFactory;
        private string CityId;
        private string WeatherApiKey;
       

        public WeatherApiConnection(IConfiguration Configuration , IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            CityId = Configuration.GetValue<string>("WeatherApiConfig:CityId");
            WeatherApiKey = Configuration.GetValue<string>("WeatherApiConfig:ApiKey");
        }

        public async Task<WeatherApiWield> GetWeatherApi()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={CityId}&appid={WeatherApiKey}&units=metric");
            WeatherApiData myDeserializedClass = JsonConvert.DeserializeObject<WeatherApiData>(await response.Content.ReadAsStringAsync());
            

            var weatherApiWield = new WeatherApiWield();
            weatherApiWield.ApiTemp = myDeserializedClass.main.temp;
            weatherApiWield.ApiHum = myDeserializedClass.main.humidity;
            return weatherApiWield;
        }

        


        //public async IAsyncEnumerable<WeatherApiData> LoadWeatherApiData()
        //{
        //    var data = await _clientFactory.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={CityId}&appid={WeatherApiKey}&units=metric");
        //    Content = await data.Content.ReadAsStringAsync();
        //    Temperature = ("temp");

        //}

       
    }
}
