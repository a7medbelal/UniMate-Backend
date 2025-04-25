using Azure;
using FluentValidation;
using Uni_Mate.Common.BaseEndpoints;

namespace Uni_Mate.Features.Authoraztion.ForgotPassword
{
    public record ForgotPasswordEndpointViewModel(string Email);

    public class ForgotPasswordEndpointViewModelValidator : AbstractValidator<ForgotPasswordEndpointViewModel>
    {
        public ForgotPasswordEndpointViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.");

        }
    }
}