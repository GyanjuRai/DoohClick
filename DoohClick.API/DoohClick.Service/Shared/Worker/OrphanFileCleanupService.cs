
using DoohClick.DataAccess.Dapper;
using DoohClick.DataAccess.Data;
using DoohClick.DataAccess.Entity;
using DoohClick.Interface.Shared.File;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Interface.Shared.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoohClick.Service.Shared.Worker
{
    public class OrphanFileCleanupService: IOrphanFileCleanupService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly ILogger<OrphanFileCleanupService> _logger;
        private readonly string _webRootPath;
        public OrphanFileCleanupService(
                AppDbContext appDbContext,
                IFileService fileService,
                ILogger<OrphanFileCleanupService> logger,
                string webRootPath
            )
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _logger = logger;
            _webRootPath = Path.Combine(webRootPath, "file");
        }

        public async Task ExecuteAsync()
        {

            HashSet<string?> media = await _appDbContext.MediaLibraries
                .Where(m => m.CreatedAt < DateTime.UtcNow.AddMinutes(-30)) // grace period
                .Select(m => m.FileUrl)
                .ToHashSetAsync();

            string[]? filesOnDisk = Directory.GetFiles(
                _webRootPath, "*.*", SearchOption.AllDirectories
                );

            int deleted = 0;

            foreach (string filePath in filesOnDisk)
            {
                string relativePath = "/file/" + Path.GetRelativePath(_webRootPath, filePath)
                                             .Replace("\\", "/");

                if (!media.Contains(relativePath))
                {
                    try
                    {
                        await _fileService.DeleteAsync(relativePath);
                        deleted++;
                        _logger.LogInformation("Deleted orphan {Path}", relativePath);
                    }
                    catch (Exception ex)
                    {
                        {
                            _logger.LogError(ex, "Failed to delete {Path}", relativePath);
                        }
                    }
                }
            }

            _logger.LogInformation(
                "OrphanCleanup done. Deleted {Deleted}/{Total}",
                deleted, filesOnDisk.Length
            );
        }
    }
}
