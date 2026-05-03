using System.Text.RegularExpressions;

namespace DoohClick.API.Helper
{
    public class SlugifyParameterTransformer: IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) { return null; }
            return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
