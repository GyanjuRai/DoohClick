using DoohClick.API.Const;
using DoohClick.API.Controller.Shared.Auth;
using DoohClick.Interface.Shared.Listitem;
using DoohClick.Model.Shared.Listitem;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Shared.Listitem
{
    [Tags("Reference")]
    [Route("reference/[controller]")]
    public class ListItemController: AuthController
    {
        private readonly IListiemService _listiemService;
        public ListItemController(
                IListiemService listiemService    
            ) 
        {
            _listiemService = listiemService;
        }

        [HttpGet("ddl")]
        [Authorize(Policy = AppPolicy.ALL)]
        public async Task<IActionResult> GetDdl([FromQuery]MvListitemDdlParam param)
        {
            List<MvListitemDdlResponse>? result = await _listiemService.GetDdl(param);
            return Ok(ApiResponse.Success(result));
        }
    }
}
