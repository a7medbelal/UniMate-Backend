using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Authoraztion.ForgotPassword.Command
{
    public record ForgotPasswordCommand(string Email): IRequest<RequestResult<bool>>;

    public class ForgotPasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ForgotPasswordCommand,RequestResult<bool>, User>
    {
        public ForgotPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.UserNotFound, "There Is No Such Email");
            }

            var tokenForAuthen = await _tokenHelper.GenerateToken(user.Id.ToString());

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var emailBody = $"Your OTP is {token}";
            // Send confirmation email with the link
            var sendEmail = await _mediator.Send(new SendEmailQuery(request.Email, "Confirm your email", emailBody));

            if (!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            return RequestResult<bool>.Success(true, "please check your email");
        }
    }
}
