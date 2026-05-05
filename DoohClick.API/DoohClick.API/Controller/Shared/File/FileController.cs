using DoohClick.API.Const;
using DoohClick.API.Controller.Shared.Auth;
using DoohClick.Interface.Shared.File;
using DoohClick.Model.Shared.File;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Shared.File
{
    public class FileController: AuthController
    {
        private readonly IFileService _fileService;
        public FileController(
                IFileService fileService
            ) 
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> Upload([FromForm] MvFileUploadParam param)
        {
            MvFileUploadResult result = await _fileService.UploadAsync(param);

            return Ok(ApiResponse.Success(result));
        }
    }
}
