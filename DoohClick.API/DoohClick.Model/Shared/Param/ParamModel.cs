

namespace DoohClick.Model.Shared.Param
{
    public record MvGridParamOption<T>
    {
        public T? Filter { get; set; }
        public int? Offset { get; set; }
        public int? PageSize { get; set; }
        public string? SearchText { get; set; }
    }
}
