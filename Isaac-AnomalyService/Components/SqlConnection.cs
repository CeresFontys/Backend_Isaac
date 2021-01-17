using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Isaac_AnomalyService.Components
{
    public class SqlConnection
    {

        private IServiceScopeFactory _scopeFactory;
        private WeatherApiConnection _weatherApiConnection;
        private double weatherApiTemp;
        private double weatherApiHum;


        public SqlConnection( WeatherApiConnection weatherApiConnection, IServiceScopeFactory scopeFactory)
        {
            _weatherApiConnection = weatherApiConnection;
            _scopeFactory = scopeFactory;
        }

       

        //public IConfiguration Configuration { get; }
        //public string GetConnectionString()
        //{
        //    var connection = Configuration["MySQL:ConnectionString"];
        //    return connection;
        //}

        public async Task SaveData<T>(IEnumerable<T> data) where T : SensorError
        {
            var sort = data.ToList();
            sort.Sort((error, sensorError) => error.DateTime.CompareTo(sensorError.DateTime));
            var distinctItems = sort.GroupBy(i => new { i.X, i.Y, i.Type, i.Floor}).Select(g => g.First());
            using var scope = _scopeFactory.CreateScope();
            SetWeatherApiData(await _weatherApiConnection.GetWeatherApi());
            var dbContext = scope.ServiceProvider.GetRequiredService<Isaac_AnomalyServiceContext>();
       
        var list = new List<T>();
            list.AddRange(distinctItems.ToList());
            if (distinctItems != null)
            {
                foreach (var error in distinctItems)
                {
                    var check = dbContext.Find<T>(error.X, error.Y, error.Floor, error.ValueType);

                    double ApiValue = error.ValueType == "Temperature" ? weatherApiTemp : weatherApiHum;

                    if (check != null)
                    {
                        
                        check.Type = error.Type;
                        check.DateTime = error.DateTime;
                        check.DateTimeNext = error.DateTimeNext;
                        check.Error = error.Error;
                        check.Floor = error.Floor;
                        check.ValueFirst = error.ValueFirst;
                        check.ValueSecond = error.ValueSecond;
                        check.ValueType = error.ValueType;
                        check.ApiValue = ApiValue;
                        check.X = error.X;
                        check.Y = error.Y;
                        check.id = error.id;
                    }
                    else
                    {
                        error.ApiValue = ApiValue;
                        await dbContext.AddAsync(error);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public List<SensorError> GetAllErrors()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Isaac_AnomalyServiceContext>();

            return dbContext.Errors.ToList();
        }

        public void DeleteError(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Isaac_AnomalyServiceContext>();
            var Error = dbContext.Errors.SingleOrDefault(error => error.id == id);
            dbContext.Remove(Error);
            dbContext.SaveChanges();
        }

        public void SetWeatherApiData(WeatherApiWield weatherApiWield)
        {
            weatherApiTemp = weatherApiWield.ApiTemp;
            weatherApiHum = weatherApiWield.ApiHum;
        }








        //public static void SaveData<T>(string sql, T data)
        //{
        //    using (IDbConnection con = new MySqlConnection(GetConnectionString()))
        //    {

        //        con.Execute(sql, data);
        //    }
        //}

    }
}
