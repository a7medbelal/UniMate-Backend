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
	public record RegisterUserCommand(string Email, string Password, string FName, string LName, string PhoneNo, string nationalID, string Role = "Student", string? UserName = null) : IRequest<RequestResult<bool>>; // Defult role is student unless specified in the request from the frontend

	public class RegisterUserCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterUserCommand,RequestResult<bool>>
    {
        public RegisterUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<bool>>Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
			// Check if the user already exists (through checking if the email exists)
			var userExist = await _userManager.FindByEmailAsync(request.Email);
			if (userExist != null)
			{
				return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "User Email already exists");
			}

			// If the role is student, check username and national ID
			if (request.Role.ToLower() == "student")
			{
				if (string.IsNullOrEmpty(request.UserName))
					return RequestResult<bool>.Failure(ErrorCode.MissingUserName, "Username is required for Student.");

				if (string.IsNullOrEmpty(request.nationalID))
					return RequestResult<bool>.Failure(ErrorCode.MissingNationalID, "National ID is required for Student.");
			}

			// Create user with the provided role
			var user = new User
			{
				Email = request.Email,
				PhoneNumber = request.PhoneNo,
				Fname = request.FName,
				Lname = request.LName,
				role = request.Role.ToLower() == "student" ? Role.Student : Role.Owner,
				ID = request.Role.ToLower() == "student" ? request.nationalID : null,
				UserName = request.Role.ToLower() == "student" ? request.UserName : null
			};
			var result = _userManager.CreateAsync(user, request.Password); 

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