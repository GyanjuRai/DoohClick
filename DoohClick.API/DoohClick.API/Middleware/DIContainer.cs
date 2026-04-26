using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Shared.Auth;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Shared.AppSetting;
using DoohClick.Model.Shared.Auth;
using DoohClick.Service.Shared.Auth;
using DoohClick.Service.Shared.JsonSerializer;

namespace DoohClick.API.Middleware
{
    public static class DIContainer
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthService, AuthService>()
                            .AddScoped<IJsonSerializer, JsonSerializer>()
                            .AddScoped<IDataAccessService, DataAccessService>();
        }

        public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            AppSetting appSetting = configuration.GetSection("AppSetting").Get<AppSetting>() 
                                    ?? throw new InvalidOperationException("AppSetting configuration is missing");

            return services.AddSingleton(appSetting);
        }
    }
}
