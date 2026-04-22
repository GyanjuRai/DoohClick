using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DoohClick.Service.Shared.JsonSetting
{
    internal static class JsonSettings
    {
        internal static readonly JsonSerializerSettings SnakeCase = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Include
        };
    }
}
