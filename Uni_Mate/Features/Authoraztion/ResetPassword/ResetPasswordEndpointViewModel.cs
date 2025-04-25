using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.ResetPassword
{
    public record ResetPasswordEndpointViewModel(string Password, string ConfirmPassword ,string Token);
    public class ResetPasswordEndpointViewModelValidator : AbstractValidator<ResetPasswordEndpointViewModel>
    {
        public ResetPasswordEndpointViewModelValidator()
        {
            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).")
            .Must(email => !email.Contains("gamil.com")).WithMessage("Did you mean 'gmail.com'? Please check your email.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm Password is required.")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            //RuleFor(x => x.Email)
            //    .NotEmpty()
            //    .WithMessage("Email is required.")
            //    .EmailAddress()
            //    .WithMessage("Invalid email format.");

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token is required.").
                MinimumLength(6).WithMessage("Token must be at least 6 characters long.");
        }
    }
}
