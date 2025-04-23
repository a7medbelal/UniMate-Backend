using FluentValidation;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.Authoraztion.RegisterUser;

public record RegisterUserRequestViewModel(string UserName,string Email, string Password, string Fname , string Lname ,  string PhoneNo, string NationalId );

public class RegisterUserRequestViewModelValidator : AbstractValidator<RegisterUserRequestViewModel>
{
    public RegisterUserRequestViewModelValidator()
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
            .NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Lname)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(x => x.PhoneNo)  
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Please provide a valid phone number.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required")
            .Length(14).WithMessage("National ID must be exactly 14 digits")
            .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits")
            .Must(BeValidEgyptionId).WithMessage("National ID must Conatain only digidts ");
    }

    private bool BeValidEgyptionId (string Naltional_ID)
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

        if (fullYear == -1) return false;

        // Validate date
        var dob = new DateTime(fullYear, month, day);
        if (dob > DateTime.Now) return false;

        // Validate governorate code
        var validGovCodes = new HashSet<int>
            {
                1, 2, 3, 4, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29
            };

        if (!validGovCodes.Contains(govCode))
            return false;


        return true; 

    }



}


