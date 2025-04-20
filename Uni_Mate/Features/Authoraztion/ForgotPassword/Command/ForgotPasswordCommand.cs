using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;

namespace Uni_Mate.Features.Authoraztion.ForgotPassword.Command
{
    public record ForgotPasswordCommand(string Email, string ClientUrl): IRequest<RequestResult<bool>>;

    public class ForgotPasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ForgotPasswordCommand,RequestResult<bool>>
    {
        public ForgotPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.EmailNotConfirmed, "There Is No Such Email");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var confirmationLink = $"{request.ClientUrl}?Email={user.Email}&Token={token}";
            // Send confirmation email with the link
            var sendEmail = await _mediator.Send(new SendEmailQuery(request.Email, "Confirm your email", confirmationLink));

            if (!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            return RequestResult<bool>.Success(true, "please check your email");
        }
    }
}
