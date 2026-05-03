

using DoohClick.Model.Shared.Listitem;

namespace DoohClick.Interface.Shared.Listitem
{
    public interface IListiemService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<MvListitemDdlResponse>?> GetDdl(MvListitemDdlParam param);
    }
}
