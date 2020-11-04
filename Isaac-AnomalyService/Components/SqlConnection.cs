using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Isaac_AnomalyService.Components
{
    public class SqlConnection
    {
        public IConfiguration Configuration { get; }
        public string GetConnectionString()
        {
            var connection = Configuration["MySQL:ConnectionString"];
            return connection;
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
