using DoohClick.API.Helper;
using DoohClick.Interface.Application.commercial.campaign;
using DoohClick.Model.Application.commercial.campaign;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.commercial.campaign
{
    [Tags("Commerical")]
    public class CampaignController: CommercialController
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("campaign/grid")]
        public async Task<IActionResult> GetGrid([FromQuery] MvGridParamOption<MvCampaignFilterOptionParam> param)
        {
            MvGridResponse<MvCampaign>? result = await _campaignService.GetGrid(param);

            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("campaign")]
        public async Task<IActionResult> Save([FromBody] MvCampaign param)
        {
            MvCampaign? result = await _campaignService.Save(param);
 
            return Ok(ApiResponse.Success(result));
        }

        [HttpDelete("campaign/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            MvCampaignIdParam param = new MvCampaignIdParam
            {
                Id = id,
                DeletedBy = ClaimsPrincipalExtensions.GetUserId(User)
            };
            MvCampaignIdParam? result = await _campaignService.Remove(param);

            return Ok(ApiResponse.Success(result));
        }
    }
}
