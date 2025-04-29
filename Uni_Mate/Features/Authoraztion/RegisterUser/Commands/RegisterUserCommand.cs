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

    public record RegisterUserCommand(string Fname,string Lname, string UserName, string email, string password , string NationalId) : IRequest<RequestResult<bool>>;

    public class RegisterUserCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterUserCommand,RequestResult<bool> , Student>
    {
        public RegisterUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<bool>>Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            // Check if the user already exists with the same email or national ID  or UserName
            var UserExist = await _repositoryIdentity.AnyAsync(c => c.Email == request.email || c.National_Id == request.NationalId || c.UserName == request.UserName);
            if (UserExist)
                return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "Student already exists");

            // Create a new student
            var user = new Student 
            {
                UserName = request.UserName,
                Email = request.email,
                Fname = request.Fname,
                Lname = request.Lname,
                National_Id = request.NationalId,
                role = Role.Student,
            };

            var result = await _userManager.CreateAsync(user, request.password); 

		

            // Check if the user was created successfully
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return RequestResult<bool>.Failure(ErrorCode.UserCreationFailed,errors);
            }


            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // we shoud have an endpoint that URl Goes to that endpoint to put the OTP and confirm the email

            var confirmationLink = $"Dear {user.UserName},<br/><br/>" +
                            "Thank you for registering. Please confirm your email address by clicking the link below:<br/><br/>" +
                            $"<a href='http://darkteam.runasp.net/ConfirmEmailEndpoint/ConfirmEmail?email={user.Email}&OTP={token}'>Click here to confirm your email</a><br/><br/>" +
                            "If you did not request this, please ignore this message.<br/><br/>" +
                            "Best regards,<br/>";


            // Send confirmation email with the link
            var sendEmail =  await _mediator.Send(new SendEmailQuery(request.email, "Confirm your email", confirmationLink));

            if(!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            return RequestResult<bool>.Success(true, "please check your email");
        }
    }

}