

using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;

namespace DoohClick.Interface.Application.crm.advertiser
{
    public interface IAdvertiserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvGridResponse<MvAdvertiser>?> GetGrid(MvGridParamOption<MvAdvertiserFilterOptions> param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MvAdvertiserDdl>?> GetDdl(MvTenantIdParam param);
    }
}
