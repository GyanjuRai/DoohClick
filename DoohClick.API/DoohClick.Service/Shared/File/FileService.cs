


using DoohClick.Interface.Shared.File;
using DoohClick.Model.Shared.Exceptions.Validation;
using DoohClick.Model.Shared.File;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using System.Diagnostics;

namespace DoohClick.Service.Shared.File
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService()
        {
            _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public async Task<MvFileUploadResult> UploadAsync(MvFileUploadParam param)
        {
            string extension = Path.GetExtension(param.File.FileName).ToLower();
            bool isVideo = IsVideoExtension(extension);
            bool isImage = IsImageExtension(extension);

            ValidateFile(isImage, isVideo, extension, param.File.Length);

            string folder = isVideo ? "file/videos" : "file/images";
            string fileName = $"{Guid.NewGuid()}{extension}";
            string savePath = Path.Combine(_webRootPath,folder, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            await using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await param.File.CopyToAsync(stream);
            }

            string resolution;
            decimal? durationSec;

            if(isImage)
            {
                using var image = await Image.LoadAsync(savePath);
                resolution = $"{image.Width}x{image.Height}";
                durationSec = null;
            }
            else
            {
                (resolution, durationSec) = await ExtractVideoMetadataAsync(savePath);
            }

            return new MvFileUploadResult
            {
                FileName = fileName,
                FileUrl = $"/{folder}/{fileName}",
                Resolution = resolution,
                DurationSec = durationSec,
                IsVideo = isVideo,
                FileSizeBytes = param.File.Length
            };
        }

        public Task DeleteAsync(string fileUrl)
        {
            string filePath = Path.Combine(
                _webRootPath,
                fileUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        private static async Task<(string Resolution, decimal DurationSec)> ExtractVideoMetadataAsync(string filePath)
        {
            var args = $"-v quiet -print_format json -show_streams \"{filePath}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffprobe",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            var root = JsonConvert.DeserializeObject<JObject>(output);
            var streams = root!["streams"] as JArray;

            string resolution = "";
            decimal duration = 0;

            foreach (var stream in streams!)
            {
                if (stream["codec_type"]?.ToString() == "video")
                {
                    int width = stream["width"]!.Value<int>();
                    int height = stream["height"]!.Value<int>();
                    resolution = $"{width}x{height}";

                    if (decimal.TryParse(stream["duration"]?.ToString(), out var prased))
                    {
                        duration = prased;
                    }

                    break;
                }
            }

            return (resolution, duration);
        }

        private static void ValidateFile(bool isImage, bool isVideo, string extension, long size)
        {
            if (!isImage && !isVideo)
                throw new ValidationException($"File type {extension} is not supported.");

            if (size > 100_000_000)
                throw new ValidationException("File size exceeds the 100MB limit.");
        }

        private static bool IsImageExtension(string extension)
        {
            return new[] { ".jpg", ".jpeg", ".png" }.Contains(extension);
        }

        private static bool IsVideoExtension(string extension)
        {
            return new[] { ".mp4", ".webm" }.Contains(extension);
        }
    }
}
