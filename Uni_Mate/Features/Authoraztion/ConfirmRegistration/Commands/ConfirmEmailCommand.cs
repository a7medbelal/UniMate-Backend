using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return RequestResult<bool>.Failure(ErrorCode.EmailNotConfirmed, "ples put the right OTP");

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "user not found");
        

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded) 
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return RequestResult<bool>.Failure(ErrorCode.UnknownError, errors);

        }
        return RequestResult<bool>.Success(true);
    }
}