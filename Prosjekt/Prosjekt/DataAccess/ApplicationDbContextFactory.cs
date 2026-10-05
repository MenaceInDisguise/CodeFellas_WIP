using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Prosjekt.DataAccess
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=127.0.0.1;Port=3306;Database=resourcedb;User=root;Password=Gruppe12!;";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Fast MariaDB-versjon forhindrer nettverkskall under design-time
            var serverVersion = new MariaDbServerVersion(new Version(11, 4, 0));

            builder.UseMySql(connectionString, serverVersion);

            return new ApplicationDbContext(builder.Options);
        }
    }
}