using DoohClick.API.Const;
using DoohClick.Interface.Application.Inv.Screen;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.Enum.ResponseEnum;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.Inv.Screen
{
    [Tags("Inventory")]
    public class ScreenController: InvController
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

    }
}
