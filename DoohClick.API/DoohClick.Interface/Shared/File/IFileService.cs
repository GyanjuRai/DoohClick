using DoohClick.Model.Shared.File;

namespace DoohClick.Interface.Shared.File
{
    public interface IFileService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MvFileUploadResult> UploadAsync(MvFileUploadParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaUrl"></param>
        /// <returns></returns>
        Task DeleteAsync(string fileUrl);
    }
}
