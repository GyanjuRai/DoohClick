using DoohClick.DataAccess.Dapper;

namespace DoohClick.API.Middleware
{
    public static class DIContainer
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services.AddTransient<IDataAccessService, DataAccessService>();
        }
    }
}
