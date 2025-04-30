using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;
//using MediatR.Extensions.Microsoft.DependencyInjection;
using System.Security.Claims;
namespace Uni_Mate.Features.StudentManager.ChangePassword.Command
{
    public record ChangePasswordCommand(string OldPassword, string NewPassword): IRequest<RequestResult<bool>>;

    public class ChangePasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ChangePasswordCommand, RequestResult<bool>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;  

        public ChangePasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters, IHttpContextAccessor httpContextAccessor)
            : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async override Task<RequestResult<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return RequestResult<bool>.Failure(ErrorCode.Unauthorized, "HttpContext is null");

            var hasAuthorizationHeader = httpContext.Request.Headers.TryGetValue("Authorization", out var token);

            var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RequestResult<bool>.Failure(ErrorCode.Unauthorized, "User ID not found in claims");

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");

            var result = _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword).Result;
            if (result.Succeeded)
                return RequestResult<bool>.Success(true, "Password changed successfully");

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return RequestResult<bool>.Failure(ErrorCode.PasswordChangeFailed, errors);
        }
    }
}
