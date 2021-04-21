using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;

namespace FittShop.Data.SqlServer
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private const string CONFIG_PATH = "../FittShop/appsettings.json";

        public AppDbContext CreateDbContext(string[] args)
        {
            if (!File.Exists(CONFIG_PATH))
                throw new FileNotFoundException("Config file not found: " + CONFIG_PATH);

            // Create file reader
            using var stream = new StreamReader(CONFIG_PATH);

            // Create class instance from json data
            var config = JsonConvert.DeserializeObject<ConnectionStringsConfig>(stream.ReadToEnd());

            Console.WriteLine("Database connection used: " + config.SelectedConnection);

            // Use connection string from appsettings and connect to SqlServer
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(config.ConnectionStrings[config.SelectedConnection]);

            // Create new AppDbContext instance
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}