using FluentValidation;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using System.Threading.Tasks;
using Uni_Mate.Common.Data.Enums;

namespace Uni_Mate.Features.Authoraztion.ResetPassword.Commands
{
    public record ResetPasswordCommand(string Password, string ConfirmPassword, string Email, string Token): IRequest<RequestResult<bool>>;

    public class ResetPasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ResetPasswordCommand, RequestResult<bool>>
    {


        public ResetPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<bool>> Handle(ResetPasswordCommand resetPasswordCommand, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordCommand.Email);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "There Is No Such Email");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordCommand.Token, resetPasswordCommand.Password);
            if (result.Succeeded == false)
            {
                return RequestResult<bool>.Failure(ErrorCode.EmailNotConfirmed, "There Is Error In Rest The Password Or Invalid Token");
            }
            return RequestResult<bool>.Success(true, "Password Reset Successfully");
        }
    }
}
