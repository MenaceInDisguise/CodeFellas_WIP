using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Prosjekt.DataAccess
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Creates an ApplicationDbContext configured from appsettings.json files and a MySQL/MariaDB connection
        /// string.
        /// </summary>
        /// <remarks>Configuration is loaded from appsettings.json and appsettings.Development.json. The
        /// 'DefaultConnection' connection string is used or a hard-coded fallback, and DbContextOptions are configured
        /// to use MySQL with MariaDB server version 11.4.0.</remarks>
        /// <param name="args">Command-line arguments; not used for configuration.</param>
        /// <returns>A new ApplicationDbContext configured with DbContextOptions that use the resolved MySQL/MariaDB connection.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=127.0.0.1;Port=3306;Database=Mapdp;User=root;Password=Gruppe12!;";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var serverVersion = new MariaDbServerVersion(new Version(11, 4, 0));

            builder.UseMySql(connectionString, serverVersion);

            return new ApplicationDbContext(builder.Options);
        }
    }
}