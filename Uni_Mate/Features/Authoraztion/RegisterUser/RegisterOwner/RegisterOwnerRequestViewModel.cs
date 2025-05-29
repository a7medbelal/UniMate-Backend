using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.RegisterOwner
{
	public record RegisterOwnerRequestViewModel(string Email, string Password, string FName, string LName, string PhoneNo); // Given through an input from the frontend

	public class RegisterOwnerRequestViewModelValidator : AbstractValidator<RegisterOwnerRequestViewModel>
	{
		public RegisterOwnerRequestViewModelValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Please provide a valid email address.")
				.Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Please enter a correctly formatted email address.")
				.Must(email => !email.Contains("gamil.com")).WithMessage("Did you mean 'gmail.com'? Please check your email.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
				.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches(@"\d").WithMessage("Password must contain at least one number.")
				.Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).");

			RuleFor(x => x.FName)
				.NotEmpty().WithMessage("First Name is required.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("First name must contain only letters.");

			RuleFor(x => x.LName)
				.NotEmpty().WithMessage("Last Name is required.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("Last name must contain only letters.");

			RuleFor(x => x.PhoneNo)
				.NotEmpty().WithMessage("Phone number is required.")
				.Matches(@"^\+?\d{10,15}$").WithMessage("Please provide a valid phone number.");
		}
	}
}