

namespace DoohClick.Model.Shared.AppSetting
{
    public record AppSetting
    {
        public required string Origins { get; set; }
        public required string WebUrl { get; set; }
        public required string ApiUrl { get; set; }
    }
}
