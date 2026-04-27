
using DoohClick.Model.Shared.Enum.ResponseEnum;

namespace DoohClick.Model.Shared.Response
{
    public static class ApiResponse
    {
        public static MvResponse<T> Success<T>(T Data, string message = "Success")
        {
            return new MvResponse<T>
            {
                Type = ResponseStatusEnum.Success.ToString(),
                Message = message,
                Data = Data
            };
        }

        public static MvResponse<object> Failure(string message, string? type = null)
        {
            return new MvResponse<object>
            {
                Type = type ?? ResponseStatusEnum.Failure.ToString(),
                Message = message,
                Data = null
            };
        }
    }
}
