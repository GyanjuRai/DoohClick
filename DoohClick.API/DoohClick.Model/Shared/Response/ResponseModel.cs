
namespace DoohClick.Model.Shared.Response
{
    public record MvResponse<T>
    {
        public required string Type { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
    public class MvGridResponse<T>
    {
        public List<T>? Data { get; set; }
        public int TotalRows { get; set; }
    }
}
