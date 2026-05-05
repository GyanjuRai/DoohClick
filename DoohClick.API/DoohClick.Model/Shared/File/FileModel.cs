
using Microsoft.AspNetCore.Http;

namespace DoohClick.Model.Shared.File
{
    public record MvFileUploadParam
    {
        public required IFormFile File { get; set; } 
    }

    public record MvFileUploadResult
    {
        public required string FileName { get; set; }
        public required string FileUrl  { get; set; }
        public string? Resolution { get; set; }
        public decimal? DurationSec { get; set; }
        public bool? IsVideo { get; set; }
        public long? FileSizeBytes { get; set; }
    }
}
