using DoohClick.API.Controller.Shared.Auth;
using DoohClick.Interface.Shared.Account;
using DoohClick.Model.Shared.Account;
using DoohClick.Model.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Shared.Account
{
    public class AccountController : AuthController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MvLoginInfoParam param)
        {
            MvLoginResponse result = await _accountService.Login(param);
            return Ok(ApiResponse.Success(result));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] MvRefreshTokenParam param)
        {
            MvLoginResponse result = await _accountService.RefreshToken(param);
            return Ok(ApiResponse.Success(result));
        }
    }
}
