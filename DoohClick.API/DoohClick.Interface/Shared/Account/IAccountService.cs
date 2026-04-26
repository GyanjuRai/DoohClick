

using DoohClick.Model.Shared.Account;

namespace DoohClick.Interface.Shared.Account
{
    public interface IAccountService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvLoginResponse> Login(MvLoginInfoParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvLoginResponse> RefreshToken(MvRefreshTokenParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvUserInfoResponse?> GetUserInfo(MvUserInfoParam param);
    }
}
