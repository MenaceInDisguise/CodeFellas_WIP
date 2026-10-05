var builder = DistributedApplication.CreateBuilder(args);

// 1. Definer MariaDB-containeren
var mariadb = builder.AddMySql("mariadbcontainer", password: builder.AddParameter("password", secret: true))
                     .WithImage("mariadb", "latest");

// Byttet databasenavn fra "mysql" til "kartdb"
var kartDb = mariadb.AddDatabase("kartdb");

// 2. Registrer webappen fra Dockerfile, sett opp porter og koble til databasen
builder.AddDockerfile(
    "Prosjekt",
    "..",
    "Prosjekt/Dockerfile")
    .WithHttpEndpoint(
        port: 5027,
        targetPort: 8080)
    .WithReference(kartDb); // Sender automatisk inn tilkoblingsstreng for kartdb til webappen

builder.Build().Run();