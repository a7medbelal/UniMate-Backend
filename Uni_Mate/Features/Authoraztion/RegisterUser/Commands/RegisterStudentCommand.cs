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
	public record RegisterStudentCommand(string Email, string Password, string FName, string LName, string PhoneNo, string NationalID, string UserName) : IRequest<RequestResult<bool>>;

	public class RegisterStudentCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterStudentCommand, RequestResult<bool>>
	{
		public RegisterStudentCommandHandler(BaseWithoutRepositoryRequestHandlerParameters parameters) : base(parameters) { }

		public async override Task<RequestResult<bool>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
		{
			var userExist = await _userManager.FindByEmailAsync(request.Email);
			if (userExist != null)
				return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "User Email already exists");

			if (string.IsNullOrEmpty(request.UserName))
				return RequestResult<bool>.Failure(ErrorCode.MissingUserName, "Username is required for Student.");

			if (string.IsNullOrEmpty(request.NationalID))
				return RequestResult<bool>.Failure(ErrorCode.MissingNationalID, "National ID is required for Student.");

			var user = new User
			{
				Email = request.Email,
				PhoneNumber = request.PhoneNo,
				Fname = request.FName,
				Lname = request.LName,
				role = Role.Student,
				ID = request.NationalID,
				UserName = request.UserName
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
				return RequestResult<bool>.Failure(ErrorCode.UserCreationFailed, string.Join(", ", result.Errors.Select(e => e.Description)));

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = $"http://localhost:5187/ConfirmEmailEndpoint/ConfirmEmail?Email={user.Email}&Token={token}";

			var sendEmail = await _mediator.Send(new SendEmailQuery(user.Email, "Confirm your email", confirmationLink));

			if (!sendEmail.isSuccess)
				return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

			return RequestResult<bool>.Success(true, "Please check your email");
		}
	}

}