
using System.ComponentModel;

namespace DoohClick.Model.Shared.Enum.Response
{
    public enum ResponseStatusEnum
    {
        [Description("Request complete")]
        Success,
        [Description("Request failed")]
        Failure,
        [Description("Resource not found")]
        NotFound,
        [Description("Validation error occurred")]
        ValidationError,
        [Description("User is not authorized")]
        Unauthorized,
        [Description("User does not have permission")]
        Forbidden,
        [Description("An unexpected error occurred")]
        ServerError,
        [Description("Conflict with current state of the resource")]
        Conflict
    }
}
