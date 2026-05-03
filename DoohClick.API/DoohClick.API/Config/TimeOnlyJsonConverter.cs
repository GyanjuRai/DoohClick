using Newtonsoft.Json;

namespace DoohClick.API.Config
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? value = reader.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
                return TimeOnly.MinValue;

            if (DateTime.TryParse(value, out var dt))
                return TimeOnly.FromDateTime(dt);

            return TimeOnly.Parse(value);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("HH:mm:ss"));
        }
    }
}
