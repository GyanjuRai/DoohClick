

using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Service.Shared.JsonSetting;
using Newtonsoft.Json;

namespace DoohClick.Service.Shared.JsonSerializer
{
    public class JsonSerializer: IJsonSerializer
    {
        public T? DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSettings.SnakeCase);
        }
    }
}
