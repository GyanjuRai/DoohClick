
using DoohClick.Model.Application.commercial.campaign;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;

namespace DoohClick.Interface.Application.commercial.campaign
{
    public interface ICampaignService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvGridResponse<MvCampaign>?> GetGrid(MvGridParamOption<MvCampaignFilterOptionParam> param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvCampaign?> Save(MvCampaign param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvCampaignIdParam?> Remove(MvCampaignIdParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        Task<MvCampaignIdParam?> Approve(MvCampaignIdParam param);
    }
}
