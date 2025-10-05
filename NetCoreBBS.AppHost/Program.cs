using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL
var postgresUsername = builder.AddParameter("pguser", "postgresadmin");
var postgresPassword = builder.AddParameter("pgpassword", "my_password");
var postgres = builder.AddPostgres("postgres", postgresUsername, postgresPassword)
    /*.WithPgAdmin(pgAdmin => 
        pgAdmin.WithHostPort(8089)
        .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@bbs.com")
        .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "admin123"))*/
    .WithDataVolume()
    .WithEnvironment("POSTGRES_DB", "netcorebbs")
    .WithEnvironment("POSTGRES_USER", "postgresadmin")
    .WithEnvironment("POSTGRES_PASSWORD", "my_password");

var postgresDb = postgres.AddDatabase("netcorebbs");

// pgAdmin (most updated version)
var pgadmin = builder.AddContainer("pgadmin", "dpage/pgadmin4")
    .WithHttpEndpoint(port: 8089, targetPort: 80)
    .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@bbs.com")
    .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "admin123")
    .WithReference(postgresDb);

// NETCoreBBS web app
var netcorebbs = builder.AddProject<Projects.NetCoreBBS>("netcorebbs-app")
    .WithReference(postgresDb)
    .WithHttpEndpoint(port: 5000, targetPort: 80) // HTTP port
    .WithHttpsEndpoint(port: 5001, targetPort: 443) // HTTPS port
    .WithEnvironment("ConnectionStrings__DefaultConnection", "Host=postgres;Database=netcorebbs;Username=postgresadmin;Password=my_password;Port=5432")
    .WaitFor(postgresDb);

builder.Build().Run();