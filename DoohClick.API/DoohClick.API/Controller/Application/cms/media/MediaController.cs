using DoohClick.API.Const;
using DoohClick.API.Helper;
using DoohClick.Interface.Application.cms.media;
using DoohClick.Model.Application.cms.media;
using DoohClick.Model.Shared.Enum.Response;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.cms.media
{
    [Tags("CMS")]
    public class MediaController: CmsController
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("grid")]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> GetAll([FromQuery] MvGridParamOption<MvMediaFilterOptions> param)
        {
            MvGridResponse<MvMedia>? result = await _mediaService.GetAll(param);

            if (result is null)
            {
                return NotFound(ApiResponse.Success("No data", ResponseStatusEnum.NotFound.ToString()));
            }

            return Ok(ApiResponse.Success(result));
        }

        [HttpPost]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> Add([FromBody] MvMedia  param)
        {
            param.CreatedBy = ClaimsPrincipalExtensions.GetUserId(User);
            param.TenantId = ClaimsPrincipalExtensions.GetTenantId(User);
            MvMedia? result = await _mediaService.Add(param);

            return Ok(ApiResponse.Success(result));
        }

        [HttpDelete]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> Remove([FromBody] MvMediaDel param)
        {
            MvMedia? result = await _mediaService.Remove(param);

            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("ddl")]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> GetDdl([FromQuery]MvMediaDdlParam param)
        {
            param.TenantId = ClaimsPrincipalExtensions.GetTenantId(User);
            List<MvMediaDdl>? result = await _mediaService.GetDdl(param);

            return Ok(ApiResponse.Success(result));
        }
    }
}
