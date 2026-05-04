using DoohClick.API.Const;
using DoohClick.Interface.Application.crm.advertiser;
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Shared.Enum.Response;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.crm.advertiser
{
    [Tags("CRM")]
    public class AdvertiserController : CrmController
    {
        private IAdvertiserService _advertiserService;

        public AdvertiserController
            (
                IAdvertiserService advertiserService
            )
        {
            _advertiserService = advertiserService;
        }

        [HttpGet("advertiser/grid")]
        [Authorize(AppPolicy.ADMINMANAGER)]
        public async Task<IActionResult> GetGrid([FromQuery] MvGridParamOption<MvAdvertiserFilterOptions> param)
        {
            MvGridResponse<MvAdvertiser>? result = await _advertiserService.GetGrid(param);

            if (result is null)
            {
                return NotFound(ApiResponse.Success("No data", ResponseStatusEnum.NotFound.ToString()));
            }

            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("advertiser/ddl")]
        [Authorize(AppPolicy.ALL)]
        public async Task<IActionResult> GetDdl([FromQuery] MvTenantIdParam param)
        {
            List<MvAdvertiserDdl>? result = await _advertiserService.GetDdl(param);

            if (result is null)
            {
                return NotFound(ApiResponse.Success("No data", ResponseStatusEnum.NotFound.ToString()));
            }

            return Ok(ApiResponse.Success(result));
        }
    }
}
