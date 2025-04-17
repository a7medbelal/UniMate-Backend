using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.LoginUser
{
    public record RequestLoginViewModel(string Email , string Password); 
   public class RequestLoginViewModelValidator : AbstractValidator<RequestLoginViewModel>
    {
        public RequestLoginViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a correctly formatted email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
