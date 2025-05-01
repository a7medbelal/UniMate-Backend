using FluentValidation;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using System.Threading.Tasks;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.UserManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Uni_Mate.Features.Authoraztion.ResetPassword.Commands
{
    public record ResetPasswordWithOutIdentity(string email, string OTP,  string Password, string ConfirmPassword ): IRequest<RequestResult<bool>>;

    public class ResetPasswordWithOutIdentityHandler : BaseWithoutRepositoryRequestHandler<ResetPasswordWithOutIdentity, RequestResult<bool>, User>
    {

        public ResetPasswordWithOutIdentityHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters , IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
    
        }
        public override async Task<RequestResult<bool>> Handle(ResetPasswordWithOutIdentity resetPasswordCommand, CancellationToken cancellationToken)
        {
            var userExist = await _repositoryIdentity.Get(u => u.Email == resetPasswordCommand.email).FirstOrDefaultAsync();

            if (userExist == null) return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "ples check is the email exsit or otp that genetrate doesnot expire");

            var decodedToken = Uri.UnescapeDataString(resetPasswordCommand.OTP);
  
            if (userExist.ResetPassword != decodedToken || userExist.ResetPasswowrdConfirnation < DateTime.UtcNow)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidOTP, "Invalid OTP");
            }

            var hasher = new PasswordHasher<string>();

            var Newpassword = hasher.HashPassword(null, resetPasswordCommand.Password);

            userExist.PasswordHash = Newpassword;

            await _repositoryIdentity.SaveIncludeAsync(userExist, nameof(userExist.PasswordHash));

            await _repositoryIdentity.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "");

        }
    }
}
