using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DAL.DataContext
{
    public class AppConfiguration
    {
        public AppConfiguration()
        {
            // specify a config builder
            var configBuilder = new ConfigurationBuilder();

            // specify a path to the builder
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            // false: must get the json
            configBuilder.AddJsonFile(path, false);

            // build
            var root = configBuilder.Build();

            // get the connection string
            var appSettings = root.GetSection("ConnectionStrings:DefaultConnection");

            SqlConnectionStr = appSettings.Value;
        }

        public string SqlConnectionStr { get; set; }
    }
}
