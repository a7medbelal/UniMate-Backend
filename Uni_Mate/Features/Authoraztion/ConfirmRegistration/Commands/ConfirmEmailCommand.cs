using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Authoraztion.ConfirmRegistration.Commands;

public record ConfirmEmailCommand(string Email, string Token) : IRequest<RequestResult<bool>>;

public class ConfirmRegistrationCommandHandler : BaseWithoutRepositoryRequestHandler<ConfirmEmailCommand, RequestResult<bool>>
{
    public ConfirmRegistrationCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        if (request.Token.Length < 6)
        {
            return RequestResult<bool>.Failure(ErrorCode.EmailNotConfirmed, "Email is not confirmed");
        }
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.EmailNotConfirmed);
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded)
        {
            user.EmailConfirmed = true;
        }
        await _userManager.UpdateAsync(user);

        return RequestResult<bool>.Success(true);
    }
}