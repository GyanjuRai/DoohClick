
using DoohClick.API.Const;
using DoohClick.API.Middleware;
using DoohClick.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    /**
     * ===============================
     *      Serilog
     * ===============================
    */
    builder.Host.UseSerilog((context, services, configuration) => {
        configuration.ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services);

    });

    /**
     * ===============================
     *      DB Connection
     * ===============================
    */
    var ACTIVE_DB = Environment.GetEnvironmentVariable(AppConst.ACTIVE_DB) ?? "Local";
    var CONNECTION_STRING = builder.Configuration.GetConnectionString(ACTIVE_DB);

    builder.Services.AddDbContext<AppDbContext>((option) =>
    {
        option.UseSqlServer(
            CONNECTION_STRING,
            SqlOptions => SqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        )
        .UseSnakeCaseNamingConvention();
    });

    /**
     * ===============================
     *      Cors Policy
     * ===============================
    */
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(AppConst.POLICY_NAME, builder =>
        {
            builder.WithOrigins(AppConst.ORIGIN)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });
    builder.Services.AddCoreServices();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    /**
     * ===============================
     *      Swagger
     * ===============================
    */
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(AppConst.API_VERSION, new OpenApiInfo
        {
            Version = AppConst.API_VERSION,
            Title = AppConst.API_TITLE,
            Description = AppConst.API_DESCRIPTION,
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<GlobalExpectionHandler>();

    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

