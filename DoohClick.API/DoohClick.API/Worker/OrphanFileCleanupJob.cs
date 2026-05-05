using DoohClick.Interface.Shared.Worker;

namespace DoohClick.API.worker
{
    public class OrphanFileCleanupJob : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrphanFileCleanupJob(
                IServiceScopeFactory serviceScopeFactory,
                IWebHostEnvironment webHostEnvironment
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(UnitNextRun(), stoppingToken);
                await DoCleanUp();
            }
        }

        private static TimeSpan UnitNextRun()
        {
            return TimeSpan.FromMinutes(30);
        }

        private async Task DoCleanUp()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider
                .GetRequiredService<IOrphanFileCleanupService>();
            await service.ExecuteAsync();
        }

    }
}
