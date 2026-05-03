

namespace DoohClick.Model.Shared.Listitem
{
    public record MvListitemDdlParam
    {
        public required string CategoryCode { get; set; }
    }

    public record MvListitemDdlResponse
    {
        public required int Id { get; set; }
        public required string Item { get; set; }
        public required string Code { get; set; }
    }
}
