
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;

namespace DoohClick.Interface.Application.Inv.Screen
{
    public interface IScreenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvGridResponse<MvScreen>?> GetAll(MvGridParamOption<MvScreenFilterOptions> param);
        /// <summary>
        /// Add if Id is null, Update if Id is not null
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvScreen?> Save(MvScreen param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvScreen?> Remove(MvScreenDelParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MvScreenDdl>?> GetDdl(MvTenantIdParam param);
    }
}
