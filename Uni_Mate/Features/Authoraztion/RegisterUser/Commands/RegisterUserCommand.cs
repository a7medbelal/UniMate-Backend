using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.Commands
{

    public record RegisterUserCommand(string Fname,string Lname, string UserName, string Email, string Password, string NationalId, IFormFile FrontPersonalImage, IFormFile BackPersonalImage) : IRequest<RequestResult<bool>>;

    public class RegisterUserCommandHandler : BaseWithoutRepositoryRequestHandler<RegisterUserCommand,RequestResult<bool> , Student>
    {
        public RegisterUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<bool>>Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            // Check if the user already exists with the same email or national ID  or UserName
            var UserExist = await _repositoryIdentity.Get(c => c.Email == request.Email || c.National_Id == request.NationalId || c.UserName == request.UserName ).FirstOrDefaultAsync();
            if (UserExist is not null  && UserExist.EmailConfirmed == true )
                return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExists, "Student already exists");

			// Upload the front and back images of the student's national ID to Cloudinary using the UploadImageCommand, and retrieves the corresponding image URLs
			var frontImageUrl = await _mediator.Send(new UploadImageCommand(request.FrontPersonalImage));
			var backImageUrl = await _mediator.Send(new UploadImageCommand(request.BackPersonalImage));

			// Check if the image upload was successful before creating a new student
			if (!frontImageUrl.isSuccess || !backImageUrl.isSuccess)
			{
				return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload the image!");
			}

			// Create a new student
			var user = new Student 
            {
                UserName = request.UserName,
                Email = request.Email,
                Fname = request.Fname,
                Lname = request.Lname,
                National_Id = request.NationalId,
				FrontPersonalImage = frontImageUrl.data,
				BackPersonalImage = backImageUrl.data,
				role = Role.Student,
            };

            var result = await _userManager.CreateAsync(user, request.Password); 

		

            // Check if the user was created successfully
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return RequestResult<bool>.Failure(ErrorCode.UserCreationFailed,errors);
            }


            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // we shoud have an endpoint that URl Goes to that endpoint to put the OTP and confirm the email
            // if some thing happen when the sending email we should not create the user i should resend the emao agin 
            var confirmationLink = $"Dear {user.UserName},<br/><br/>" +
                            "Thank you for registering. Please confirm your email address by clicking the link below:<br/><br/>" +
                            $"<a href='https://uni-mate-web.vercel.app/confirmemail?email={user.Email}&OTP={token}'>Click here to confirm your email</a><br/><br/>" +
                            "If you did not request this, please ignore this message.<br/><br/>" +
                            "Best regards,<br/>";

            // Send confirmation email with the link

            var sendEmail =  await _mediator.Send(new SendEmailQuery(request.Email, "Confirm your email", confirmationLink));

            if(!sendEmail.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EmailSendingFailed, "Email sending failed");

            return RequestResult<bool>.Success(true, "please check your email");
        }
    }

}