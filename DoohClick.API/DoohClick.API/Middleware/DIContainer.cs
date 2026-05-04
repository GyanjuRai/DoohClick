using DoohClick.API.Const;
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.cms.media;
using DoohClick.Interface.Application.crm.advertiser;
using DoohClick.Interface.Application.Inv.Screen;
using DoohClick.Interface.Shared.Account;
using DoohClick.Interface.Shared.Auth;
using DoohClick.Interface.Shared.File;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Interface.Shared.Listitem;
using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.AppSetting;
using DoohClick.Model.Shared.Auth;
using DoohClick.Model.Shared.Enum.User;
using DoohClick.Service.Application.cms.media;
using DoohClick.Service.Application.crm.advertiser;
using DoohClick.Service.Application.Inv.Screen;
using DoohClick.Service.Shared.Account;
using DoohClick.Service.Shared.Auth;
using DoohClick.Service.Shared.File;
using DoohClick.Service.Shared.JsonSerializer;
using DoohClick.Service.Shared.Listitem;

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

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicy.ADMIN, policy =>
                    policy.RequireClaim(AppClaim.UserRole, nameof(UserRoleEnum.ADMIN)));

                options.AddPolicy(AppPolicy.MANAGER, policy =>
                    policy.RequireClaim(AppClaim.UserRole, nameof(UserRoleEnum.MANAGER)));

                options.AddPolicy(AppPolicy.OPERATOR, policy =>
                    policy.RequireClaim(AppClaim.UserRole, nameof(UserRoleEnum.OPERATOR)));

                options.AddPolicy(AppPolicy.ADMINMANAGER, policy =>
                    policy.RequireClaim(AppClaim.UserRole,
                        nameof(UserRoleEnum.ADMIN),
                        nameof(UserRoleEnum.MANAGER)));

                options.AddPolicy(AppPolicy.ADMINOPERATOR, policy =>
                    policy.RequireClaim(AppClaim.UserRole,
                        nameof(UserRoleEnum.ADMIN),
                        nameof(UserRoleEnum.OPERATOR)));

                options.AddPolicy(AppPolicy.ALL, policy =>
                    policy.RequireClaim(AppClaim.UserRole,
                        nameof(UserRoleEnum.ADMIN),
                        nameof(UserRoleEnum.MANAGER),
                        nameof(UserRoleEnum.OPERATOR)));
            });

            return services;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthService, AuthService>()
                            .AddScoped<IJsonSerializer, JsonSerializer>()
                            .AddScoped<IDataAccessService, DataAccessService>()
                            .AddTransient<IAccountService, AccountService>()
                            .AddScoped<IFileService, FileService>();
        }

        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            return services.AddTransient<IScreenService, ScreenService>()
                            .AddTransient<IAdvertiserService, AdvertiserService>()
                            .AddTransient<IMediaService, MediaService>();
        }

        public static IServiceCollection AddSharedService(this IServiceCollection services)
        {
            return services.AddTransient<IListiemService, ListitemService>();
        }
    }
}
