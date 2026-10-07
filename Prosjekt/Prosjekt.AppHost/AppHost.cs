var builder = DistributedApplication.CreateBuilder(args);

// 1. Define the MariaDB container
var mariadb = builder.AddMySql("mariadbcontainer", password: builder.AddParameter("password", secret: true))
                     .WithImage("mariadb", "latest");

// Changed the database name from "mysql" to "kartdb"
var mapDb = mariadb.AddDatabase("kartdb");

// 2. Register the web app from the Dockerfile, set up ports and connect to the database
builder.AddDockerfile(
    "Prosjekt",
    "..",
    "Prosjekt/Dockerfile")
    .WithHttpEndpoint(
        port: 5027,
        targetPort: 8080)
    .WithReference(mapDb); // Automatically passes the connection string for kartdb to the web app

builder.Build().Run();