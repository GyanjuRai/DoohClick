
namespace DoohClick.DataAccess.Dapper
{
    public interface IDataAccessService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<string> ActionProcedure(string sp, string json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<string> RetrievalProcedure(string sp, string json);
    }
}
