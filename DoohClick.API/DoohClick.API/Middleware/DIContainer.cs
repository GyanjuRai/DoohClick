using DoohClick.API.Const;
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Shared.Account;
using DoohClick.Interface.Shared.Auth;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Shared.AppSetting;
using DoohClick.Model.Shared.Auth;
using DoohClick.Model.Shared.Enum.UserEnum;
using DoohClick.Service.Shared.Account;
using DoohClick.Service.Shared.Auth;
using DoohClick.Service.Shared.JsonSerializer;

namespace DoohClick.API.Middleware
{
    public static class DIContainer
    {
        public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            AppSetting appSetting = configuration.GetSection("AppSetting").Get<AppSetting>()
                                    ?? throw new InvalidOperationException("AppSetting configuration is missing");

            MvJwtConfig jwtConfig = configuration.GetSection("Jwt").Get<MvJwtConfig>()
                                    ?? throw new InvalidOperationException("JwtConfig configuration is missing");

            return services.AddSingleton(appSetting)
                            .AddSingleton(jwtConfig);
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthService, AuthService>()
                            .AddScoped<IJsonSerializer, JsonSerializer>()
                            .AddScoped<IDataAccessService, DataAccessService>()
                            .AddTransient<IAccountService, AccountService>();
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicy.ADMIN, policy => 
                    policy.RequireRole(nameof(UserRoleEnum.ADMIN)));

                options.AddPolicy(AppPolicy.MANAGER, policy => 
                    policy.RequireRole(nameof(UserRoleEnum.MANAGER)));
            
                options.AddPolicy(AppPolicy.OPERATOR, policy => 
                    policy.RequireRole(nameof(UserRoleEnum.OPERATOR)));

                options.AddPolicy(AppPolicy.ADMINMANAGER, policy =>
                    policy.RequireRole(
                            nameof(UserRoleEnum.ADMIN),
                            nameof(UserRoleEnum.MANAGER)
                        ));

                options.AddPolicy(AppPolicy.ADMINOPERATOR, policy =>
                    policy.RequireRole(
                            nameof(UserRoleEnum.ADMIN),
                            nameof(UserRoleEnum.OPERATOR)
                        ));

                options.AddPolicy(AppPolicy.ALL, policy =>
                    policy.RequireRole(
                            nameof(UserRoleEnum.ADMIN),
                            nameof(UserRoleEnum.MANAGER),
                            nameof(UserRoleEnum.OPERATOR)
                        ));

            });

            return services;
        }
    }
}
