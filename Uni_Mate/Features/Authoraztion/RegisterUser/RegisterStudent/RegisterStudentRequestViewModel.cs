using FluentValidation;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.RegisterStudent;

public record RegisterStudentRequestViewModel(string Fname, string Lname, string UserName, string Email, string Password, string ConfrimPassword, string NationalId, IFormFile FrontPersonalImage, IFormFile BackPersonalImage);

public class RegisterStudentRequestViewModelValidator : AbstractValidator<RegisterStudentRequestViewModel>
{
    public RegisterStudentRequestViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Please enter a correctly formatted email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).")
            .Must(email => !email.Contains("gamil.com")).WithMessage("Did you mean 'gmail.com'? Please check your email.");

		RuleFor(x => x.Fname)
				.NotEmpty().WithMessage("First Name is required.")
				.Matches(@"^[\p{L}]+$").WithMessage("First name must contain only letters.");

		RuleFor(x => x.Lname)
			.NotEmpty().WithMessage("Last Name is required.")
			.Matches(@"^[\p{L}]+$").WithMessage("Last name must contain only letters.");

		RuleFor(x => x.ConfrimPassword)
            .NotEmpty().WithMessage("Confrim Password is required.")
            .Equal(x => x.Password).WithMessage("Password and Confirm Password must match.");


        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required")
            .Length(14).WithMessage("National ID must be exactly 14 digits")
            .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits")
            .Custom((nationalId, context) =>
            {
                var result = BeValidEgyptianId(nationalId);
                if (!result.isSuccess)
                {
                    context.AddFailure(result.message); // your custom error
                }
            });

        RuleFor(x => x.FrontPersonalImage)
			.NotEmpty().WithMessage("Front Personal Image is required.")
			.Must(x => x.Length > 0).WithMessage("Front Personal Image is required.")
			.Must(x => x.ContentType == "image/jpeg" || x.ContentType == "image/png").WithMessage("Front Personal Image must be a JPEG or PNG file.");
		RuleFor(x => x.BackPersonalImage)
			.NotEmpty().WithMessage("Back Personal Image is required.")
			.Must(x => x.Length > 0).WithMessage("Back Personal Image is required.")
			.Must(x => x.ContentType == "image/jpeg" || x.ContentType == "image/png").WithMessage("Back Personal Image must be a JPEG or PNG file.");

	}


    private RequestResult<bool> BeValidEgyptianId(string Naltional_ID)
    {
        int century = int.Parse(Naltional_ID[0].ToString());
        int year = int.Parse(Naltional_ID.Substring(1, 2));
        int month = int.Parse(Naltional_ID.Substring(3, 2));
        int day = int.Parse(Naltional_ID.Substring(5, 2));
        int govCode = int.Parse(Naltional_ID.Substring(7, 2));

        // Determine the full year
        int fullYear = century switch
        {
            2 => 1900 + year,
            3 => 2000 + year,
            _ => -1
        };

        if (fullYear == -1) return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Invalid century in National ID");

        // Validate date
        var dob = new DateTime(fullYear, month, day);
        if (dob > DateTime.Now) return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Date of birth cannot be in the future");

        // Validate governorate code
        var validGovCodes = new HashSet<int>
            {
                1, 2, 3, 4, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29
            };

        if (!validGovCodes.Contains(govCode))
            return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Invalid governorate code in National ID");


        return RequestResult<bool>.Success(true, "Valid National ID");

    }



}


