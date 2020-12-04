using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Components.Logic
{
    public class WeatherApi
    {

        

        //private HttpClient _client;
        //private string CityId;
        //private string WeatherApiKey;

        //public WeatherApi(IConfiguration Configuration)
        //{
        //    CityId = Configuration.GetValue<string>("WeatherApiConfig:CityId");
        //    WeatherApiKey = Configuration.GetValue<string>("WeatherApiConfig:ApiKey");
        //}

        //public async Task<string> GetWeatherApi()
        //{
        //    _client = new HttpClient();
        //   var response = await _client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={CityId}&appid={WeatherApiKey}&units=metric");
        //   return await response.Content.ReadAsStringAsync();

        //}



    }
}
