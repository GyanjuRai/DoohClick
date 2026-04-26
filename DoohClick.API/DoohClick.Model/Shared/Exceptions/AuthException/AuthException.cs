
namespace DoohClick.Model.Shared.Exceptions
{
    public class AuthException : AppException
    {
        public AuthException(
            string message, 
            int statusCode, 
            string errorCode
            ) : base(message, statusCode = 401, errorCode)
        {}
    }
}
