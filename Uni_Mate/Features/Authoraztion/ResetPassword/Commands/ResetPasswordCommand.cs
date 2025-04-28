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
    public record ResetPasswordCommand(string email, string OTP,  string Password, string ConfirmPassword ): IRequest<RequestResult<bool>>;

    public class ResetPasswordCommandHandler : BaseWithoutRepositoryRequestHandler<ResetPasswordCommand, RequestResult<bool>, User>
    {

        public ResetPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters , IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
    
        }
        public override async Task<RequestResult<bool>> Handle(ResetPasswordCommand resetPasswordCommand, CancellationToken cancellationToken)
        {
            //Check if the user exists with the provided email
            // just fetch the needed data
            //var user = await _repositoryIdentity.Get(c => c.Email == resetPasswordCommand.email).Select(c => new User { Id = c.Id, Email = c.Email, PasswordHash = c.PasswordHash }).FirstOrDefaultAsync();

            var user = await _userManager.FindByEmailAsync(resetPasswordCommand.email);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User Not Found");
            }
           // var decodedToken = Uri.UnescapeDataString(resetPasswordCommand.OTP);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordCommand.OTP , resetPasswordCommand.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return RequestResult<bool>.Failure(ErrorCode.UnknownError, errors);
            }
            return RequestResult<bool>.Success(true, "Password Reset Successfully");


            //var userExist = await _repositoryIdentity.Get(u => u.Email == resetPasswordCommand.email).FirstOrDefaultAsync();

            //if (userExist == null) return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "ples check is the email exsit or otp that genetrate doesnot expire");

            //var decodedToken = Base64UrlEncoder.Decode(resetPasswordCommand.OTP);
            //var result = await _userManager.ResetPasswordAsync(user, resetPasswordCommand.OTP, resetPasswordCommand.Password);
            //if (!result.Succeeded)
            //{
            //    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            //    return RequestResult<bool>.Failure(ErrorCode.UnknownError, errors);
            //}

            //var hasher = new PasswordHasher<string>();

            //var Newpassword = hasher.HashPassword(null, resetPasswordCommand.Password);

            //var userr = new User()
            //{
            //    Id = userExist.Id,
            //    PasswordHash= Newpassword,
            //};

            //await _repositoryIdentity.SaveIncludeAsync(userr, nameof(userr.PasswordHash));

            //await _repositoryIdentity.SaveChangesAsync();

            //return RequestResult<bool>.Success(true , "");


        }
    }
}
