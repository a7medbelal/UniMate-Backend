using FluentValidation;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using System.Threading.Tasks;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Authoraztion.ResetPassword.Commands
{
    public record ResetPasswordCommand(string email, string OTP,  string Password, string ConfirmPassword ): IRequest<RequestResult<bool>>;

    public class ResetPasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ResetPasswordCommand, RequestResult<bool>, User>
    {
     // IHttpContextAccessor _HttpContextAccessor;

        public ResetPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters , IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
           //_HttpContextAccessor = httpContextAccessor;
        }
        public override async Task<RequestResult<bool>> Handle(ResetPasswordCommand resetPasswordCommand, CancellationToken cancellationToken)
        {


           // var user = await _repositoryIdentity.Get(c=> c.Email == resetPasswordCommand.email).Select(c=> new {})
           var user = await _userManager.FindByEmailAsync(resetPasswordCommand.email);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User Not Found");
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordCommand.OTP, resetPasswordCommand.Password);
            if (result.Succeeded == false)
            {
                return RequestResult<bool>.Failure(ErrorCode.EmailNotConfirmed, "There Is Error In Rest The Password Or Invalid Token");
            }
            return RequestResult<bool>.Success(true, "Password Reset Successfully");
        }
    }
}
