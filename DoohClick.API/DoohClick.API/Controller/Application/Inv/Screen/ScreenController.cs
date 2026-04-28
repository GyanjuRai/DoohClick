using DoohClick.API.Const;
using DoohClick.API.Helper;
using DoohClick.Interface.Application.Inv.Screen;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.Enum.Response;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.Inv.Screen
{
    [Tags("Inventory")]
    public class ScreenController : InvController
    {
        private readonly IScreenService _screenService;
        public ScreenController(
                IScreenService screenService
            )
        {
            _screenService = screenService;
        }

        [HttpGet("grid")]
        [Authorize(Policy = AppPolicy.ALL)]
        public async Task<IActionResult> GetGrid([FromQuery] MvGridParamOption<MvScreenFilterOptions> param)
        {
            MvGridResponse<MvScreen>? result = await _screenService.GetGrid(param);
            if (result is null)
            {
                return NotFound(ApiResponse.Success("No data", ResponseStatusEnum.NotFound.ToString()));
            }

            return Ok(ApiResponse.Success(result));
        }

        [HttpPost("save")]
        [Authorize(Policy = AppPolicy.ADMINMANAGER)]
        public async Task<IActionResult> Save([FromBody] MvScreen param)
        {
            MvScreen screen = param;
            screen.CreatedBy = ClaimsPrincipalExtensions.GetUserId(User);
            screen.TenantId = ClaimsPrincipalExtensions.GetTenantId(User);

            MvScreen? result = await _screenService.Save(screen);
            if (result is null)
            {
                return BadRequest(ApiResponse.Success("Failed to save data", ResponseStatusEnum.Failure.ToString()));
            }
            return Ok(ApiResponse.Success(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPolicy.ADMINMANAGER)]
        public async Task<IActionResult> Remove(Guid id)
        {
            MvScreenDelParam screen = new MvScreenDelParam
            {
                Uuid = id,
                TenantId = ClaimsPrincipalExtensions.GetTenantId(User),
                DeletedBy = ClaimsPrincipalExtensions.GetUserId(User)
            };

            MvScreen? result = await _screenService.Remove(screen);
            if (result is null)
            {
                return BadRequest(ApiResponse.Failure("Failed to remove data"));
            }
            return Ok(ApiResponse.Success(result));
        }
    }
}
