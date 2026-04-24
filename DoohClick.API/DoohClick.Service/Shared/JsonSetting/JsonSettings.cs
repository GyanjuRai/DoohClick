using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DoohClick.Service.Shared.JsonSetting
{
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings SnakeCase = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Include
        };
    }
}
