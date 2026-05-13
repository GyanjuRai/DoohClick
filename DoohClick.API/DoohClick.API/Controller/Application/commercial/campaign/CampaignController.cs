using DoohClick.API.Helper;
using DoohClick.Interface.Application.commercial.campaign;
using DoohClick.Model.Application.commercial.campaign;
using DoohClick.Model.Shared.CodePrefix;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Helper;
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

        [HttpGet("campaign/schedules")]
        public async Task<IActionResult> GetSchedules([FromQuery] MvCampaignIdParam param)
        {
            List<MvCampaignScreenSchedule>? result = await _campaignService.GetSchedules(param);
            
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("campaign")]
        public async Task<IActionResult> Save([FromBody] MvCampaign param)
        {
            param.CampaignCode = CodeGenerator.Generate(CodePrefix.Campaign, ClaimsPrincipalExtensions.GetTenantCode(User));
            param.CreatedBy = ClaimsPrincipalExtensions.GetUserId(User);
            param.ModifiedBy = ClaimsPrincipalExtensions.GetUserId(User);
            param.TenantId = ClaimsPrincipalExtensions.GetTenantId(User);
            MvCampaign? result = await _campaignService.Save(param);
 
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("campaign/schedule")]
        public async Task<IActionResult> SaveSchedule([FromBody] List<MvCampaignScreenScheduleParam> param)
        {
            MvCampaignScreenSchedule? result = await _campaignService.SaveSchedule(param);

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

        [HttpPost("campaign/approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            MvCampaignIdParam param = new MvCampaignIdParam 
            { 
                Id = id, 
                UpdatedBy =  ClaimsPrincipalExtensions.GetUserId(User) 
            };
            MvCampaignIdParam? result = await _campaignService.Approve(param);

            return Ok(ApiResponse.Success(result));
        }
    }
}
