
using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;

namespace DoohClick.Model.Shared.Exceptions.Auth
{
    public class AuthException : AppException
    {
        public AuthException(
            string message
            ) : base(message, (int)HttpStatusCodeEnum.Unauthorized, ResponseStatusEnum.Unauthorized.ToString())
        {}
    }
}
