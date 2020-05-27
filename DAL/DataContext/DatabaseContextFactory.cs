using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DataContext
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<RegisterContext>
    {
        public RegisterContext CreateDbContext(string[] args)
        {
            AppConfiguration configs = new AppConfiguration();
            var optionsBuilder = new DbContextOptionsBuilder<RegisterContext>();
            optionsBuilder.UseSqlServer(configs.SqlConnectionStr);
            return new RegisterContext(optionsBuilder.Options);
        }
    }
}
