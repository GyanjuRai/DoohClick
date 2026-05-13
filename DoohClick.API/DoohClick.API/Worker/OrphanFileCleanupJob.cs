using DoohClick.Interface.Shared.Worker;

namespace DoohClick.API.worker
{
    public class OrphanFileCleanupJob : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        public OrphanFileCleanupJob(
                IServiceScopeFactory serviceScopeFactory,
                IWebHostEnvironment webHostEnvironment,
                ILogger<OrphanFileCleanupJob> logger
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoCleanUp();
                    await Task.Delay(GetNextRunDelay(), stoppingToken);
                    _logger.LogInformation("DoCleanUp completed at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DoCleanUp failed at {Time}", DateTime.Now);
                }
            }
        }

        private static TimeSpan GetNextRunDelay()
        {
            DateTime now = DateTime.Now;
            DateTime next = now.Date.AddDays(1).AddHours(2);
            return next - now;
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
