
using DoohClick.Model.Application.cms.media;
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.File;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;

namespace DoohClick.Interface.Application.cms.media
{
    public interface IMediaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvGridResponse<MvMedia>?> GetAll(MvGridParamOption<MvMediaFilterOptions> param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        Task<MvMedia?> Add(MvMedia param);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvMedia?> Remove(MvMediaDel param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MvMediaDdl>?> GetDdl(MvMediaDdlParam param);
    }
}
