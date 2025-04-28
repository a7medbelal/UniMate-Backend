using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.Encoders;
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
        private readonly IConfiguration _configuration;
        public ForgotPasswordCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters, IConfiguration configuration) : base(parameters)
        {
            _configuration = configuration;
        }

        public override async Task<RequestResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellation)
        {
            // Check if the user exists with the provided email
            // just fetch the needed data  
            //var user = await _repositoryIdentity.Get(c=> c.Email == request.Email).
            //    Select(c=>new { c.Id,
            //       c.Email,
            //       c.ResetPassword,
            //       c.ResetPasswowrdConfirnation,  
            //    }).FirstOrDefaultAsync();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                  return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.UserNotFound, "There Is No Such Email");

            //// Generate password reset token
            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);



            #region Generate token without identity 

            var token = Guid.NewGuid().ToString().Substring(0, 6);
            var encodedToken = Uri.EscapeDataString(token);
            //var userr = new User
            //{
            //    Id = user.Id,
            //    ResetPassword = token,
            //    ResetPasswowrdConfirnation = DateTime.UtcNow.AddMinutes(15)
            //};
            user.ResetPassword = token;
            user.ResetPasswowrdConfirnation = DateTime.UtcNow.AddMinutes(15);

            await _repositoryIdentity.SaveIncludeAsync(user, nameof(user.ResetPassword), nameof(user.ResetPasswowrdConfirnation));

            await _repositoryIdentity.SaveChangesAsync();

            #endregion

            
            //Encode the token for URL
            // Build the final URL
            var frontendResetUrl = _configuration["Frontend:ResetPasswordUrl"];
         
            var resetUrl = $"{frontendResetUrl}?email={user.Email}&otp={encodedToken}";
            
            
            
            // Send confirmation email with the link
            var sendEmail = await _mediator.Send(new SendEmailQuery(request.Email, $"Confirm your email {token}" ,resetUrl));

            if (!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            
            
            return RequestResult<bool>.Success(true, "please check your email");
        }
    }
}
