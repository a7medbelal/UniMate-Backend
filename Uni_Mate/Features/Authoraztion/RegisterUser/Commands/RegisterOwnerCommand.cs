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
	public record RegisterOwnerCommand(string Email, string Password, string FName, string LName, string PhoneNo) : IRequest<RequestResult<bool>>;

	public class RegisterOwnerCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterOwnerCommand, RequestResult<bool> , Owner>
	{
		public RegisterOwnerCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters) : base(parameters) { }

		public async override Task<RequestResult<bool>> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
		{
			var userExist = await _userManager.FindByEmailAsync(request.Email);
			if (userExist != null)
				return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "User Email already exists");

			var user = new Owner
			{
				Email = request.Email,
				PhoneNumber = request.PhoneNo,
				Fname = request.FName,
				UserName = request.Email,
				Lname = request.LName,
				role = Role.Owner,
				
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
				return RequestResult<bool>.Failure(ErrorCode.UserCreationFailed, string.Join(", ", result.Errors.Select(e => e.Description)));

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = $"Dear {user.UserName},<br/><br/>" +
							"Thank you for registering. Please confirm your email address by clicking the link below:<br/><br/>" +
							$"<a href='http://darkteam.runasp.net/ConfirmEmailEndpoint/ConfirmEmail?email={user.Email}&OTP={token}'>Click here to confirm your email</a><br/><br/>" +
							"If you did not request this, please ignore this message.<br/><br/>" +
							"Best regards,<br/>";


            var sendEmail = await _mediator.Send(new SendEmailQuery(user.Email, "Confirm your email", confirmationLink));

			if (!sendEmail.isSuccess)
				return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

			return RequestResult<bool>.Success(true, "Please check your email");
		}
	}

}