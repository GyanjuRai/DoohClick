

using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;

namespace DoohClick.Model.Shared.Exceptions.Validation
{
    public class ValidationException: AppException
    {
        public ValidationException(
            string message
            ) : base(message, (int)HttpStatusCodeEnum.BadRequest, ResponseStatusEnum.ValidationError.ToString())
        { }
    }
}
