using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RegisterContext>
    {
        public RegisterContext CreateDbContext(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<RegisterContext>();

            var cnnString = config.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(cnnString);

            return new RegisterContext(builder.Options);
        }
    }
}
