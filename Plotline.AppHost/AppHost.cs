using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

#region connect remote bd
//builder.AddConnectionString("Postgres", "Host=localhost;Port=5432;Database=;Username=;Password=");
#endregion

#region run bd with aspire
var postgresPassword = builder.AddParameter("Password",
    secret: true,
    value: "");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithImageTag("17")
    .WithPassword(postgresPassword)
    .WithEnvironment("POSTGRES_DB", "");
//.WithDataVolume();

var plotlineDb = postgres.AddDatabase("plotlinedb");
#endregion

var api = builder.AddProject<Projects.Plotline_API>("plotlineapi")
    .WithReference(plotlineDb);

// var frontend = builder.AddNpmApp("frontend", "../PlotlineFrontend")
//     .WithReference(api);

builder.Build().Run();
