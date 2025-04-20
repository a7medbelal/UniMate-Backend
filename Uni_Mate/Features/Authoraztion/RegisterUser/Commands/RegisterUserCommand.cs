using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.Commands
{
    public record RegisterUserCommand(string UserName,string email, string password, string name, string phoneNo, string country) : IRequest<RequestResult<bool>>;

    public class RegisterUserCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterUserCommand,RequestResult<bool>>
    {
        public RegisterUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<bool>>Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if the user already exists
            var UserExist = await _userManager.FindByEmailAsync(request.email);
            if (UserExist != null)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "Student already exists");
            } 

            // Create a new student
            var user = new User 
            {
                UserName = request.UserName,
                Email = request.email,
                PhoneNumber = request.phoneNo,
                Fname = request.name,
                Lname = request.name,
                role = Role.Student,
                Address = request.country
            };
            var result = _userManager.CreateAsync(user, request.password); 

            // Check if the user was created successfully
            if (!result.Result.Succeeded)
            {
                var errors = string.Join(", ", result.Result.Errors.Select(e => e.Description));

                return RequestResult<bool>.Failure(ErrorCode.UserCreationFailed,errors);
            }


            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // we shoud have an endpoint that URl Goes to that endpoint to put the OTP and confirm the email
            var confirmationLink = $"http://localhost:5187/ConfirmEmailEndpoint/ConfirmEmail?Email={user.Email}&Token={token}";
            // Send confirmation email with the link
           var sendEmail =  await _mediator.Send(new SendEmailQuery(user.Email, "Confirm your email", confirmationLink));

            if(!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            return RequestResult<bool>.Success(true, "please check your email");
        }
    }

}
