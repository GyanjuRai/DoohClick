
using DoohClick.API.Config;
using DoohClick.API.Const;
using DoohClick.API.Helper;
using DoohClick.API.Middleware;
using DoohClick.DataAccess.Data;
using DoohClick.Model.Shared.Auth;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Application starting up");

    var builder = WebApplication.CreateBuilder(args);

    /**
     * ===============================
     *      Serilog
     * ===============================
    */
    builder.Host.UseSerilog((context, services, configuration) =>
    {
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
     *      Jwt Configuration
     * ===============================
    */
    MvJwtConfig jwtConfig = builder.Configuration.GetSection("Jwt").Get<MvJwtConfig>()
                                    ?? throw new InvalidOperationException("JwtConfig configuration is missing");
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                bool tokenExpired = context.AuthenticateFailure is SecurityTokenExpiredException;
                
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                
                if(tokenExpired)
                    context.Response.Headers.Append("Token-Expired", "true");

                return context.Response.WriteAsJsonAsync(ApiResponse.Failure(tokenExpired ? "Session has expired" : "Invalid token"));
            }
        };
    });

    builder.Services.AddAppConfigurations(builder.Configuration)
                 .AddAuthorizationPolicies()
                 .AddCoreServices()
                 .AddApplicationService();
    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    });

    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();

    builder.Services.AddEndpointsApiExplorer();

    /**
     * ===============================
     *      Cors Policy
     * ===============================
    */
    string[] allowOrigins = (builder.Configuration.GetSection("AppSetting")["Origins"] ?? "").Split(',');
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(AppConst.POLICY_NAME, builder =>
        {
            builder.WithOrigins(allowOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });

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

    app.UseCors(AppConst.POLICY_NAME);

    app.UseAuthentication();

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

