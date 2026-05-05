
using DoohClick.DataAccess.Dapper;
using DoohClick.DataAccess.Data;
using DoohClick.DataAccess.Entity;
using DoohClick.Interface.Shared.File;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Interface.Shared.Worker;
using Microsoft.EntityFrameworkCore;

namespace DoohClick.Service.Shared.Worker
{
    public class OrphanFileCleanupService: IOrphanFileCleanupService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly string _webRootPath;
        public OrphanFileCleanupService(
                AppDbContext appDbContext,
                IFileService fileService,
                string webRootPath
            )
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webRootPath = Path.Combine(webRootPath, "file");
        }

        public async Task ExecuteAsync()
        {
            string[]? filesOnDisk = Directory.GetFiles(_webRootPath, "*.*", SearchOption.AllDirectories);

            List<string?> media = await _appDbContext.MediaLibraries
                .Select(m => m.FileUrl)
                .ToListAsync();

            foreach(string filePath in filesOnDisk)
            {
                string relativePath = "/file/" + Path.GetRelativePath(_webRootPath, filePath)
                                             .Replace("\\", "/");

                if (!media.Contains(relativePath)) 
                { 
                    _fileService.DeleteAsync(filePath);
                }
            }
        }
    }
}
