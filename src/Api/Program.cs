using Api;
using Application;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.Sources.Clear();
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services
        .AddApplication(builder.Configuration)
        .AddInfractructure(builder.Configuration)
        .AddPresentation(builder.Configuration);
}

Log.Logger =
    new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();
{
    //app.MigrateDatabase();
    app.UsePresentation(builder.Configuration);
}

