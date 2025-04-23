using FluentValidation;
namespace Uni_Mate.Features.Authoraztion.ConfirmRegistration;

public record ConfirmEmailViewModel(string Email, string OTP);

public class ConfirmRegistrationViewModelValidator : AbstractValidator<ConfirmEmailViewModel>
{
    public ConfirmRegistrationViewModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.");
        
        RuleFor(x => x.OTP)
            .NotEmpty().WithMessage("Token is required.")
            .MinimumLength(6).WithMessage("Token must be at least 6 characters long.");
    }
}