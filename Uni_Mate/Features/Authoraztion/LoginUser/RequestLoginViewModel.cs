using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.LoginUser
{
    public record RequestLoginViewModel(string Email , string Password); 
   public class RequestLoginViewModelValidator : AbstractValidator<RequestLoginViewModel>
    {
        public RequestLoginViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email or ID  is required ");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
