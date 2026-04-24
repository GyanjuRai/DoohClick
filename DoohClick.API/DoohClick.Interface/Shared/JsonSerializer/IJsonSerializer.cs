
namespace DoohClick.Interface.Shared.JsonSerializer
{
    public interface IJsonSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        T? DeserializeObject<T>(string json);
    }
}
