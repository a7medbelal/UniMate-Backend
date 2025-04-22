using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.RegisterStudent;

public record RegisterStudentRequestViewModel(string UserName, string Email, string Password, string FName, string LName, string PhoneNo, string nationalID, string Role = "Student"); // Default role is Student

public class RegisterStudentRequestViewModelValidator : AbstractValidator<RegisterStudentRequestViewModel>
{
    public RegisterStudentRequestViewModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User Name is required.");

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
            .Matches(@"^[a-zA-Z]+$").WithMessage("First name must contain only letters.");

        RuleFor(x => x.LName)
            .NotEmpty().WithMessage("Last Name is required.")
            .Matches(@"^[a-zA-Z]+$").WithMessage("Last name must contain only letters.");

        RuleFor(x => x.PhoneNo)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Please provide a valid phone number.");

        RuleFor(x => x.nationalID)
            .NotEmpty().WithMessage("NAtional ID is required.")
            .Length(14).WithMessage("ID must be 14 numbers long.")
            .Matches(@"^\d{14}$").WithMessage("National ID must contain only numbers.");

    }
}