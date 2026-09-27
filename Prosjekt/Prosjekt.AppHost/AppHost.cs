var builder = DistributedApplication.CreateBuilder(args);

// 1. Definer MariaDB-containeren med eksplisitt bilde, root-passord og databasen "mysql"
var mariadb = builder.AddMySql("mariadbcontainer", password: builder.AddParameter("password", "Gruppe12!"))
                     .WithImage("mariadb", "latest");

var mysqlDb = mariadb.AddDatabase("mysql");

// 2. Registrer webappen fra Dockerfile, sett opp porter og koble til databasen
builder.AddDockerfile(
    "Prosjekt",
    "..",
    "Prosjekt/Dockerfile")
    .WithHttpEndpoint(
        port: 5027,
        targetPort: 8080)
    .WithReference(mysqlDb); // Sender automatisk inn tilkoblingsstreng til webappen

builder.Build().Run();