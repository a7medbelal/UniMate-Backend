using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Authoraztion.LogoutUser.Commands;
public record LogoutUserCommand() : IRequest<RequestResult<bool>>;

public class LogoutUserCommandHandler : BaseWithoutRepositoryRequestHandler<LogoutUserCommand, RequestResult<bool>, User>
{
    public LogoutUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<bool>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_userInfo.ID);
        if (user == null)
            return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
        await _signInManager.SignOutAsync();
        return RequestResult<bool>.Success(true, "Logout successful");
    }
}
